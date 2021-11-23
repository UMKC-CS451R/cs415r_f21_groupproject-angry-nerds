import React from 'react';
import { Redirect, withRouter } from 'react-router';
import { RefreshUser, GetUser } from '../SignIn/User';

class AccountSidebar extends React.Component {
    constructor (props) { 
        super(props);

        this.state = {
            redirect: null,
            user: {},
            accounts: []
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
            const sortedAccounts=response["accounts"].sort((a,b)=>a["accountId"]-b["accountId"])
            this.setState({
                user:response, 
                accounts:sortedAccounts
            });
        });
    }

    formatMoney = (dollars, cents) => {
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

    render() {
        if (this.state.redirect) {
            return <Redirect to={this.state.redirect} />
        }
        return (
            <div>
                {this.state.accounts.map((account, index) => {
                    return (
                        <div>
                            <h2><a href={"account/" + index}>{account["typeDescription"]} Account</a></h2>
                            <h3>{"*".repeat(5) + account["accountId"].toString().slice(-4)}</h3>
                            <p>Current Balance: {this.formatMoney(account["endBalanceDollars"], account["endBalanceCents"])}</p>
                        </div>
                    )
                })}
            </div>
        );
    }
}

export default AccountSidebar;
