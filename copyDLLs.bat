cls
cd "C:\Users\Aleksander\Projects\Planes262\GameDataStructures"
rd /s /q bin
"C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe"  GameDataStructures.csproj /p:Configuration=Release
xcopy /y "C:\Users\Aleksander\Projects\Planes262\GameDataStructures\bin\Release\net471\GameDataStructures.dll" "C:\Users\Aleksander\Projects\Planes262\Game\Assets"

cd ..\GameJudge
rd /s /q bin
"C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\msbuild.exe"  GameJudge.csproj /p:Configuration=Release
xcopy /y "C:\Users\Aleksander\Projects\Planes262\GameJudge\bin\Release\net471\GameJudge.dll" "C:\Users\Aleksander\Projects\Planes262\Game\Assets"

cd ..

