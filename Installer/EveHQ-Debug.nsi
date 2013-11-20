
SetCompressor /solid lzma
;Header Info
;--------------------------------

!include DotNetFramework.nsh
!include MUI2.nsh
!include x64.nsh
!include Upgrade.nsh
!include DirClean.nsh
!include "FileFunc.nsh"



 
Name "EveHQ"
OutFile "EveHQ-Debug-Setup.exe"

RequestExecutionLevel admin

InstallDir $PROGRAMFILES\EveHQ
InstallDirRegKey HKLM "Software\EveHQ" "Install_Dir"



!define MUI_ICON "..\EveHQ\Resources\EveHQ.ico"
!define MUI_UNICON "..\EveHQ\Resources\EveHQ.ico"
!define MUI_WELCOMEFINISHPAGE_BITMAP EveHQInstallerSideImage.bmp

#Installer file settings
VIAddVersionKey "CompanyName" "Software Addicts Studios"
VIAddVersionKey "FileDescription" "EveHQ: The Internet Spaceship Toolki"
VIAddVersionKey "LegalCopyright" "Copyright 2005-2013, EveHQ Dev Team"
VIAddVersionKey "ProductName" "EveHQ Setup"
!ifdef Version
VIProductVersion ${Version}
!else
VIProductVersion 1.0.0.0
!endif



#Page Declarations
;--------------------------------
#Welcome page

!insertmacro MUI_PAGE_WELCOME

#License
!insertmacro MUI_PAGE_LICENSE ..\EveHQ\License.txt

#Install directory
!insertmacro MUI_PAGE_DIRECTORY

#StartMenu 
Var StartMenuFolder
!define MUI_STARTMENUPAGE_REGISTRY_ROOT "HKLM"
!define MUI_STARTMENUPAGE_REGISTRY_KEY "Software\EveHQ"
!define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "Start Menu Folder"
!insertmacro MUI_PAGE_STARTMENU "EveHQ" $StartMenuFolder

#Install files progress screen
!insertmacro MUI_PAGE_INSTFILES

#Finish page

#!define MUI_FINISHPAGE_RUN $INSTDIR\EveHQ.exe /local
#!define MUI_FINISHPAGE_RUN_TEXT "Run EveHQ.exe"
!define MUI_FINISHPAGE_SHOWREADME ""
!define MUI_FINISHPAGE_SHOWREADME_NOTCHECKED
!define MUI_FINISHPAGE_SHOWREADME_TEXT "Create Desktop Shortcut"
!define MUI_FINISHPAGE_SHOWREADME_FUNCTION CreateDesktopShortcut
!insertmacro MUI_PAGE_FINISH

#Uninstall pages
!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

;--------------------------------
!insertmacro MUI_LANGUAGE "English"

; The stuff to install
Section "EveHQ Files"

SectionIn RO
  
###################################################################
# Program files                                                   #
###################################################################
  SetOutPath $INSTDIR
  
  File "..\BuildOutput\Debug\DevComponents.DotNetBar2.dll"
  File "..\BuildOutput\Debug\EveCacheParser.dll"
  File "..\BuildOutput\Debug\EveHQ.Caching.dll"
  File "..\BuildOutput\Debug\EveHQ.Caching.pdb"
  File "..\BuildOutput\Debug\EveHQ.Common.dll"
  File "..\BuildOutput\Debug\EveHQ.Core.dll"
  File "..\BuildOutput\Debug\EveHQ.Core.pdb"
  File "..\BuildOutput\Debug\EveHQ.EveAPI.dll"
  File "..\BuildOutput\Debug\EveHQ.EveAPI.pdb"
  File "..\BuildOutput\Debug\EveHQ.EveData.dll"
  File "..\BuildOutput\Debug\EveHQ.EveData.pdb"
  File "..\BuildOutput\Debug\EveHQ.exe"
  File "..\BuildOutput\Debug\EveHQ.pdb"
  File "..\BuildOutput\Debug\EveHQ.exe.config"
  File "..\BuildOutput\Debug\EveHQ.HQF.dll"
  File "..\BuildOutput\Debug\EveHQ.HQF.pdb"
  File "..\BuildOutput\Debug\EveHQ.KillMailViewer.dll"
  File "..\BuildOutput\Debug\EveHQ.KillMailViewer.pdb"
  File "..\BuildOutput\Debug\EveHQ.Market.dll"
  File "..\BuildOutput\Debug\EveHQ.Market.pdb"
  File "..\BuildOutput\Debug\EveHQ.Prism.dll"
  File "..\BuildOutput\Debug\EveHQ.Prism.pdb"
  File "..\BuildOutput\Debug\EveHQ.Void.dll"
  File "..\BuildOutput\Debug\EveHQ.Void.pdb"
  File "..\BuildOutput\Debug\GammaJul.lglcd.dll"
  File "..\BuildOutput\Debug\GammaJul.lglcd.Native32.dll"
  File "..\BuildOutput\Debug\GammaJul.lglcd.Native64.dll"
  File "..\BuildOutput\Debug\Ionic.Zip.dll"
  File "..\BuildOutput\Debug\Newtonsoft.json.dll"
  File "..\BuildOutput\Debug\protobuf-net.dll"
  File "..\BuildOutput\Debug\EveHQ.SettingsConverter.exe"
  File "..\BuildOutput\Debug\EveHQ.SettingsConverter.pdb"
  File "..\BuildOutput\Debug\System.Data.SQLite.dll"
  File "..\BuildOutput\Debug\System.Net.Http.dll"
  File "..\BuildOutput\Debug\System.Net.Http.Extensions.dll"
  File "..\BuildOutput\Debug\System.Net.Http.Primitives.dll"
  File "..\BuildOutput\Debug\System.Net.Http.WebRequest.dll"
  File "..\BuildOutput\Debug\System.Runtime.dll"
  File "..\BuildOutput\Debug\System.Threading.Tasks.dll"
  File "..\EveHQ\License.txt"

