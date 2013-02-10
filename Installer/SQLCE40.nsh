Section "SQL Compact Edition 4.0"

SectionIn RO
	ReadRegStr $0 HKLM "SOFTWARE\Microsoft\Microsoft SQL Server Compact Edition\v4.0" "Version"
		strCmp $0 "" SQLCENotFound
		goto SkipCE
		
	SQLCENotFound:
	MessageBox MB_ICONEXCLAMATION|MB_YESNO|MB_DEFBUTTON2 "SQL Server Compact Edition v4.0 Libraries are required in order for EveHQ to function. $\n \
																												If you entend to use EveHQ against a full SQL server data, you can skip this installation. $\n \
																												Do you wish to install SQL Server Compact Edition v4.0?" /SD IDNO IDYES DownloadSQLCE IDNO SkipCE
											
  DownloadSQLCE:																												
	#Are we 64bit or 32bit Windows 
	${If} ${RunningX64}
		NSISDL::download /TIMEOUT=120000 "http://download.microsoft.com/download/0/5/D/05DCCDB5-57E0-4314-A016-874F228A8FAD/SSCERuntime_x64-ENU.exe" "$PLUGINSDIR\SQLCE.exe"
	${Else}
			NSISDL::download /TIMEOUT=120000 "http://download.microsoft.com/download/0/5/D/05DCCDB5-57E0-4314-A016-874F228A8FAD/SSCERuntime_x86-ENU.exe" "$PLUGINSDIR\SQLCE.exe"
	${EndIf}
	Pop $0
	StrCmp $0 "success" InstallSQLCE
	Abort "Download of SQLCE Failed. $\n The error was: $0"
	
	InstallSQLCE:
	Banner::show /NOUNLOAD "Installing SQLCE. Please Wait"
	NSEXEC::ExecToStack '"$PLUGINSDIR\SQLCE.exe" /i /quiet /passive /norestart"'
	Pop $0
	SetRebootFlag true
	Banner::destroy
	
	
	StrCmp $0 "" Exit
	StrCmp $0 "0" Exit
	Abort "Enexpected Error: $0"
	
	
	Exit:
	SkipCE:
SectionEnd