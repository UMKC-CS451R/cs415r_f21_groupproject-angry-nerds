import React from 'react';
import { Redirect } from 'react-router';

function SignoutPage() {
    window.localStorage.removeItem("user");
    return (
        <Redirect to="/" />
    );
}

export default SignoutPage;
