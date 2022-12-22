#!/bin/bash
# Date: November 2022
# Author: Michael Schletz
# Description: Installiert die .NET SDK in /tmp/dotnetx.xx.xx und lädt ein tar Archiv
# von der URL, die als Parameter definiert wurde, in /tmp/dotnetapp. Danach wird
# der Befehl mit dotnet run ausgeführt. Bei einem oracle Container wird der Parameter oracle
# übergeben, sonst sqlserver

if [ -d "/opt/oracle" ]; then DOWNLOADER="curl"; else DOWNLOADER="wget -O /dev/stdout"; fi

VERSION=$($DOWNLOADER https://dotnetcli.azureedge.net/dotnet/Sdk/6.0/latest.version)
ARCH=$(uname -m)
if [ $ARCH = "aarch64" ] || [ $ARCH = "arm64" ]; then
    INSTALLFILE=dotnet-sdk-$VERSION-linux-arm64.tar.gz
else
    INSTALLFILE=dotnet-sdk-$VERSION-linux-x64.tar.gz
fi
DOTNET_HOME=/tmp/dotnet$VERSION
APP_DIR=/tmp/dotnetapp
HOME=/tmp

if [ ! -d "$DOTNET_HOME" ]; then
    echo Lade .NET $VERSION. Bitte warten und Pfoten weg von der Konsole...
    $DOWNLOADER https://dotnetcli.azureedge.net/dotnet/Sdk/$VERSION/$INSTALLFILE > /tmp/$INSTALLFILE
    echo Entpacke .NET $VERSION. Bitte warten und Pfoten weg von der Konsole...
    mkdir -p $DOTNET_HOME && tar zxf /tmp/$INSTALLFILE -C $DOTNET_HOME
fi

echo Lade $1 und führe das Projekt aus...
$DOWNLOADER $1 > /tmp/app.tar
rm -rf $APP_DIR && mkdir -p $APP_DIR && tar xf /tmp/app.tar -C $APP_DIR

cd $APP_DIR
echo Kompiliere und starte das Programm. Bitte warten und Pfoten weg von der Konsole...
if [ -d "/opt/oracle" ]; then 
    export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1 && $DOTNET_HOME/dotnet run -- oracle
else 
    export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0 && $DOTNET_HOME/dotnet run -- sqlserver
fi
