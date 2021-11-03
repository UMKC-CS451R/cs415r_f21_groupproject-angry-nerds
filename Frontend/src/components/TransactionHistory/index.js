import React, { useEffect, useState } from 'react';
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
            pageSize: 30,
            pageNumber: 1,
            transactions: []
        };

        //this.getTransactions();

        // this.handleChange = this.handleChange.bind(this);
    }

    componentDidMount() { 
        this.getTransactions(); 
    }
    
    //   handleChange = event => {
    //     this.setState({
    //         [event.target.name]: event.target.value
    //     });
    //   }

    getTransactions() {
        let API = "https://localhost:44347/api/";
        let query = "getTransactionHistory";
        fetch(API + query, {
            method: 'POST',
            mode: 'cors',
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + JSON.parse(window.localStorage.getItem("user"))["token"]
            },
            body: JSON.stringify({
                "accountId":211111110,
                "pageSize":30, 
                "pageNumber":0
            })
        })
        .then(response => response.json())
        .then(json => this.setState({transactions: json['transactions']}));
    };

    render() {
        return (
            <div>
                <TransactionTable columns={Columns} data={this.state.transactions} />
            </div>
        );
    }
}

export default TransactionHistory;
