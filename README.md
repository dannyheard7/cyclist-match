![{{alt text}}](https://github.com/dannyheard7/cycling-buddies/workflows/Deploy%20-%20Production/badge.svg)

```
choco install mkcert
mkcert -install
mkdir -p certs
mkcert -cert-file certs/local-cert.pem -key-file certs/local-key.pem "oidc.localhost"
```
