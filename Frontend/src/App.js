import React from 'react';
import './App.css';

import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';
import Home from './pages';
import SigninPage from './pages/signin';
import SignoutPage from './pages/signout';
import TransactionHistoryPage from './pages/Customer/TransactionHistoryPage';
import DashboardPage from './pages/Customer/DashboardPage';

function App() {
  return (
    <Router>
      <Switch>
        <Route path='/' component={Home} exact />
        <Route path='/signin' component={SigninPage} exact />
        <Route path='/signout' component={SignoutPage} exact />
        <Route path='/customer/account/:id' component={TransactionHistoryPage} exact />
        <Route path='/customer/dashboard' component={DashboardPage} exact />
      </Switch>
    </Router>
  );
}

export default App;
