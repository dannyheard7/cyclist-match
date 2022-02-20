import certifi
import requests
import os;

custom_cert_file = os.environ["LOCAL_CERT"]

try:
    requests.get('https://oidc.localhost')
except requests.exceptions.SSLError as err:
    cafile = certifi.where()
    with open(custom_cert_file, 'rb') as infile:
        customca = infile.read()
    with open(cafile, 'ab') as outfile:
        outfile.write(customca)