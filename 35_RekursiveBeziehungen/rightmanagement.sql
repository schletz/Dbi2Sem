-- CREATE DATABASE nur für SQL Server
-- USE master
-- DROP DATABASE Groupmembers
-- CREATE DATABASE Groupmembers
-- USE Groupmembers
-- GO

-- *************************************************************************************************
-- TEIL 1: Gruppen als eigene Tabellen
-- *************************************************************************************************

CREATE TABLE Abteilung (
	AbteilungId INTEGER PRIMARY KEY,
	Name        VARCHAR(200) NOT NULL,
	CanEdit     INTEGER
);

CREATE TABLE Klasse (
	KlasseId    INTEGER PRIMARY KEY,
	Abteilung   INTEGER NOT NULL REFERENCES Abteilung(AbteilungId),
	Name        VARCHAR(200) NOT NULL,
	CanEdit     INTEGER
);

CREATE TABLE Schueler (
	SchuelerId  INTEGER PRIMARY KEY,
	Firstname   VARCHAR(200) NOT NULL,
	Lastname    VARCHAR(200) NOT NULL,
	Klasse      INTEGER NOT NULL REFERENCES Klasse(KlasseId),
	CanEdit     INTEGER
);


-- Abteilungsgruppen anlegen
INSERT INTO Abteilung VALUES (1100, 'Informatik Abend', NULL);
INSERT INTO Abteilung VALUES (2100, 'Wirtschaftsingenieur', 0);

-- Klassengruppen anlegen
INSERT INTO Klasse VALUES (1110, 1100, '2AKIF', NULL);
INSERT INTO Klasse VALUES (1120, 1100, '2BKIF', 1);
INSERT INTO Klasse VALUES (2110, 2100, '1AHWIT', NULL);
INSERT INTO Klasse VALUES (2120, 2100, '2AHWIT', 1);

-- Schüler anlegen
-- 2AKIF
insert into Schueler values (1000, 'Selig', 'Grabiec', 1110, NULL);
insert into Schueler values (1001, 'Bee', 'Apted', 1110, 1);
-- 2BKIF
insert into Schueler values (1002, 'Hiram', 'Ramsted', 1120, NULL);
insert into Schueler values (1003, 'Jeth', 'Lewknor', 1120, NULL);
-- 1AHWIT
insert into Schueler values (1004, 'Garrick', 'Trusdale', 2110, NULL);
insert into Schueler values (1005, 'Velvet', 'Adshed', 2110, NULL);
-- 2AHWIT
insert into Schueler values (1006, 'Kennan', 'Imloch', 2120, NULL);
insert into Schueler values (1007, 'Mitchell', 'Rearden', 2120, 0);


-- Welche effektiven Rechte haben die Schüler?
SELECT a.Name AS Abt, k.Name AS Klasse, s.Firstname, s.Lastname,
       a.CanEdit AS CanEditAbt,
       k.CanEdit AS CanEditKlasse,
       s.CanEdit AS CanEditSchueler,
       COALESCE(s.CanEdit, k.CanEdit, a.CanEdit, 0) AS EffectiveEdit
FROM Abteilung a INNER JOIN Klasse k ON (a.AbteilungId = k.Abteilung)
                 INNER JOIN Schueler s ON (k.KlasseId = s.Klasse)
ORDER BY a.Name, k.Name, s.Lastname;


-- *************************************************************************************************
-- TEIL 2: Lösung mit einer allgemeinen Gruppentabelle
-- *************************************************************************************************
CREATE TABLE AppGroup (
	GroupId     INTEGER PRIMARY KEY,
	ParentGroup INTEGER REFERENCES AppGroup(GroupId),
	Name        VARCHAR(200) NOT NULL,
	CanEdit     INTEGER
);

CREATE TABLE Person (
	PersonId  INTEGER PRIMARY KEY,
	Firstname VARCHAR(200) NOT NULL,
	Lastname  VARCHAR(200) NOT NULL,
	AppGroup  INTEGER NOT NULL REFERENCES AppGroup(GroupId),
	CanEdit   INTEGER
);

