import React from 'react';
import { Redirect, withRouter } from 'react-router';
import { RefreshUser, GetUser } from '../SignIn/User';
import { Columns } from './Columns';
import TransactionTable from './TransactionTable';
import './TableStyling.js';

class TransactionHistory extends React.Component {
    constructor (props) { 
        super(props);

        this.state = {
            redirect: null,
            id: 0,
            pageSize: 30,
            pageNumber: 0,
            user: {},
            account: {
                accountId: 0,
                endBalanceDollars: 0,
                endBalanceCents: 0,
                typeDescription: ""
            },
            transactions: []
        };
    }

    componentDidMount() {
        let paramId = this.props.match.params["id"];
        const localUserString = window.localStorage.getItem("user");
        console.log(localUserString);

        if (localUserString === null || localUserString === 'undefined'){
            this.setState({redirect: "/signin"});
            return;
        }
        const parsedUser = JSON.parse(localUserString);
        console.log(parsedUser);

        if (parsedUser["tokenExpires"] <= Date.now()) {
            this.setState({redirect:"/signin"});
            return;
        }
        
        RefreshUser(parsedUser)
        .then((refreshedUser) => GetUser(refreshedUser))
        .then((response) => {
            window.localStorage.setItem("user", JSON.stringify(response));
            this.setState({user:response, id:paramId}, () => {
                if (this.state.user["accounts"].length - 1 < this.state.id){
                    this.setState({redirect: "./0"}, () => {
                        this.setState({redirect: null, id:0}, () => this.getTransactions());
                    });
                }
                else{
                    this.getTransactions(); 
                }
            });
        });
    }

    getTransactions() {
        let API = "https://localhost:44347/api/";
        let query = "getTransactionHistory";
        const account = this.state.user["accounts"]
        .sort((a,b)=>a["accountId"]-b["accountId"])[this.state.id];
        this.setState({ account });
        console.log(account);
        fetch(API + query, {
            method: 'POST',
            mode: 'cors',
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + this.state.user["token"]
            },
            body: JSON.stringify({
                "accountId":account["accountId"],
                "pageSize":this.state.pageSize, 
                "pageNumber":this.state.pageNumber
            })
        })
        .then(response => {
            if (response.ok) {
                response.json()
                .then(json => this.beautifyTransactions(json['transactions']))
                .then(json => this.setState({transactions: json}));
            }
            else {
                const newPage = this.state.pageNumber - 1;
                this.setState({pageNumber:newPage});
            }
        });
    };

    beautifyTransactions(transactions) {
        console.log(transactions);
        const MONTHS = ["Jan.", "Feb.", "Mar.", "Apr.", "May", "June", "July", "Aug.", "Sep.", "Oct.", "Nov.", "Dec."];
        const formatTime = (year, month, day) => {
            return MONTHS[month - 1] + " " + day.toString() + ", " + year.toString();
        };
        const formatMoney = (dollars, cents) => {
            let pre = "";
            let post = "";
            if (dollars < 0 || cents < 0) {
                pre = "(" + pre;
                post = post + ")";
            }
            let newCents = Math.abs(cents).toString();
            newCents = (newCents.length === 1) ? "0" + newCents: newCents;
            const newDollars = Math.abs(dollars).toString();
            return "$" + " " + pre + newDollars + "." + newCents + post;
        }
        return transactions.map((row) => {
            return {
                date: formatTime(row["timeYear"], row["timeMonth"], row["timeDay"]),
                transaction: formatMoney(row["amountDollars"], row["amountCents"]),
                balance: formatMoney(row["endBalanceDollars"], row["endBalanceCents"]),
                vendor: row["vendor"]
            };
        })
    }

    decPage = () => {
        if(this.state.pageNumber > 0) {
            const newPage = this.state.pageNumber - 1;
            this.setState({pageNumber:newPage}, () => this.getTransactions());
        }
    }

    incPage = () => {
        const newPage = this.state.pageNumber + 1;
        this.setState({pageNumber:newPage}, () => this.getTransactions());
    }

    changePageSize = (event) => {
        const newSize = parseInt(event.target.value);
        this.setState({pageSize:newSize}, () => this.getTransactions());
    }

    render() {
        if (this.state.redirect) {
            return <Redirect to={this.state.redirect} />
        }
        return (
            <div>
                <h1>{this.state.account["typeDescription"]} Account: {"*".repeat(5) + this.state.account["accountId"].toString().slice(-4)}</h1>
                <div>
                    <TransactionTable columns={Columns} data={this.state.transactions} />
                </div>
                <div>
                <button type="button" onClick={this.decPage} id="left">Previous Page</button>
                <label htmlFor="pageSizeSelector">Page Size:</label>
                <select name="pageSizeSelector" id="pageSizeSelector" onChange={this.changePageSize} value={this.state.pageSize}>
                    <option value="30">30</option>
                    <option value="60">60</option>
                    <option value="90">90</option>
                </select>
                <p>Page Number: {this.state.pageNumber + 1}</p>
                <button type="button" onClick={this.incPage} id="right">Next Page</button>
                </div>
            </div>
        );
    }
}

export default withRouter(TransactionHistory);
