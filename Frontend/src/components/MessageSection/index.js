import React from 'react';
import { Redirect } from 'react-router';
import { RefreshUser, GetUser } from '../SignIn/User';
import { Columns } from './columns';
import TransactionTable from '../TransactionHistory/TransactionTable';

class MessageSection extends React.Component {
    constructor (props) { 
        super(props);

        this.state = {
            redirect: null,
            pageSize: 10,
            pageNumber: 0,
            user: {},
            messages: []
        };
    }

    componentDidMount() {
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
            this.setState({user:response}, () => this.getMessages());
        });
    }

    getMessages() {
        let API = "https://localhost:44347/api/";
        let query = "getMessageHistory";
        fetch(API + query, {
            method: 'POST',
            mode: 'cors',
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + this.state.user["token"]
            },
            body: JSON.stringify({
                "pageSize":this.state.pageSize, 
                "pageNumber":this.state.pageNumber
            })
        })
        .then(response => {
            if (response.ok) {
                response.json()
                .then(json => this.beautifyMessages(json))
                .then(json => this.setState({messages: json}));
            }
            else {
                const newPage = this.state.pageNumber - 1;
                this.setState({pageNumber:newPage});
            }
        });
    };

    beautifyMessages(messages) {
        const MONTHS = ["Jan.", "Feb.", "Mar.", "Apr.", "May", "June", "July", "Aug.", "Sep.", "Oct.", "Nov.", "Dec."];
        const formatTime = (year, month, day) => {
            return MONTHS[month - 1] + " " + day.toString() + ", " + year.toString();
        };
        return messages.map((row) => {
            return {
                date: formatTime(row["timeYear"], row["timeMonth"], row["timeDay"]),
                message: row["contents"]
            };
        })
    }

    decPage = () => {
        if(this.state.pageNumber > 0) {
            const newPage = this.state.pageNumber - 1;
            this.setState({pageNumber:newPage}, () => this.getMessages());
        }
    }

    incPage = () => {
        const newPage = this.state.pageNumber + 1;
        this.setState({pageNumber:newPage}, () => this.getMessages());
    }

    changePageSize = (event) => {
        const newSize = parseInt(event.target.value);
        this.setState({pageSize:newSize}, () => this.getMessages());
    }

    render() {
        if (this.state.redirect) {
            return <Redirect to={this.state.redirect} />
        }
        return (
            <div>
                <h1>Notification Messages</h1>
                <div>
                    <TransactionTable columns={Columns} data={this.state.messages} />
                </div>
                <div>
                <button type="button" onClick={this.decPage} id="left">Previous Page</button>
                <label htmlFor="pageSizeSelector">Page Size:</label>
                <select name="pageSizeSelector" id="pageSizeSelector" onChange={this.changePageSize} value={this.state.pageSize}>
                    <option value="10">10</option>
                    <option value="15">15</option>
                    <option value="30">30</option>
                </select>
                <p>Page Number: {this.state.pageNumber + 1}</p>
                <button type="button" onClick={this.incPage} id="right">Next Page</button>
                </div>
            </div>
        );
    }
}

export default MessageSection;