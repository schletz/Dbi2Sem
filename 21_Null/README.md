# NULL

Unterlagen auf http://griesmayer.com/?menu=Oracle&semester=Semester_5&topic=03_NULL

**Erstellen eines Users NullDemo**

```sql
CREATE USER NullDemo IDENTIFIED BY oracle;
GRANT CONNECT, RESOURCE, CREATE VIEW TO NullDemo;
GRANT UNLIMITED TABLESPACE TO NullDemo;
```

Basis für unsere Abfragen sind folgende 2 Tabellen samt Musterdaten:

**Tabelle PATIENT**

| PATIENT_ID | NAME   | HEALTHY | INVESTIGATIONS | OPERATIONS | DISEASES |
| ---------- | ------ | ------- | -------------- | ---------- | -------- |
| 1          | Hans   | Y       | (NULL)         | 0          | 3        |
| 2          | Werner | (NULL)  | 2              | 2          | 2        |
| 3          | Thomas | N       | 0              | 0          | (NULL)   |
| 4          | Susi   | N       | (NULL)         | 2          | 2        |
| 5          | Alex   | (NULL)  | 1              | (NULL)     | (NULL)   |
| 6          | Kurt   | Y       | 2              | (NULL)     | 1        |
| 7          | Max    | Y       | (NULL)         | (NULL)     | (NULL)   |

**Tabelle DISEASES_CLASS**

| NUMBEROF | CONDITION      |
| -------- | -------------- |
| (NULL)   | NOT APPLICABLE |
| 0        | GOOD           |
| 1        | OK             |
| 2        | POOR           |
| 3        | BAD            |
| 4        | VERY BAD       |

```sql
DROP TABLE PATIENT CASCADE CONSTRAINTS;
DROP TABLE DISEASES_CLASS CASCADE CONSTRAINTS;
CREATE TABLE PATIENT (
    PATIENT_ID INTEGER PRIMARY KEY,
    NAME       VARCHAR2(100) NOT NULL,
    HEALTHY    CHAR(1),
    INVESTIGATIONS INTEGER,
    OPERATIONS     INTEGER,
    DISEASES       INTEGER
);

CREATE TABLE DISEASES_CLASS (
    NUMBEROF  INTEGER,
    CONDITION VARCHAR2(20)
);

INSERT INTO PATIENT VALUES (1, 'Hans',   'Y',    NULL, 0,    3);
INSERT INTO PATIENT VALUES (2, 'Werner',  NULL,  2,    2,    2);
INSERT INTO PATIENT VALUES (3, 'Thomas', 'N',    0,    0,    NULL);
INSERT INTO PATIENT VALUES (4, 'Susi',   'N',    NULL, 2,    2);
INSERT INTO PATIENT VALUES (5, 'Alex',    NULL,  1,    NULL, NULL);
INSERT INTO PATIENT VALUES (6, 'Kurt',   'Y',    2,    NULL, 1);
INSERT INTO PATIENT VALUES (7, 'Max',    'Y',    NULL, NULL, NULL);
COMMIT;

INSERT INTO DISEASES_CLASS VALUES (NULL, 'NOT APPLICABLE');
INSERT INTO DISEASES_CLASS VALUES (0,    'GOOD');
INSERT INTO DISEASES_CLASS VALUES (1,    'OK');
INSERT INTO DISEASES_CLASS VALUES (2,    'POOR');
INSERT INTO DISEASES_CLASS VALUES (3,    'BAD');
INSERT INTO DISEASES_CLASS VALUES (4,    'VERY BAD');
COMMIT;
```

## NULL als Kriterium

```sql
-- Wie viele Personen sind gesund (3)?
SELECT *
FROM PATIENT
WHERE HEALTHY = 'Y';

-- Nun fragen wir das Gegenteil ab, bekommen aber keine NULL Werte für HEALTHY.
SELECT *
FROM PATIENT
WHERE HEALTHY <> 'Y';

-- Wollen wir auch NULL Werte, so können wir das auf 2 Arten lösen:
SELECT *
FROM PATIENT
WHERE HEALTHY IS NULL OR HEALTHY = 'N';
SELECT *
FROM PATIENT
WHERE COALESCE(HEALTHY, 'N') = 'N';
```

## NULL in Gruppenfunktionen

```sql
-- Wie viele Werte ungleich NULL sind in Healthy?
SELECT COUNT(*) AS RowCount,
    COUNT(HEALTHY) AS Healthy,
    COUNT(*) - COUNT(HEALTHY) AS HealthyNull
FROM PATIENT;

-- Bei Aggregatsfunktionen (AVG, SUM, ...) wird NULL ignoriert. Bei der Mittelwertbildung führt dies
-- aber zu nicht erwarteten Werten, wenn der Mittelwert aus nur wenigen Datensätzen berechnet wurde.
SELECT
    AVG(DISEASES) AS AvgDiseases,
    COUNT(DISEASES) AS CountDiseases
FROM PATIENT;
```

## NULL in Berechnungen

```sql
-- Bei arithmetischen Operationen ist der Ergebnis NULL, wenn ein Operand NULL ist.
SELECT PATIENT_ID, DISEASES, OPERATIONS,
    DISEASES + 1,
    DISEASES - OPERATIONS,
    0 * DISEASES
FROM PATIENT;
```

## NULL in JOIN Operationen

```sql
-- NULL im JOIN: Alle NULL Werte in DISEASES fallen weg:
SELECT *
FROM PATIENT INNER JOIN DISEASES_CLASS ON (DISEASES = NUMBEROF);

-- Möchten wir für NULL den Wert NOT APPLICABLE drucken, arbeiten wir mit COALESCE und einem nicht verwendeten
-- Standardwert.
SELECT *
FROM PATIENT INNER JOIN DISEASES_CLASS ON (COALESCE(DISEASES,-1) = COALESCE(NUMBEROF,-1))

-- Häufiger verwendet wird jedoch ein LEFT JOIN mit einem Standardwert, wenn der Fremdschlüssel NULL ist.
-- Hier braucht es keinen Datensatz mit NULL in DISEASES_CLASS.
SELECT
    PATIENT_ID,
    NAME,
    DISEASES,
    CONDITION,
    COALESCE(TO_CHAR(DISEASES), 'NOT APPLICABLE') AS CONDITION2
FROM PATIENT LEFT JOIN DISEASES_CLASS ON (DISEASES = NUMBEROF)
```

## NULL in Listen mit IN

```sql
-- Liefert nur 1 Datensatz mit DISEASES = 1
SELECT * FROM PATIENT WHERE DISEASES IN (SELECT 1 FROM DUAL UNION SELECT NULL FROM DUAL);
-- Liefert keinen Datensatz mit DISEASES = 1
SELECT * FROM PATIENT WHERE DISEASES NOT IN (SELECT 1 FROM DUAL UNION SELECT NULL FROM DUAL);
```

## NULL in UPDATE Statements

```sql
-- Nach den UPDATE bleibt der Wert NULL so wie er ist.
UPDATE PATIENT SET DISEASES = DISEASES + 1;
SELECT * FROM PATIENT;

-- Nun sind alle Werte von DISEASES NULL
UPDATE PATIENT SET DISEASES = DISEASES + NULL;
SELECT * FROM PATIENT;
```

## NULL in Strings

```sql
-- Es wird HelloWorld geliefert:
SELECT 'Hello' || 'World'
FROM DUAL;
-- SQL Server liefert NULL (ANSI Konformes Verhalten):
SELECT 'Hello' + NULL
-- Oracle liefert Hello
SELECT 'Hello' || null
FROM DUAL;
```
