# 1. Clean
if(Test-Path -LiteralPath "./publish" ){
    Remove-Item -Recurse -Force ./publish
}
if(Test-Path -LiteralPath "./HelloID.JsonEditor.Installer/bin" ){
    Remove-Item -Recurse -Force ./HelloID.JsonEditor.Installer/bin
}
if(Test-Path -LiteralPath "./HelloID.JsonEditor.Installer/obj" ){
    Remove-Item -Recurse -Force ./HelloID.JsonEditor.Installer/obj
}

# 2. Publish (Create the files)
dotnet publish HelloID.JsonEditor.UI/HelloID.JsonEditor.UI.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ./publish

# 3. Build MSI
dotnet build HelloID.JsonEditor.Installer/HelloID.JsonEditor.Installer.wixproj -c Release