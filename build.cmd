
@echo off

color 17
Title Ω≈±æ±‡“Î¥¶¿Ì

REM msbuild config

set config=Release

set outputdir=bin

set commonflags=/t:Rebuild /p:Configuration=%config%;AllowUnsafeBlocks=true /p:CLSCompliant=False



if %PROCESSOR_ARCHITECTURE%==x86 (

         set msbuild="%WINDIR%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"

) else ( set msbuild="%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe"

)


%msbuild% "%~dp0screenshot.csproj" %commonflags% /tv:4.0 /p:TargetFrameworkVersion=v4.5 /p:Platform="Any Cpu" /p:OutputPath="%~dp0%outputdir%"

if errorlevel 1 goto build-error

:build-error

echo Failed to compile...
exit /b 1
