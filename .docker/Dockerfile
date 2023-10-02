FROM ubuntu

EXPOSE 7777 7778

COPY server/ /root/server/
COPY entrypoint.sh /root/entrypoint.sh

WORKDIR /root/
ENTRYPOINT ["/bin/bash", "./entrypoint.sh"]
