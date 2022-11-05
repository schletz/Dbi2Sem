cd $HOME
VERSION=$(curl -s https://dotnetcli.azureedge.net/dotnet/Sdk/6.0/latest.version)
INSTALLFILE=dotnet-sdk-$VERSION-linux-x64.tar.gz

if [ ! -f "$INSTALLFILE" ]; then
    curl -s https://dotnetcli.azureedge.net/dotnet/Sdk/$VERSION/$INSTALLFILE > $INSTALLFILE
fi
echo Installiere .NET $VERSION
mkdir -p $HOME/dotnet && tar zxf $INSTALLFILE -C $HOME/dotnet
export DOTNET_ROOT=$HOME/dotnet
export PATH=$PATH:$HOME/dotnet
export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
