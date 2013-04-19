Section "MS .NET Framework v4"
	
SectionIn RO
	ReadRegStr $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" "Version"
	strCmp $0 "" Net40NotFound
	goto Exit
	
	Net40NotFound:
	MessageBox MB_ICONEXCLAMATION|MB_YESNO|MB_DEFBUTTON2 "Version 4.0 of the Microsoft .NET Framework is required for EveHQ to run, and it does not appear to be installed on your system. In order to continue, it must be installed, \
																			 and this can be done automatically if you choose. To install the Microsoft .NET Framework v4.0, you must be currently connected to the internet. Do you wish to install the Microsoft .NET Framework and continue \
																			 with the installation?" /SD IDNO IDYES DownloadDotNet IDNO CancelInstall
	
	CancelInstall:
	Abort "Missing required software to continue. Please manually install the Microsoft .NET Framework v4.0 and start the installation again."
	
	DownloadDotNet:
	NSISDL::download /TIMEOUT=120000 "http://download.microsoft.com/download/9/5/A/95A9616B-7A37-4AF6-BC36-D6EA96C8DAAE/dotNetFx40_Full_x86_x64.exe" "$PLUGINSDIR\dotnetfx.exe"
	Pop $0
	StrCmp $0 "success" InstallDotNet
	Abort "Download of Microsoft .NET Framework 4.0 Failed. $\n The error was: $0"
	
	InstallDotNet:
	Banner::show /NOUNLOAD "Installing .NET Framework 4.0. Please Wait"
	NSEXEC::ExecToStack '"$PLUGINSDIR\dotnetfx.exe" /q /norestart"'
	Pop $0
	SetRebootFlag true
	Banner::destroy
	
	
	StrCmp $0 "" Exit
	StrCmp $0 "0" Exit
	Abort "Enexpected Error: $0"
	
	Exit:
SectionEnd



