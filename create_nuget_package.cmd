@echo off

rmdir NuGetPackage /s /q
mkdir NuGetPackage
mkdir NuGetPackage\Epicycle.Math-cs.0.1.7.0
mkdir NuGetPackage\Epicycle.Math-cs.0.1.7.0\lib

copy package.nuspec NuGetPackage\Epicycle.Math-cs.0.1.7.0\Epicycle.Math-cs.0.1.7.0.nuspec
copy README.md NuGetPackage\Epicycle.Math-cs.0.1.7.0\README.md
copy LICENSE NuGetPackage\Epicycle.Math-cs.0.1.7.0\LICENSE

xcopy externals\lib_dotnet\Clipper.6.2.1\lib\clipper_library.dll NuGetPackage\Epicycle.Math-cs.0.1.7.0\lib\net35\
xcopy bin\net35\Release\Epicycle.Math.TestUtils_cs.dll NuGetPackage\Epicycle.Math-cs.0.1.7.0\lib\net35\
xcopy bin\net35\Release\Epicycle.Math.TestUtils_cs.pdb NuGetPackage\Epicycle.Math-cs.0.1.7.0\lib\net35\
xcopy bin\net35\Release\Epicycle.Math.TestUtils_cs.xml NuGetPackage\Epicycle.Math-cs.0.1.7.0\lib\net35\
xcopy bin\net35\Release\Epicycle.Math_cs.dll NuGetPackage\Epicycle.Math-cs.0.1.7.0\lib\net35\
xcopy bin\net35\Release\Epicycle.Math_cs.pdb NuGetPackage\Epicycle.Math-cs.0.1.7.0\lib\net35\
xcopy bin\net35\Release\Epicycle.Math_cs.xml NuGetPackage\Epicycle.Math-cs.0.1.7.0\lib\net35\
xcopy externals\lib_dotnet\Clipper.6.2.1\lib\clipper_library.dll NuGetPackage\Epicycle.Math-cs.0.1.7.0\lib\net40\
xcopy bin\net40\Release\Epicycle.Math.TestUtils_cs.dll NuGetPackage\Epicycle.Math-cs.0.1.7.0\lib\net40\
xcopy bin\net40\Release\Epicycle.Math.TestUtils_cs.pdb NuGetPackage\Epicycle.Math-cs.0.1.7.0\lib\net40\
xcopy bin\net40\Release\Epicycle.Math.TestUtils_cs.xml NuGetPackage\Epicycle.Math-cs.0.1.7.0\lib\net40\
xcopy bin\net40\Release\Epicycle.Math_cs.dll NuGetPackage\Epicycle.Math-cs.0.1.7.0\lib\net40\
xcopy bin\net40\Release\Epicycle.Math_cs.pdb NuGetPackage\Epicycle.Math-cs.0.1.7.0\lib\net40\
xcopy bin\net40\Release\Epicycle.Math_cs.xml NuGetPackage\Epicycle.Math-cs.0.1.7.0\lib\net40\
xcopy externals\lib_dotnet\Clipper.6.2.1\lib\clipper_library.dll NuGetPackage\Epicycle.Math-cs.0.1.7.0\lib\net45\
xcopy bin\net45\Release\Epicycle.Math.TestUtils_cs.dll NuGetPackage\Epicycle.Math-cs.0.1.7.0\lib\net45\
xcopy bin\net45\Release\Epicycle.Math.TestUtils_cs.pdb NuGetPackage\Epicycle.Math-cs.0.1.7.0\lib\net45\
xcopy bin\net45\Release\Epicycle.Math.TestUtils_cs.xml NuGetPackage\Epicycle.Math-cs.0.1.7.0\lib\net45\
xcopy bin\net45\Release\Epicycle.Math_cs.dll NuGetPackage\Epicycle.Math-cs.0.1.7.0\lib\net45\
xcopy bin\net45\Release\Epicycle.Math_cs.pdb NuGetPackage\Epicycle.Math-cs.0.1.7.0\lib\net45\
xcopy bin\net45\Release\Epicycle.Math_cs.xml NuGetPackage\Epicycle.Math-cs.0.1.7.0\lib\net45\

cd NuGetPackage
nuget pack Epicycle.Math-cs.0.1.7.0\Epicycle.Math-cs.0.1.7.0.nuspec -Properties version=0.1.7.0
7z a -tzip Epicycle.Math-cs.0.1.7.0.zip Epicycle.Math-cs.0.1.7.0 Epicycle.Math-cs.0.1.7.0.nupkg

echo nuget push Epicycle.Math-cs.0.1.7.0.nupkg > push.cmd
echo pause >> push.cmd

pause