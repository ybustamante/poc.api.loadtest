#!/bin/bash
set -e

mkdir -p /etc/ssl
echo "$SSL_Server_Certificate" >> /etc/ssl/SSL_Server_Certificate.cer
echo "$SSL_Server_key" >> /etc/ssl/SSL_Server_key.pem
echo "$SSL_Client_Certificate" >> /etc/ssl/SSL_Client_Certificate.cer

exec "$@"