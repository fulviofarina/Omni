REM echo cd %SYSTEMROOT%\SysWow64\ > "%temp%\batch1.bat"
SET ba1=%temp%\batch1.bat
SET ba2=%temp%\batch2.bat
SET vb=%temp%\vb.vbs
echo call %SYSTEMROOT%\system32\dism /online /enable-feature /featurename:"MSMQ-Container" > %ba1%
echo call %SYSTEMROOT%\SYSNATIVE\dism /online /enable-feature /featurename:"MSMQ-Container" >> %ba1%
echo call %SYSTEMROOT%\system32\dism /online /enable-feature /featurename:"MSMQ-Server" > %ba2%
echo call %SYSTEMROOT%\SYSNATIVE\dism /online /enable-feature /featurename:"MSMQ-Server" >> %ba2%
REM echo %SendKeys% "n" >>  "%temp%\batch1.bat"
REM echo %SendKeys% "n" >>  "%temp%\batch2.bat"
REM echo exit > "%temp%\batch1.bat"
echo Set UAC = CreateObject^("Shell.Application"^) > %vb%
set params = %*:"="
echo UAC.ShellExecute "cmd.exe", "/c %temp%\batch1.bat", "", "runas", 1 >> %vb%
echo UAC.ShellExecute "cmd.exe", "/c %temp%\batch2.bat", "", "runas", 1 >> %vb%
REM echo Set oShell = CreateObject^("WScript.Shell")  >> "%temp%\vb1.vbs"
REM echo %EnterKeys% "N" >> "%temp%\getadmin1.bat"
REM echo end > "%temp%\batch2.bat"
REM echo Set UAC = CreateObject^("Shell.Application"^) > "%temp%\vb2.vbs"
REM set params = %*:"="
REM echo UAC.ShellExecute "cmd.exe", "/c %temp%\batch2.bat", "", "runas", 1 >> "%temp%\vb2.vbs"
REM echo %EnterKeys% "N" >> "%temp%\getadmin2.bat"
REM "%temp%\getadmin1.vbs" 
REM "%temp%\getadmin2.vbs" 

