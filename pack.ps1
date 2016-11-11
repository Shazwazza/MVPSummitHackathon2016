Remove-Item "C:\Users\Shannon\.nuget\packages\.tools"
Remove-Item "C:\Users\Shannon\.nuget\packages\dotnet-ref"

dms restore
dms build

cd dotnet-ref
dms restore
dms build
dms pack -o ../nupkgs

cd ..
cd sample/ConsumingProject
dms restore
