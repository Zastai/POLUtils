;; Checks related to the .NET Framework

;; $Id$

Var DOTNET_AVAILABLE

Function CheckDotNet
  Pop $R0
  ;; Absolute Requirement: An installation root
  ReadRegStr $R1 HKLM "Software\Microsoft\.NETFramework" "InstallRoot"
  StrCmp "" $R1 NoDotNet
  ;; While the generic chech will find most "normal" installations of the .NET Framework 1.1, some slip through
  ;; the cracks, so check for 1.1 specially.
  StrCmp "v1.1" $R0 Special11Check GenericCheck
Special11Check:
  ReadRegDWORD $R2 HKLM "Software\Microsoft\NET Framework Setup\NDP\v1.1.4322" "Install"
  IfErrors NoDotNet
  IntCmp $R2 1 GotDotNet NoDotNet
GenericCheck:
  StrCpy $R2 0
  BuildCheckLoop:
    EnumRegValue $R3 HKLM "Software\Microsoft\.NETFramework\Policy\$R0" $R2
    IntOp $R2 $R2 + 1
    StrCmp $R3 "" NoDotNet
    IfFileExists "$R1$R0.$R3" GotDotNet BuildCheckLoop
  GoTo NoDotNet
GotDotNet:
  ClearErrors
  StrCpy $DOTNET_AVAILABLE "Y"
  GoTo TheEnd
NoDotNet:
  ClearErrors
  StrCpy $DOTNET_AVAILABLE "N"
TheEnd:
FunctionEnd
