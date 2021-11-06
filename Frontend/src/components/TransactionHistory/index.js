import React from 'react';
import { Redirect, withRouter } from 'react-router';
import { Columns } from './Columns';
import TransactionTable from './TransactionTable';
import {
  Container,
  TableWrap
} from './TransactionElements';

class TransactionHistory extends React.Component {
    constructor (props) { 
        super(props);

        this.state = {
            redirect: null,
            pageSize: 30,
            pageNumber: 0,
            user: {},
            transactions: []
        };

        //this.getTransactions();

        // this.handleChange = this.handleChange.bind(this);
    }

    componentDidMount() {
        const id = this.props.match.params["id"];
        const localUser = JSON.parse(window.localStorage.getItem("user"));
        this.setState({user:localUser}, () => {
            if (!this.verifyLoggedIn()) {
                this.setState({redirect: "/signin"});
            }
            else if (this.state.user["accounts"].length - 1 < id){
                this.setState({redirect: "./0"}, () => {
                    this.setState({redirect: null});
                });
                this.getTransactions(0);
            }
            else{
                this.getTransactions(id); 
            }
        });
    }
    
    //   handleChange = event => {
    //     this.setState({
    //         [event.target.name]: event.target.value
    //     });
    //   }

    verifyLoggedIn() {
        if (this.state.user === {}) {
            return false;
        }
        else if (this.state.user["tokenExpires"] <= Date.now()){
            return false;
        }
        return true;
    }
    
    getTransactions(id) {
        let API = "https://localhost:44347/api/";
        let query = "getTransactionHistory";
        const accountId = this.state.user["accounts"].map(account => account["accountId"]).sort((a,b)=>a-b)[id];
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
        .then(response => response.json())
        .then(json => this.setState({transactions: json['transactions']}));
    };

    render() {
        if (this.state.redirect) {
            return <Redirect to={this.state.redirect} />
        }
        return (
            <div>
                <TransactionTable columns={Columns} data={this.state.transactions} />
            </div>
        );
    }
}

export default withRouter(TransactionHistory);
