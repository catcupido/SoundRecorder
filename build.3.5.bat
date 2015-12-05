::modify this path to use appropriate version of MSBuild:
set tool=%windir%\Microsoft.NET\Framework\v3.5\MSBuild.exe

%tool% SoundRecorder.csproj /p:Configuration=Debug
%tool% SoundRecorder.csproj /p:Configuration=Release