###################################################################
# Data files                                                      #
###################################################################


  SetOutPath $INSTDIR\StaticData

	File "..\BuildOutput\Debug\StaticData\Agents.dat"
	File "..\BuildOutput\Debug\StaticData\AssemblyArrays.dat"
	File "..\BuildOutput\Debug\StaticData\attributes.dat"
	File "..\BuildOutput\Debug\StaticData\AttributeTypes.dat"
	File "..\BuildOutput\Debug\StaticData\Blueprints.dat"
	File "..\BuildOutput\Debug\StaticData\boosters.dat"
	File "..\BuildOutput\Debug\StaticData\CertCats.dat"
	File "..\BuildOutput\Debug\StaticData\CertRec.dat"
	File "..\BuildOutput\Debug\StaticData\Certs.dat"
	File "..\BuildOutput\Debug\StaticData\CertSkills.dat"
	File "..\BuildOutput\Debug\StaticData\Constellations.dat"
	File "..\BuildOutput\Debug\StaticData\Divisions.dat"
	File "..\BuildOutput\Debug\StaticData\EffectTypes.dat"
	File "..\BuildOutput\Debug\StaticData\GroupCats.dat"
	File "..\BuildOutput\Debug\StaticData\implants.dat"
	File "..\BuildOutput\Debug\StaticData\ItemCats.dat"
	File "..\BuildOutput\Debug\StaticData\ItemFlags.dat"
	File "..\BuildOutput\Debug\StaticData\ItemGroups.bin"
	File "..\BuildOutput\Debug\StaticData\ItemGroups.dat"
	File "..\BuildOutput\Debug\StaticData\ItemList.dat"
	File "..\BuildOutput\Debug\StaticData\ItemMarketGroups.dat"
	File "..\BuildOutput\Debug\StaticData\Items.dat"
	File "..\BuildOutput\Debug\StaticData\ItemUnlocks.dat"
	File "..\BuildOutput\Debug\StaticData\MarketGroups.dat"
	File "..\BuildOutput\Debug\StaticData\Masteries.dat"
	File "..\BuildOutput\Debug\StaticData\MetaGroups.dat"
	File "..\BuildOutput\Debug\StaticData\MetaTypes.dat"
	File "..\BuildOutput\Debug\StaticData\modules.dat"
	File "..\BuildOutput\Debug\StaticData\NPCCorps.dat"
	File "..\BuildOutput\Debug\StaticData\Regions.dat"
	File "..\BuildOutput\Debug\StaticData\ShipGroups.bin"
	File "..\BuildOutput\Debug\StaticData\ships.dat"
	File "..\BuildOutput\Debug\StaticData\skills.dat"
	File "..\BuildOutput\Debug\StaticData\SkillUnlocks.dat"
	File "..\BuildOutput\Debug\StaticData\Stations.dat"
	File "..\BuildOutput\Debug\StaticData\Systems.dat"
  File "..\BuildOutput\Debug\StaticData\TypeAttributes.dat"
  File "..\BuildOutput\Debug\StaticData\TypeEffects.dat"
  File "..\BuildOutput\Debug\StaticData\Units.dat"
  
  # delete cache files
  
${If} $useLocalFlag == "1" 
   Delete $INSTDIR\EveHQ\HQF\Cache\*.*
  Delete $INSTDIR\EveHQ\CoreCache\*.*
  Delete $INSTDIR\EveHQ\ImageCache\*.*
  !insertmacro RemoveFilesAndSubDirs "$INSTDIR\EveHQ\MarketCache\"
