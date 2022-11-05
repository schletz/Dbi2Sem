cd /home
VERSION=$(curl -s https://dotnetcli.azureedge.net/dotnet/Sdk/6.0/latest.version)
INSTALLFILE=dotnet-sdk-$VERSION-linux-x64.tar.gz

if [ ! -f "$INSTALLFILE" ]; then
    curl -s https://dotnetcli.azureedge.net/dotnet/Sdk/$VERSION/$INSTALLFILE > $INSTALLFILE
fi
echo Installiere .NET $VERSION
mkdir -p /home/dotnet && tar zxf $INSTALLFILE -C /home/dotnet
export DOTNET_ROOT=/home/dotnet
export PATH=$PATH:/home/dotnet
export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
