Remove-Item "C:\Users\Shannon\.nuget\packages\sampletargets.reftarget"
Remove-Item "C:\Users\Shannon\.nuget\packages\.tools"
Remove-Item "C:\Users\Shannon\.nuget\packages\dotnet-ref"
Remove-Item "C:\Users\Shannon\.nuget\packages\dotnet-packer"

dms restore
dms build

cd dotnet-packer
dms pack -o ../nupkgs

cd ../dotnet-ref
dms pack -o ../nupkgs

cd ../SampleTargets.PackerTarget
dms pack -o ../nupkgs

cd ../SampleTargets.RefTarget
dms pack -o ../nupkgs

cd ..
cd sample/ConsumingProject
dms restore
