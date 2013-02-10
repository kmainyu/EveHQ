
SetCompressor /solid lzma
;Header Info
;--------------------------------

!include DotNetFramework.nsh
!include MUI2.nsh
!include x64.nsh
!include SQLCE40.nsh
!include Upgrade.nsh


Name "EveHQ"
OutFile "EveHQ-Setup.exe"

RequestExecutionLevel admin

InstallDir $PROGRAMFILES\EveHQ
InstallDirRegKey HKLM "Software\EveHQ" "Install_Dir"



!define MUI_ICON "..\EveHQ\Resources\EveHQ.ico"
!define MUI_UNICON "..\EveHQ\Resources\EveHQ.ico"
!define MUI_WELCOMEFINISHPAGE_BITMAP EveHQInstallerSideImage.bmp

#Installer file settings
VIAddVersionKey "CompanyName" "Software Addicts Studios"
VIAddVersionKey "FileDescription" "Installs EveHQ: The Internet Spaceship Toolki"
VIAddVersionKey "LegalCopyright" "Copyright 2005-2013, EveHQ Dev Team"
VIAddVersionKey "ProductName" "EveHQ Setup"


#Page Declarations
;--------------------------------
#Welcome page

!insertmacro MUI_PAGE_WELCOME

#License
!define MUI_LICENSEPAGE_CHECKBOX
!define MUI_LICENSEPAGE_CHECKBOX_TEXT "I accept the terms"
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
!define MUI_FINISHPAGE_RUN $INSTDIR\EveHQ.exe
!define MUI_FINISHPAGE_RUN_TEXT "Run EveHQ.exe"
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
  
  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  File "..\BuildOutput\Release\DevComponents.DotNetBar2.dll"
  File "..\BuildOutput\Release\EveCacheParser.dll"
  File "..\BuildOutput\Release\EveHQ.Core.dll"
  File "..\BuildOutput\Release\EveHQ.CoreControls.dll"
  File "..\BuildOutput\Release\EveHQ.DataConverter.dll"
  File "..\BuildOutput\Release\EveHQ.DataUpgrader.exe"
  File "..\BuildOutput\Release\EveHQ.DataUpgrader.exe.config"
  File "..\BuildOutput\Release\EveHQ.EveAPI.dll"
  File "..\BuildOutput\Release\EveHQ.exe"
  File "..\BuildOutput\Release\EveHQ.exe.config"
  File "..\BuildOutput\Release\EveHQ.HQF.dll"
  File "..\BuildOutput\Release\EveHQ.ItemBrowser.dll"
  File "..\BuildOutput\Release\EveHQ.KillMailViewer.dll"
  File "..\BuildOutput\Release\EveHQ.Market.dll"
  File "..\BuildOutput\Release\EveHQ.Prism.dll"
  File "..\BuildOutput\Release\EveHQ.Void.dll"
  File "..\BuildOutput\Release\EveHQPatcher.exe"
  File "..\BuildOutput\Release\EveHQPatcher.exe.config"
  File "..\BuildOutput\Release\GammaJul.lglcd.dll"
  File "..\BuildOutput\Release\Newtonsoft.json.dll"
  File "..\BuildOutput\Release\System.Threading.dll"
  File "..\EveHQ.Data\EveHQ.sdf"
  File "..\EveHQ\License.txt"

 ; Write the installation path into the registry
  WriteRegStr HKLM SOFTWARE\EveHQ "Install_Dir" "$INSTDIR"
  
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
  Delete $INSTDIR\*.*

  ; Remove shortcuts, if any
  Delete "$SMPROGRAMS\EveHQ\*.*"

  ; Remove directories used
  RMDir "$SMPROGRAMS\EveHQ"
  RMDir "$INSTDIR"

SectionEnd
  
  
  
#Functions
#--------------------------------------------

Function .onInit

#uninstall the msi version.
push $R0
  StrCpy $R0 {0613D880-939E-4C9D-AD7C-A10DF7D7D5E9
  Call UninstallMSI
pop $R0

!insertmacro REMOVE_FAKE_MSICOMPONENTS "0613D880939E4C9DAD7CA10DF7D7D5E9" "5EBB032870C24A58A4AC-225419B25E51"
# sets a silly high version that should block old eveHQ msi installers from isntalling old and busted versions.
!insertmacro WRITE_FAKE_MSICOMPONENTS "0613D880939E4C9DAD7CA10DF7D7D5E9" "5EBB032870C24A58A4AC-225419B25E51" 9 9 9 9

#Uninstalls previous installations of EveHQ via NSIS
ReadRegStr $R0 HKLM \
  "Software\Microsoft\Windows\CurrentVersion\Uninstall\EveHQ" \
  "UninstallString"
  StrCmp $R0 "" done
 
  MessageBox MB_OKCANCEL|MB_ICONEXCLAMATION \
  "EveHQ is already installed. $\n$\nClick `OK` to remove the \
  previous version or `Cancel` to cancel this upgrade." \
  IDOK uninst
  Abort
 
;Run the uninstaller
uninst:
  ClearErrors
  ExecWait '$R0 _?=$INSTDIR' ;Do not copy the uninstaller to a temp file
 
  IfErrors no_remove_uninstaller done
    ;You can either use Delete /REBOOTOK in the uninstaller or add some code
    ;here to remove the uninstaller. Use a registry key to check
    ;whether the user has chosen to uninstall. If you are using an uninstaller
    ;components page, make sure all sections are uninstalled.
  no_remove_uninstaller:
 

done:


FunctionEnd





#Ensure it isn't running



