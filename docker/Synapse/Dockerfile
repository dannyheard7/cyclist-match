FROM matrixdotorg/synapse:v1.52.0

COPY ./certs/local-cert.pem /usr/local/share/ca-certificates/local-cert.crt
RUN update-ca-certificates

COPY ./certs/root-cert.pem /usr/local/share/ca-certificates/local-root-cert.crt
RUN cat /usr/local/share/ca-certificates/local-root-cert.crt /usr/local/share/ca-certificates/local-cert.crt > /usr/local/share/ca-certificates/full.crt

ENV LOCAL_CERT=/usr/local/share/ca-certificates/full.crt

COPY ./docker/Synapse/cert-hack.py cert-hack.py
COPY ./docker/Synapse/startup.sh startup.sh

ENTRYPOINT [ "/bin/sh", "startup.sh" ]