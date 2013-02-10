



#Will check for and uninstall previous MSI installations.
Function UninstallMSI
  ; $R0 should contain the GUID of the application
  push $R1
  ReadRegStr $R1 HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\$R0" "UninstallString"
  StrCmp $R1 "" UninstallMSI_nomsi
    MessageBox MB_YESNOCANCEL|MB_ICONQUESTION  "A previous version of EveHQ was found. It is recommended that you uninstall it first.$\n$\n\Do you want to do that now?" IDNO UninstallMSI_nomsi IDYES UninstallMSI_yesmsi
      Abort
UninstallMSI_yesmsi:
    ExecWait '"msiexec.exe" /x $R0'
    MessageBox MB_OK|MB_ICONINFORMATION "Click OK to continue upgrading your version of EveHQ."
UninstallMSI_nomsi: 
  pop $R1
FunctionEnd


# Adds an entry to the registry so that old MSI installers will see that there is an upgrade in place
!macro BUILD_MSICOMPATIBLE_VERNUM MAJOR MINOR RELEASE BUILD RESULT
  StrCpy ${RESULT} "0"
 
  IntOp $R0 ${MAJOR} * 0x1000000
  IntOp $R1 ${MINOR} * 0x10000
  IntOp $R2 ${RELEASE} * 0x100
 
  IntOp ${RESULT} ${RESULT} + $R0
  IntOp ${RESULT} ${RESULT} + $R1
  IntOp ${RESULT} ${RESULT} + $R2
  IntOp ${RESULT} ${RESULT} + ${BUILD}
!macroend
 
!macro WRITE_FAKE_MSICOMPONENTS TURNPCODE TURNUCODE MAJ MIN REL BUILD
  !insertmacro BUILD_MSICOMPATIBLE_VERNUM ${MAJ} ${MIN} ${REL} ${BUILD} $0
  WriteRegDword HKEY_LOCAL_MACHINE "Software\Classes\Installer\Products\${TURNPCODE}" "Version" $0
  WriteRegDword HKEY_LOCAL_MACHINE "Software\Classes\Installer\Products\${TURNPCODE}" "Assignment" 0x1
  WriteRegStr HKEY_LOCAL_MACHINE "Software\Classes\Installer\UpgradeCodes\${TURNUCODE}" "${TURNPCODE}" ""
!macroend

!macro REMOVE_FAKE_MSICOMPONENTS TURNPCODE TURNUCODE
  DeleteRegKey HKEY_LOCAL_MACHINE "Software\Classes\Installer\Products\${TURNPCODE}"
  DeleteRegKey HKEY_LOCAL_MACHINE "Software\Classes\Installer\UpgradeCodes\${TURNUCODE}"
!macroend