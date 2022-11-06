# Date: November 2022
# Author: Michael Schletz
# Description: Installiert die .NET SDK in /tmp/dotnetx.xx.xx und lädt ein tar Archiv
# von der URL, die in $APP_URL definiert wurde, in /tmp/dotnetapp. Danach wird
# der Befehl mit dotnet run ausgeführt. Bei einem oracle Container wird der Parameter oracle
# übergeben, sonst sqlserver

if [ -d "/opt/oracle" ]; then DOWNLOADER="curl -s"; else DOWNLOADER="wget -q -O /dev/stdout"; fi

VERSION=$($DOWNLOADER https://dotnetcli.azureedge.net/dotnet/Sdk/6.0/latest.version)
INSTALLFILE=dotnet-sdk-$VERSION-linux-x64.tar.gz
DOTNET_HOME=/tmp/dotnet$VERSION
APP_DIR=/tmp/dotnetapp
HOME_OLD=$HOME
HOME=/tmp

if [ ! -d "$DOTNET_HOME" ]; then
    echo Lade .NET $VERSION...
    $DOWNLOADER https://dotnetcli.azureedge.net/dotnet/Sdk/$VERSION/$INSTALLFILE > /tmp/$INSTALLFILE
    echo Entpacke .NET $VERSION...
    mkdir -p $DOTNET_HOME && tar zxf /tmp/$INSTALLFILE -C $DOTNET_HOME
fi

echo Lade $APP_URL und führe das Projekt aus...
$DOWNLOADER $APP_URL > /tmp/app.tar
rm -rf APP_DIR && mkdir -p $APP_DIR && tar xf /tmp/app.tar -C $APP_DIR

cd $APP_DIR
if [ -d "/opt/oracle" ]; then 
    export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1 && $DOTNET_HOME/dotnet run -- oracle
else 
    export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0 && $DOTNET_HOME/dotnet run -- sqlserver
fi
HOME=$HOME_OLD