-- Die Gruppen und ihre Hierarchien aufbauen.
INSERT INTO AppGroup VALUES (1000, NULL, 'Studierende', NULL);
    INSERT INTO AppGroup VALUES (1100, 1000, 'Informatik Abend', NULL);
        INSERT INTO AppGroup VALUES (1110, 1100, '2AKIF', NULL);
        INSERT INTO AppGroup VALUES (1120, 1100, '2BKIF', 1);
    INSERT INTO AppGroup VALUES (2100, 1000, 'Wirtschaftsingenieur', 0);
        INSERT INTO AppGroup VALUES (2110, 2100, '1AHWIT', NULL);
        INSERT INTO AppGroup VALUES (2120, 2100, '2AHWIT', 1);

INSERT INTO AppGroup VALUES (2000, NULL, 'Lehrende', 1);
INSERT INTO AppGroup VALUES (2200, 2000, 'Karenziert', 0);

-- Schüler eintragen.
-- 2AKIF
insert into Person values (1000, 'Selig', 'Grabiec', 1110, NULL);
insert into Person values (1001, 'Bee', 'Apted', 1110, 1);
-- 2BKIF
insert into Person values (1002, 'Hiram', 'Ramsted', 1120, NULL);
insert into Person values (1003, 'Jeth', 'Lewknor', 1120, NULL);
-- 1AHWIT
insert into Person values (1004, 'Garrick', 'Trusdale', 2110, NULL);
insert into Person values (1005, 'Velvet', 'Adshed', 2110, NULL);
-- 2AHWIT
insert into Person values (1006, 'Kennan', 'Imloch', 2120, NULL);
insert into Person values (1007, 'Mitchell', 'Rearden', 2120, 0);


-- Lehrer eintragen.
insert into Person values (2000, 'Kristofer', 'Kitchingman', 2000, NULL);
insert into Person values (2001, 'Filberto', 'Brettor', 2000, NULL);
-- Karenzierte Lehrer
insert into Person values (2002, 'Tamma', 'McTeague', 2200, NULL);


-- Effektive Rechte, max. 3 Ebenen
SELECT p.Lastname, p.Firstname, 
    p.CanEdit AS EditFromPerson,
	g1.Name AS Group1, g1.CanEdit AS EditFromGroup1,
	g2.Name AS Group2, g2.CanEdit AS EditFromGroup2, 
	g3.Name AS Group3, g3.CanEdit AS EditFromGroup3,
	COALESCE(p.CanEdit, g1.CanEdit, g2.CanEdit, g3.CanEdit, 0) AS EffectiveEdit
FROM Person p LEFT JOIN AppGroup g1 ON (p.AppGroup = g1.GroupId)
 LEFT JOIN AppGroup g2 ON (g1.ParentGroup = g2.GroupId)
 LEFT JOIN AppGroup g3 ON (g2.ParentGroup = g3.GroupId);

 -- Zusatzinfo: Rekursives SQL
WITH RightsCTE AS
(
SELECT p.PersonId, p.Lastname, p.Firstname,
       g.GroupId, g.Name, g.ParentGroup,
       COALESCE(p.CanEdit, g.CanEdit) AS CanEdit
FROM Person p INNER JOIN AppGroup g ON (p.AppGroup = g.GroupId)
UNION ALL
    SELECT 
        r.PersonId, r.Lastname, r.Firstname,
        g1.GroupId, g1.Name, g1.ParentGroup, -- Hier werden Daten der parent group ausgegeben.
        COALESCE(r.CanEdit, g1.CanEdit)      -- Effekive Rechte berechnen, wenn wir die Hierarchie hinauf gehen.
    -- Join zwischen der eigenen CTE Tabelle und der neuen Gruppe
    FROM RightsCTE r INNER JOIN AppGroup g1 ON (r.ParentGroup = g1.GroupId)
)
SELECT * FROM RightsCTE WHERE ParentGroup IS NULL;
