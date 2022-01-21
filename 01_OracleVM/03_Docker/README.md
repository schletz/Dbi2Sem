# Docker Image als Ersatz für die VM

Die neuste Express Edition (XE) von Oracle gibt es auch als Docker Container. Lade dafür
Docker für dein Betriebssystem von [docs.docker.com](https://docs.docker.com/get-docker/).

Nach der erfolgreichen Installation wird der Container für Oracle 21 XE mittels der folgenden
Befehle geladen und ausgeführt:

```text
docker pull gvenzl/oracle-xe:21-full
docker run -d -p 1521:1521 -e ORACLE_PASSWORD=oracle -v oracle-volume:/opt/oracle/XE21CFULL/oradata gvenzl/oracle-xe:21-full
```

Zum Beenden und Starten des Containers kann nun Docker Desktop verwendet werden.
