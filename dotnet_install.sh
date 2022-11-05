cd $HOME
VERSION=$(curl -s https://dotnetcli.azureedge.net/dotnet/Sdk/6.0/latest.version)
INSTALLFILE=dotnet-sdk-$VERSION-linux-x64.tar.gz

if [ ! -d "$HOME/dotnet" ]; then
    echo Installiere .NET $VERSION
    curl -s https://dotnetcli.azureedge.net/dotnet/Sdk/$VERSION/$INSTALLFILE > $INSTALLFILE
    mkdir -p $HOME/dotnet && tar zxf $INSTALLFILE -C $HOME/dotnet
    rm $INSTALLFILE
fi
