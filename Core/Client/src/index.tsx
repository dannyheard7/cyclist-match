import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import * as serviceWorker from './serviceWorker';

if (window.location.pathname === '/oidc-silent-renew') {
    const oidc = require('oidc-client');
    new oidc.UserManager()
        .signinSilentCallback()
        .then()
        .catch((error: any) => {
            console.error(error);
        });
} else {
    ReactDOM.render(<App />, document.getElementById('root'));
}

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
