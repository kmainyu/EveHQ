@echo off
CD /D %1
SubWCRev.exe "." ".\EveHQ\My Project\AssemblyInfo.template.vb" ".\EveHQ\My Project\AssemblyInfo.vb" -f
SubWCRev.exe "." ".\EveHQ.Core\My Project\AssemblyInfo.template.vb" ".\EveHQ.Core\My Project\AssemblyInfo.vb" -f
SubWCRev.exe "." ".\EveHQ.CoreControls\Properties\AssemblyInfo.template.cs" ".\EveHQ.CoreControls\Properties\AssemblyInfo.cs" -f
SubWCRev.exe ".\EveHQ.CorpHQ" ".\EveHQ.CorpHQ\My Project\AssemblyInfo.template.vb" ".\EveHQ.CorpHQ\My Project\AssemblyInfo.vb" -f
SubWCRev.exe ".\EveHQ.DataConverter" ".\EveHQ.DataConverter\My Project\AssemblyInfo.template.vb" ".\EveHQ.DataConverter\My Project\AssemblyInfo.vb" -f
SubWCRev.exe "." ".\EveHQ.EveAPI\My Project\AssemblyInfo.template.vb" ".\EveHQ.EveAPI\My Project\AssemblyInfo.vb" -f
SubWCRev.exe ".\EveHQ.EveAPIProxy" ".\EveHQ.EveAPIProxy\My Project\AssemblyInfo.template.vb" ".\EveHQ.EveAPIProxy\My Project\AssemblyInfo.vb" -f
SubWCRev.exe ".\EveHQ.HQF" ".\EveHQ.HQF\My Project\AssemblyInfo.template.vb" ".\EveHQ.HQF\My Project\AssemblyInfo.vb" -f
SubWCRev.exe ".\EveHQ.ItemBrowser" ".\EveHQ.ItemBrowser\My Project\AssemblyInfo.template.vb" ".\EveHQ.ItemBrowser\My Project\AssemblyInfo.vb" -f
SubWCRev.exe ".\EveHQ.KillMailViewer" ".\EveHQ.KillMailViewer\My Project\AssemblyInfo.template.vb" ".\EveHQ.KillMailViewer\My Project\AssemblyInfo.vb" -f
SubWCRev.exe ".\EveHQ.PI" ".\EveHQ.PI\Properties\AssemblyInfo.template.cs" ".\EveHQ.PI\Properties\AssemblyInfo.cs" -f
SubWCRev.exe ".\EveHQ.PosManager" ".\EveHQ.PosManager\Properties\AssemblyInfo.template.cs" ".\EveHQ.PosManager\Properties\AssemblyInfo.cs" -f
SubWCRev.exe ".\EveHQ.Prism" ".\EveHQ.Prism\My Project\AssemblyInfo.template.vb" ".\EveHQ.Prism\My Project\AssemblyInfo.vb" -f
SubWCRev.exe ".\EveHQ.RouteMap" ".\EveHQ.RouteMap\Properties\AssemblyInfo.template.cs" ".\EveHQ.RouteMap\Properties\AssemblyInfo.cs" -f
SubWCRev.exe ".\EveHQ.Void" ".\EveHQ.Void\My Project\AssemblyInfo.template.vb" ".\EveHQ.Void\My Project\AssemblyInfo.vb" -f
SubWCRev.exe ".\EveHQPatcher" ".\EveHQPatcher\My Project\AssemblyInfo.template.vb" ".\EveHQPatcher\My Project\AssemblyInfo.vb" -f
