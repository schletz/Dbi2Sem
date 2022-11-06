# Date: November 2022
# Author: Michael Schletz
# Description: Installiert die .NET SDK in /tmp/dotnetx.xx.xx und lädt ein tar Archiv
# von der übergebenen URL in /tmp/dotnetapp/(timestamp) und führt danach den Befehl mit
# dotnet run aus. Bei einem oracle Container wird der Parameter oracle übergeben, sonst sqlserver
if [ -d "/opt/oracle" ]; then DOWNLOADER="curl -s"; else DOWNLOADER="wget -q -O /dev/stdout"; fi

VERSION=$($DOWNLOADER https://dotnetcli.azureedge.net/dotnet/Sdk/6.0/latest.version)
INSTALLFILE=dotnet-sdk-$VERSION-linux-x64.tar.gz
DOTNET_HOME=/tmp/dotnet$VERSION
APP_DIR=/tmp/dotnetapp/$(date +'%s')

if [ ! -d "$DOTNET_HOME" ]; then
    echo Lade .NET $VERSION
    $DOWNLOADER https://dotnetcli.azureedge.net/dotnet/Sdk/$VERSION/$INSTALLFILE > /tmp/$INSTALLFILE
    echo Entpacke .NET $VERSION
    mkdir -p $DOTNET_HOME && tar zxf /tmp/$INSTALLFILE -C $DOTNET_HOME
fi

$DOWNLOADER $1 > /tmp/app.tar
mkdir -p $APP_DIR && tar xf /tmp/app.tar -C $APP_DIR

cd $APP_DIR
if [ -d "/opt/oracle" ]; then 
    export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1 && $DOTNET_HOME/dotnet run -- oracle
else 
    HOME_OLD=$HOME
    HOME=/tmp
    export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0 && $DOTNET_HOME/dotnet run -- sqlserver
    HOME=$HOME_OLD
fi
cd $HOME
