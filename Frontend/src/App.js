import React from 'react';
import './App.css';

import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';
import Home from './pages';
import SigninPage from './pages/signin';

function App() {
  return (
    <Router>
      <Switch>
        <Route path='/signin' component={SigninPage} exact />
        <Route path='/' component={Home} exact />
      </Switch>
    </Router>
  );
}

export default App;


