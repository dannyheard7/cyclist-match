import React from 'react';
import { createRoot } from 'react-dom/client';
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
    const container = document.getElementById('root');
    const root = createRoot(container!);
    root.render(<App />);
}

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
