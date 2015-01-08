@echo off

cd projects
msbuild Epicycle.Math.net35.sln /t:Clean,Build /p:Configuration=Debug
msbuild Epicycle.Math.net35.sln /t:Clean,Build /p:Configuration=Release
msbuild Epicycle.Math.net40.sln /t:Clean,Build /p:Configuration=Debug
msbuild Epicycle.Math.net40.sln /t:Clean,Build /p:Configuration=Release
msbuild Epicycle.Math.net45.sln /t:Clean,Build /p:Configuration=Debug
msbuild Epicycle.Math.net45.sln /t:Clean,Build /p:Configuration=Release

pause
