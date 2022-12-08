#!/bin/bash
# Erstellt die HR Datenbank von \\enterprise\ausbildung\unterricht\unterlagen\resch\Skripten\Datenbanken\DB_HR_Oracle
# für die PL/SQL Übungen

sqlplus system/oracle@//localhost/XEPDB1 <<< "
    DROP USER hr CASCADE;
    CREATE USER hr IDENTIFIED BY oracle;
    GRANT CONNECT, RESOURCE, CREATE VIEW TO hr;
    GRANT UNLIMITED TABLESPACE TO hr;
"

curl https://raw.githubusercontent.com/schletz/Dbi2Sem/master/01_OracleVM/03_Docker/1_hr_cre.sql > /tmp/1_hr_cre.sql
curl https://raw.githubusercontent.com/schletz/Dbi2Sem/master/01_OracleVM/03_Docker/2_hr_popul.sql > /tmp/2_hr_popul.sql
sqlplus system/oracle@//localhost/XEPDB1 /tmp/1_hr_cre.sql
sqlplus system/oracle@//localhost/XEPDB1 /tmp/2_hr_popul.sql
