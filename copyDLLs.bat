cd "C:\Users\aleks\Projects\TS Games\Planes262\GameDataStructures"
rd /s /q obj
rd /s /q bin
"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\devenv.exe" GameDataStructures.csproj /Build "Debug"
xcopy /y c:\source d:\target

cd ..\GameJudge
rd /s /q obj
rd /s /q bin
"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\devenv.exe" GameJudge.csproj /Build "Debug"