# Docker Image als Ersatz für die VM

Die neuste Express Edition (XE) von Oracle gibt es auch als Docker Container. Lade dafür
Docker für dein Betriebssystem von [docs.docker.com](https://docs.docker.com/get-docker/).

Die Installation von Docker Desktop und das Laden des Containers ist als Video verfügbar:

<video controls>
<source src="https://youtu.be/K6IMey4u_cA" type="video/mp4">
</video> 

Nach der erfolgreichen Installation wird der Container für Oracle 21 XE mittels der folgenden
Befehle in der Windows Konsole geladen und ausgeführt. Der Container hat rund 3.5 GB.

```text
docker pull gvenzl/oracle-xe:21-full
docker run -d -p 1521:1521 -e ORACLE_PASSWORD=oracle -v oracle-volume:/opt/oracle/XE21CFULL/oradata gvenzl/oracle-xe:21-full
```
Die Umgebungsvariable *ORACLE_PASSWORD* setzt das Systempasswort. Da es keine Produktionsdatenbank
ist, verwenden wir zur Vereinfachung *oracle*.

## Umbenennen des Containers

Zuerst müssen wir herausfinden, welche ID unser Container hat. Der folgende Befehl zeigt
eine Liste aller Container und dessen Daten an:

```text
docker container ls
```

**Ausgabe:**
```text
CONTAINER ID   IMAGE                      COMMAND                  CREATED         STATUS         PORTS                    NAMES
c3231e31cbcf   gvenzl/oracle-xe:21-full   "container-entrypoin…"   4 minutes ago   Up 4 minutes   0.0.0.0:1521->1521/tcp   exciting_chaplygin
```

In diesem Fall hat der Container die ID *c3231e31cbcf*. Beachte, dass die ID immer anders ist.
Ersetze daher die angegebene ID durch den entsprechenden Wert.
Mit *docker rename* kann dieser in einen schöneren Namen, nämlich *oracle21c* umbenannt werden:

```text
docker rename c3231e31cbcf oracle21c
```

## Starten und Stoppen

Nach *docker run* ist unser Container bereits gestartet. Nach einen Neustart bzw. zum Beenden des
Containers sind die zwei folgenden Befehle wichtig:

```text
docker stop oracle21c
docker start oracle21c
```

## Ausführen von Programmen im Container

Mit *docker exec -it oracle21c COMMAND* können Befehle direkt im Container ausgeführt werden.
Die Option *-i* bedeutet eine interaktive Ausführung. *-t* öffnet ein Terminal, sodass nicht CR+LF
von Windows gesendet wird (Linux verwendet nur CR).

### SQL*Plus 

SQL*Plus ist ein Kommandozeilentool, welches direkt SQL Befehle absetzen kann. Wollen wir als
System User direkt Befehle in der pluggable database absetzen, können wir
mittels *docker exec* das Dienstprogramm *sqlplus* starten. Das Passwort ist *oracle* und wurde
im *docker run* Befehl weiter oben als Umgebungsvariable *ORACLE_PASSWORD* gesetzt.

```text
docker exec -it oracle21c sqlplus system/oracle@//localhost/XEPDB1
```

Wollen wir *systemweite Änderungen* machen, gibt es noch den User *SYS*. Hier können Konfigurationen,
die das ganze System betreffen, gelesen und gesetzt werden:

```text
docker exec -it oracle21c sqlplus SYS AS SYSDBA
```

Mit dem Befehl *quit* kann der SQL*Plus Prompt verlassen werden.

### Shell (bash)

Wir können auch eine Shell öffnen und Befehle in Linux absetzen:

```text
docker exec -it oracle21c /bin/bash
```

Mit *exit* kann die Shell verlassen und zu Windows zurückgekehrt werden.