${Else}
   Delete $APPDATA\EveHQ\HQF\Cache\*.*
  Delete $APPDATA\EveHQ\CoreCache\*.*
  Delete $APPDATA\EveHQ\ImageCache\*.*
  !insertmacro RemoveFilesAndSubDirs "$APPDATA\EveHQ\MarketCache\"
${EndIf}
  
 SetOutPath $INSTDIR
  
  

 ; Write the installation path into the registry
  WriteRegStr HKLM SOFTWARE\EveHQ "Install_Dir" "$INSTDIR"
  
  # start menu
  !insertmacro MUI_STARTMENU_WRITE_BEGIN EveHQ
	 SetShellVarContext current
		 CreateDirectory "$SMPROGRAMS\$StartMenuFolder"
${If} $useLocalFlag == "1"
		 CreateShortCut "$SMPROGRAMS\$StartMenuFolder\EveHQ.lnk" "$INSTDIR\EveHQ.exe" "/local"
${Else}
     CreateShortCut "$SMPROGRAMS\$StartMenuFolder\EveHQ.lnk" "$INSTDIR\EveHQ.exe"
${EndIf}
	!insertmacro MUI_STARTMENU_WRITE_END
  
  ; Write the uninstall keys for Windows
 WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\EveHQ" "DisplayName" "EveHQ"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\EveHQ" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\EveHQ" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\EveHQ" "NoRepair" 1
  WriteUninstaller "uninstall.exe"

SectionEnd


#Uninstall section and logic
;--------------------------------


Section "un.Uninstall"
  
  
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\EveHQ"
  DeleteRegKey HKLM SOFTWARE\EveHQ

  ; Remove files and uninstaller
  Delete $INSTDIR\*.exe
  Delete $INSTDIR\*.dll
  Delete $INSTDIR\*.pdb
  Delete $INSTDIR\*.txt
  
${If} $useLocalFlag == "1" 
  !insertmacro RemoveFilesAndSubDirs "$INSTDIR\StaticData" 
${Else}
  !insertmacro RemoveFilesAndSubDirs "$APPDATA\EveHQ\StaticData" 
${EndIf}


  ; Remove shortcuts, if any
  !insertmacro MUI_STARTMENU_GETFOLDER EveHQ $StartMenuFolder
  Delete "$SMPROGRAMS\$StartMenuFolder\EveHQ.lnk"
  RMDir "$SMPROGRAMS\$StartMenuFolder\"
  
  #delete the desktop shortcut
  delete "$desktop\EveHQ.lnk"

  ; Remove directories used
  RMDir "$R0\EveHQ"
  RMDir "$INSTDIR"

SectionEnd
  
  
  
#Functions
#--------------------------------------------


Function .onInit

# make sure it isn't running
call EveHQNotRunning

#uninstall the msi versions.

#2.11.10
push $R0
  StrCpy $R0 {F0922F53-0905-4830-9755-93E1B0C32B20}
  Call UninstallMSI
pop $R0

#2.11.9
push $R0
  StrCpy $R0 {5EBB0328-70C2-4A58-A4AC-225419B25E51}
  Call UninstallMSI
pop $R0


#2.11.8
push $R0
  StrCpy $R0 {7E13F3C3-7B43-44E1-8D4D-6060243674D2}
  Call UninstallMSI
pop $R0

#2.11.7
push $R0
  StrCpy $R0 {674EC3E3-3B20-48A8-A997-AB9500300B3E}
  Call UninstallMSI
pop $R0


#2.11.6
push $R0
  StrCpy $R0 {0613D880-939E-4C9D-AD7C-A10DF7D7D5E9}
  Call UninstallMSI
pop $R0

#2.11.2
push $R0
  StrCpy $R0 {DED16EBB-EA17-4DD5-894A-E17EDBCD0A48}
  Call UninstallMSI
pop $R0


#2.11.1
push $R0
  StrCpy $R0 {6566D84A-B0C0-49E7-A289-C862B579899A}
  Call UninstallMSI
pop $R0

#2.11.0
push $R0
  StrCpy $R0 {95ACBF5B-C866-4A6D-BD56-63CF1C404C00}
  Call UninstallMSI
pop $R0

#2.8.0
push $R0
  StrCpy $R0 {8927BFA3-3347-4203-A8C7-2FA79B7614A0}
  Call UninstallMSI
pop $R0

#2.7.0
push $R0
  StrCpy $R0 {4E452D37-9B20-471D-B2AF-F6B0683A2D48}
  Call UninstallMSI
