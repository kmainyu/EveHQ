



#Will check for and uninstall previous MSI installations.
Function UninstallMSI
  ; $R0 should contain the GUID of the application
  push $R1
  ReadRegStr $R1 HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\$R0" "UninstallString"
  StrCmp $R1 "" UninstallMSI_nomsi
    MessageBox MB_YESNOCANCEL|MB_ICONQUESTION  "A previous version of EveHQ was found. It is recommended that you uninstall it first.$\n$\nDo you want to do that now?" IDNO UninstallMSI_nomsi IDYES UninstallMSI_yesmsi
      Abort
UninstallMSI_yesmsi:
    ExecWait '"msiexec.exe" /x $R0'
    # making sure the entry is gone and that a crappy MSI doesn't leave "residue"
    DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\$R0"
    MessageBox MB_OK|MB_ICONINFORMATION "Click OK to continue upgrading your version of EveHQ."
UninstallMSI_nomsi: 
  pop $R1
FunctionEnd

