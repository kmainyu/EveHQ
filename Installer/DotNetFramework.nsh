Section "MS .NET Framework v3.5"
	
SectionIn RO
	ReadRegStr $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5" "Version"
	strCmp $0 "" Net35NotFound
	goto Exit
	
	Net35NotFound:
	MessageBox MB_ICONEXCLAMATION|MB_YESNO|MB_DEFBUTTON2 "Version 3.5 of the Microsoft .NET Framework is required for EveHQ to run, and it does not appear to be installed on your system. In order to continue, it must be installed, \
																			 and this can be done automatically if you choose. To install the Microsoft .NET Framework v3.5, you must be currently connected to the internet. Do you wish to install the Microsoft .NET Framework and continue \
																			 with the installation?" /SD IDNO IDYES DownloadDotNet IDNO CancelInstall
	
	CancelInstall:
	Abort "Missing required software to continue. Please manually install the Microsoft .NET Framework v3.5 and start the installation again."
	
	DownloadDotNet:
	NSISDL::download /TIMEOUT=120000 "http://download.microsoft.com/download/2/0/e/20e90413-712f-438c-988e-fdaa79a8ac3d/dotnetfx35.exe" "$PLUGINSDIR\dotnetfx35full.exe"
	Pop $0
	StrCmp $0 "success" InstallDotNet
	Abort "Download of Microsoft .NET Framework 3.5 Failed. $\n The error was: $0"
	
	InstallDotNet:
	Banner::show /NOUNLOAD "Installing .NET FX. Please Wait"
	NSEXEC::ExecToStack '"$PLUGINSDIR\dotnetfx35full.exe" /q /norestart"'
	Pop $0
	SetRebootFlag true
	Banner::destroy
	
	
	StrCmp $0 "" Exit
	StrCmp $0 "0" Exit
	Abort "Enexpected Error: $0"
	
	Exit:
SectionEnd



