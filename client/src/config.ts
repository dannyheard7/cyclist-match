declare global {
  interface Window { env: any; }
}

window.env = window.env || {};

const settings = {
  AUTH0_DOMAIN: window.env.AUTH0_DOMAIN || process.env.REACT_APP_AUTH0_DOMAIN,
  AUTH0_CLIENT_ID: window.env.AUTH0_CLIENT_ID || process.env.REACT_APP_AUTH0_CLIENT_ID,
  AUTH0_API_AUDIENCE: window.env.AUTH0_API_AUDIENCE || process.env.REACT_APP_AUTH0_API_AUDIENCE,
  CLIENT_HOST: window.location.protocol + '//' + window.location.hostname + (window.location.port ? ':' + window.location.port : ''),
  API_HOST: window.env.API_HOST || process.env.REACT_APP_API_HOST,
  GA_TRACKING_ID: window.env.GA_TRACKING_ID || process.env.REACT_APP_GA_TRACKING_ID,
  RECAPTCHA_SITE_KEY: window.env.RECAPTCHA_SITE_KEY || process.env.REACT_APP_RECAPTCHA_SITE_KEY
};

export default settings;
