import React from 'react';
import { Redirect, withRouter } from 'react-router';
import { Columns } from './Columns';
import TransactionTable from './TransactionTable';
import './style.css';

class TransactionHistory extends React.Component {
    constructor (props) { 
        super(props);

        this.state = {
            redirect: null,
            id: 0,
            pageSize: 30,
            pageNumber: 0,
            user: {},
            transactions: []
        };
    }

    componentDidMount() {
        const paramId = this.props.match.params["id"];
        const localUser = JSON.parse(window.localStorage.getItem("user"));
        this.setState({id:paramId, user:localUser}, () => {
            if (!this.verifyLoggedIn()) {
                this.setState({redirect: "/signin"});
            }
            else if (this.state.user["accounts"].length - 1 < this.state.id){
                this.setState({redirect: "./0"}, () => {
                    this.setState({redirect: null});
                });
                this.setState({id:0}, () => this.getTransactions());
            }
            else{
                this.getTransactions(); 
            }
        });
    }

    verifyLoggedIn() {
        if (this.state.user === {}) {
            return false;
        }
        else if (this.state.user["tokenExpires"] <= Date.now()){
            return false;
        }
        return true;
    }
    
    getTransactions() {
        let API = "https://localhost:44347/api/";
        let query = "getTransactionHistory";
        const accountId = this.state.user["accounts"]
        .map(account => account["accountId"])
        .sort((a,b)=>a-b)[this.state.id];
        fetch(API + query, {
            method: 'POST',
            mode: 'cors',
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + this.state.user["token"]
            },
            body: JSON.stringify({
                "accountId":accountId,
                "pageSize":this.state.pageSize, 
                "pageNumber":this.state.pageNumber
            })
        })
        .then(response => {
            if (response.ok) {
                response.json().then(json => this.setState({transactions: json['transactions']}));
            }
            else {
                const newPage = this.state.pageNumber - 1;
                this.setState({pageNumber:newPage});
            }
        });
    };

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
                <div>
                    <TransactionTable columns={Columns} data={this.state.transactions} />
                </div>
                <div>
                <button type="button" onClick={this.decPage} id="left">Previous Page</button>
                <label for="pageSizeSelector">Page Size:</label>
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
