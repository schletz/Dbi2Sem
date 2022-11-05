cd $HOME
DOTNET_SDK=dotnet-sdk-6.0.402-linux-x64.tar.gz
URL=https://download.visualstudio.microsoft.com/download/pr/d3e46476-4494-41b7-a628-c517794c5a6a/6066215f6c0a18b070e8e6e8b715de0b/$DOTNET_SDK

if [ ! -f "$DOTNET_SDK" ]; then
    curl $URL > $DOTNET_SDK
    mkdir -p $HOME/dotnet && tar zxf $DOTNET_SDK -C $HOME/dotnet
    export DOTNET_ROOT=$HOME/dotnet
    export PATH=$PATH:$HOME/dotnet
    export DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
fi