pop $R0

#2.6.0
push $R0
  StrCpy $R0 {8A081720-BCB6-4EC8-8602-48309AFE4AA1}
  Call UninstallMSI
pop $R0

#2.5.0
push $R0
  StrCpy $R0 {628381D5-AE8D-4C99-A236-7B10B23BA420}
  Call UninstallMSI
pop $R0

#Uninstalls previous installations of EveHQ via NSIS
ReadRegStr $R0 HKLM \
  "Software\Microsoft\Windows\CurrentVersion\Uninstall\EveHQ" \
  "UninstallString"
  StrCmp $R0 "" FinalCheck
 
  MessageBox MB_OKCANCEL|MB_ICONEXCLAMATION \
  "EveHQ is already installed. $\n$\nClick `OK` to remove the \
  previous version or `Cancel` to cancel this upgrade." \
  IDOK uninst
  Abort
 
#Run the uninstaller
uninst:
  ClearErrors
  ExecWait '$R0 _?=$INSTDIR' ;Do not copy the uninstaller to a temp file
 
  IfErrors no_remove_uninstaller done
    ;You can either use Delete /REBOOTOK in the uninstaller or add some code
    ;here to remove the uninstaller. Use a registry key to check
    ;whether the user has chosen to uninstall. If you are using an uninstaller
    ;components page, make sure all sections are uninstalled.
  no_remove_uninstaller:
 
FinalCheck:
#Check that the EveHQ.exe file is no longer present... will be the safety check that it has been uninstalled
IfFileExists $INSTDIR\EveHQ.exe EveHQPresent done

EveHQPresent:
MessageBox MB_OK \
"A possible old version of EveHQ appears to be currently installed in the folder $INSTDIR. Automatic attempts to detect the method to uninstall it have failed. \
Please manually uninstall the existing installation and then re-run this installer." \

abort


done:

var /GLOBAL cmdParams

${GetParameters} $cmdParams

var /GLOBAL useLocalFlag
StrCpy $useLocalFlag 0

#Check input parameters given to the installer.

    ; /local

    ${GetOptions} $cmdLineParams 'local' $R0

    IfErrors +2 0

    StrCpy $useLocalFlag 1
    

FunctionEnd

Function un.onInit

call un.EveHQNotRunning
FunctionEnd


Function CreateDesktopShortcut
${If} $useLocalFlag == "1"
CreateShortcut "$desktop\EveHQ.lnk" "$instdir\EveHQ.exe" "/local"
${Else}
CreateShortcut "$desktop\EveHQ.lnk" "$instdir\EveHQ.exe"
${EndIf}
FunctionEnd


#Ensure it isn't running

Function EveHQNotRunning
	IfFileExists "$INSTDIR\EveHQ.exe" 0 success
	IntOp $1 0 + 0
	
	tryToContinue:
	ClearErrors
	FileOpen $0 "$INSTDIR\EveHQ.exe" a
	IfErrors isRunning
	FileClose $0
	goto success

	isRunning:
	IfSilent waitForClose
	MessageBox MB_RETRYCANCEL|MB_DEFBUTTON1|MB_ICONEXCLAMATION \
			"Please close the EveHQ process before continuing. Note: It may be running minimized." /SD IDCANCEL IDRETRY tryToContinue IDCANCEL abort
	goto tryToContinue

	waitForClose:
	IntOp $1 $1 + 1
	IntCmp $1 60 0 0 attemptTimedOut
	Sleep 1000
	goto tryToContinue

	attemptTimedOut:
	Abort "EveHQ failed to close in an acceptable amount of time.."

	abort:
	Abort "Execution cancelled by user."
 
	success:
FunctionEnd


Function un.EveHQNotRunning
	IfFileExists "$INSTDIR\EveHQ.exe" 0 success
	IntOp $1 0 + 0
	
	tryToContinue:
	ClearErrors
	FileOpen $0 "$INSTDIR\EveHQ.exe" a
	IfErrors isRunning
	FileClose $0
	goto success

	isRunning:
	IfSilent waitForClose
	MessageBox MB_RETRYCANCEL|MB_DEFBUTTON1|MB_ICONEXCLAMATION \
			"Please close the EveHQ process before continuing. Note: It may be running minimized." /SD IDCANCEL IDRETRY tryToContinue IDCANCEL abort
	goto tryToContinue

	waitForClose:
	IntOp $1 $1 + 1
	IntCmp $1 60 0 0 attemptTimedOut
	Sleep 1000
	goto tryToContinue

	attemptTimedOut:
	Abort "EveHQ failed to close in an acceptable amount of time.."

	abort:
	Abort "Execution cancelled by user."
 
	success:
FunctionEnd


