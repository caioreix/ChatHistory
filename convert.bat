@echo off
REM Drag the chat.history file onto this .bat or pass it as an argument

IF "%~1"=="" (
    echo Usage: drag the chat.history file onto this script or pass it as an argument.
    pause
    exit /b
)

set "input=%~1"
set "output=%input%.converted"
> "%output%" REM Clears the output file

for /f "usebackq delims=" %%L in ("%input%") do (
    REM Decodes base64 and performs URL-escape with PowerShell
    for /f "delims=" %%D in ('powershell -NoProfile -Command "[System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String('%%L'))"') do (
        for /f "delims=" %%U in ('powershell -NoProfile -Command "[System.Uri]::EscapeDataString('%%D')"') do (
            echo %%U>>"%output%"
        )
    )
)

echo Conversion completed: %output%
pause
