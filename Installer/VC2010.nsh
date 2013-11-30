Section "SQL Compact Edition 4.0"

SectionIn RO
	ReadRegStr $0 HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{F0C3E5D1-1ADE-321E-8167-68EF0DE699A5}" "DisplayName"
		strCmp $0 "" VCRNotFound
		goto SkipVCR
		
	VCRNotFound:
	MessageBox MB_ICONEXCLAMATION|MB_YESNO|MB_DEFBUTTON2 "EveHQ requires the Visual C++ 2010 Runtime, but the installer couldn't find it on your system. $\n \
																												If you are sure you have installed it previously, you can skip this download however if it is missing EveHQ will not work $\n \
																												Do you wish to download and install the Microsoft Visual C++ 2010 Runtime?" /SD IDNO IDYES DownloadVCR IDNO SkipVCR
											
  DownloadVCR:																												
	#Are we 64bit or 32bit Windows 
	#${If} ${RunningX64}
	#	NSISDL::download /TIMEOUT=120000 "http://download.microsoft.com/download/0/5/D/05DCCDB5-57E0-4314-A016-874F228A8FAD/SSCERuntime_x64-ENU.exe" "$PLUGINSDIR\SQLCE.exe"
	#${Else}
	#		NSISDL::download /TIMEOUT=120000 "http://download.microsoft.com/download/0/5/D/05DCCDB5-57E0-4314-A016-874F228A8FAD/SSCERuntime_x86-ENU.exe" "$PLUGINSDIR\SQLCE.exe"
	#${EndIf}
	NSISDL::download /TIMEOUT=120000 "http://download.microsoft.com/download/C/6/D/C6D0FD4E-9E53-4897-9B91-836EBA2AACD3/vcredist_x86.exe" "$PLUGINSDIR\vcredist_x86.exe"
	Pop $0
	StrCmp $0 "success" InstallVCR
	Abort "Download of Microsoft Visual C++ 2010 Runtime Failed. $\n The error was: $0"
	
	InstallVCR:
	Banner::show /NOUNLOAD "Installing Microsoft Visual C++ 2010 Runtime. Please Wait"
	NSEXEC::ExecToStack '"$PLUGINSDIR\vcredist_x86.exe" /q /norestart"'
	Pop $0
	SetRebootFlag true
	Banner::destroy
	
	
	StrCmp $0 "" Exit
	StrCmp $0 "0" Exit
	Abort "Enexpected Error: $0"
	
	
	Exit:
	SkipVCR:
SectionEnd