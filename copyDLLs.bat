cd "C:\Users\aleks\Projects\TS Games\Planes262\GameDataStructures"
rd /s /q bin
"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\msbuild.exe"  GameDataStructures.csproj /p:Configuration=Release /l:FileLogger,Microsoft.Build.Engine;logfile=Manual_MSBuild_ReleaseVersion_LOG.log
xcopy /y "C:\Users\aleks\Projects\TS Games\Planes262\GameDataStructures\bin\Release\net471\GameDataStructures.dll" "C:\Users\aleks\Projects\TS Games\Planes262\Game\Assets"
xcopy /y "C:\Users\aleks\Projects\TS Games\Planes262\GameDataStructures\bin\Release\net471\GameDataStructures.dll" "C:\Users\aleks\Projects\TS Games\Level Editor\Assets"

cd ..\GameJudge
rd /s /q bin
"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\msbuild.exe"  GameJudge.csproj /p:Configuration=Release /l:FileLogger,Microsoft.Build.Engine;logfile=Manual_MSBuild_ReleaseVersion_LOG.log
xcopy /y "C:\Users\aleks\Projects\TS Games\Planes262\GameJudge\bin\Release\net471\GameJudge.dll" "C:\Users\aleks\Projects\TS Games\Planes262\Game\Assets"



