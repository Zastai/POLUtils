;; --- General Settings ---

!include "MUI.nsh"

SetCompressor LZMA
AllowSkipFiles off
SetOverwrite ifnewer
CRCCheck on

Name "POLUtils"

!define SITE_URL "http://users.telenet.be/pebbles/"

!include "Version.nsh"

OutFile "Installers\POLUtils-${VERSION}-${BUILDDIR}.exe"

InstallDir       "$PROGRAMFILES\Pebbles\POLUtils"
InstallDirRegKey HKCU "Software\Pebbles\POLUtils" "Install Location"

!define MUI_ICON                 "${NSISDIR}\Contrib\Graphics\Icons\modern-install-colorful.ico"
!define MUI_UNICON               "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall-colorful.ico"
!define MUI_HEADERIMAGE_BITMAP   "${NSISDIR}\Contrib\Graphics\Header\win.bmp"
!define MUI_HEADERIMAGE_UNBITMAP "${NSISDIR}\Contrib\Graphics\Header\win.bmp"

!define MUI_COMPONENTSPAGE_SMALLDESC
!define MUI_ABORTWARNING
!define MUI_UNABORTWARNING
!define MUI_HEADERIMAGE

!define MUI_FINISHPAGE_NOAUTOCLOSE
!define MUI_FINISHPAGE_RUN_NOTCHECKED
!define MUI_FINISHPAGE_RUN            $INSTDIR\POLUtils.exe
!define MUI_FINISHPAGE_RUN_TEXT       $(UI_RUNPROG)
!define MUI_FINISHPAGE_LINK           $(SITE_NAME)
!define MUI_FINISHPAGE_LINK_LOCATION  ${SITE_URL}

Var START_MENU_FOLDER

!define MUI_STARTMENUPAGE_DEFAULTFOLDER      "Pebbles"
!define MUI_STARTMENUPAGE_REGISTRY_ROOT      HKCU
!define MUI_STARTMENUPAGE_REGISTRY_KEY       "Software\Pebbles\POLUtils"
!define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "Start Menu Folder"

!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_STARTMENU "StartMenu" $START_MENU_FOLDER
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

!include "Languages.nsh"

InstType "Basic"
InstType "Full"

!include "DotNet.nsh"

;; --- Sections ---

Section "-DotNetCheck"
  Push "v1.1"
  Call CheckDotNet
  StrCmp $DOTNET_VERSION "" 0 NoAbort
    MessageBox MB_OK|MB_ICONSTOP $(MB_DOTNET_NOT_FOUND)
    Abort
  NoAbort:
  DetailPrint $(LOG_DOTNET_FOUND)
SectionEnd

Section "-ManagedDirectXCheck"
  IfFileExists $WINDIR\Assembly\GAC\Microsoft.DirectX.* MDXFound
    MessageBox MB_OK|MB_ICONINFORMATION $(MB_MDX_NOT_FOUND)
    DetailPrint $(LOG_MDX_NOT_FOUND)
    GoTo MDXTestDone
  MDXFound:
    DetailPrint $(LOG_MDX_FOUND)
  MDXTestDone:
SectionEnd

Section "-CodePageCheck"
  ReadRegStr $0 HKLM SYSTEM\CurrentControlSet\Control\Nls\Codepage "932"
  StrCmp $0 "" CPNoSJIS
  ReadRegStr $0 HKLM SYSTEM\CurrentControlSet\Control\Nls\Codepage "1251"
  StrCmp $0 "" CPNoCyrillic
  ReadRegStr $0 HKLM SYSTEM\CurrentControlSet\Control\Nls\Codepage "1252"
  StrCmp $0 "" CPNoWestern
  DetailPrint "All necessary codepages are available."
  GoTo CPTestDone
CPNoSJIS:
  DetailPrint "Shift-JIS codepage not available."
  GoTo CPNotFound
CPNoCyrillic:
  DetailPrint "Cyrillic codepage not available."
  GoTo CPNotFound
CPNoWestern:
  DetailPrint "Western codepage not available."
  GoTo CPNotFound
CPNotFound:
  MessageBox MB_OK|MB_ICONINFORMATION "At least one required codepage (Western, Cyrillic or Shift-JIS) is not available on this system.$\r$\nThe FFXI Data Browser and Macro Manager will not run until they are installed."
  GoTo CPTestDone
CPTestDone:
SectionEnd

Section "Main Program" SECTION_MAIN
  SectionIn 1 2 RO
  SetOutPath "$INSTDIR"
  File "${BUILDDIR}\PlayOnline.Core.dll"
  File "${BUILDDIR}\PlayOnline.FFXI.dll"
  File "${BUILDDIR}\PlayOnline.Utils.AudioManager.dll"
  File "${BUILDDIR}\PlayOnline.Utils.FFXIConfigEditor.dll"
  File "${BUILDDIR}\PlayOnline.Utils.FFXIDataBrowser.dll"
  File "${BUILDDIR}\PlayOnline.Utils.FFXIMacroManager.dll"
  File "${BUILDDIR}\PlayOnline.Utils.TetraViewer.dll"
  File "${BUILDDIR}\POLUtils.exe"
  File "${BUILDDIR}\POLUtils.exe.manifest"
SectionEnd

SubSection "Translations" SECTION_TRANS

  Section "Dutch" SECTION_TR_NL
    SectionIn 2
    SetOutPath "$INSTDIR\nl"
    File "${BUILDDIR}\nl\*.resources.dll"
  SectionEnd

  Section "Japanese" SECTION_TR_JA
    SectionIn 2
    SetOutPath "$INSTDIR\ja"
    File "${BUILDDIR}\ja\*.resources.dll"
  SectionEnd

SubSectionEnd

Section "Desktop Shortcut" SECTION_DESKTOP_SHORTCUT
  SetOutPath "$INSTDIR"
  CreateShortCut "$DESKTOP\POLUtils.lnk" "$INSTDIR\POLUtils.exe" "" "shell32.dll" 165 SW_SHOWNORMAL "" $(DESC_SHORTCUT)
SectionEnd

Section "-FinishUp"
  ; write uninstall information
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\POLUtils" "DisplayName"     "POLUtils (remove only)"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\POLUtils" "UninstallString" '"$INSTDIR\uninstall.exe"'
  SetOutPath $INSTDIR
  WriteUninstaller "uninstall.exe"
  ; create start menu entries if requested
  !insertmacro MUI_STARTMENU_WRITE_BEGIN "StartMenu"
    CreateDirectory "$SMPROGRAMS\$START_MENU_FOLDER"
    SetOutPath "$INSTDIR"
    CreateShortCut "$SMPROGRAMS\$START_MENU_FOLDER\POLUtils.lnk" "$INSTDIR\POLUtils.exe" "" "shell32.dll" 165 SW_SHOWNORMAL "" $(DESC_SHORTCUT)
    WriteINIStr "$SMPROGRAMS\$START_MENU_FOLDER\$(SITE_NAME).url" "InternetShortCut" "URL" ${SITE_URL}
    CreateShortCut "$SMPROGRAMS\$START_MENU_FOLDER\Uninstall POLUtils.lnk" "$INSTDIR\uninstall.exe" "" "" 0 SW_SHOWNORMAL "" ""
  !insertmacro MUI_STARTMENU_WRITE_END
  ; Store install folder
  WriteRegStr HKCU "Software\Pebbles\POLUtils" "Install Location" $INSTDIR
SectionEnd

Section "un.Main Program"
  Delete "$INSTDIR\PlayOnline.Core.dll"
  Delete "$INSTDIR\PlayOnline.FFXI.dll"
  Delete "$INSTDIR\PlayOnline.Utils.AudioManager.dll"
  Delete "$INSTDIR\PlayOnline.Utils.FFXIConfigEditor.dll"
  Delete "$INSTDIR\PlayOnline.Utils.FFXIDataBrowser.dll"
  Delete "$INSTDIR\PlayOnline.Utils.FFXIMacroManager.dll"
  Delete "$INSTDIR\PlayOnline.Utils.TetraViewer.dll"
  Delete "$INSTDIR\POLUtils.exe"
  Delete "$INSTDIR\POLUtils.exe.manifest"
SectionEnd

SubSection "un.Translations"

  Section "un.Dutch" UNSECTION_TR_NL
    Delete "$INSTDIR\nl\*.resources.dll"
    RMDir "$INSTDIR\nl"
  SectionEnd

  Section "un.Japanese" UNSECTION_TR_JA
    Delete "$INSTDIR\ja\*.resources.dll"
    RMDir "$INSTDIR\ja"
  SectionEnd

SubSectionEnd

Section "un.Desktop Shortcut"
  Delete "$DESKTOP\POLUtils.lnk"
SectionEnd

Section "un.TheRest"
  Delete "$INSTDIR\uninstall.exe"
  RMDir "$INSTDIR"
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\POLUtils"
  DeleteRegKey HKCU "Software\Pebbles\POLUtils"
  !insertmacro MUI_STARTMENU_GETFOLDER "StartMenu" $START_MENU_FOLDER
  StrCmp $START_MENU_FOLDER "" NoSMSubDir
    Delete "$SMPROGRAMS\$START_MENU_FOLDER\POLUtils.lnk"
    Delete "$SMPROGRAMS\$START_MENU_FOLDER\$(SITE_NAME).url"
    Delete "$SMPROGRAMS\$START_MENU_FOLDER\Uninstall POLUtils.lnk"
    RMDir  "$SMPROGRAMS\$START_MENU_FOLDER"
    GoTo EndSMClean
  NoSMSubDir:
    Delete "$SMPROGRAMS\POLUtils.lnk"
    Delete "$SMPROGRAMS\Pebbles' Program Page.url"
  EndSMClean:
SectionEnd

;; --- Section Descriptions ---

!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
  !insertmacro MUI_DESCRIPTION_TEXT ${SECTION_MAIN}             $(DESC_SECTION_MAIN)
  !insertmacro MUI_DESCRIPTION_TEXT ${SECTION_TRANS}            $(DESC_SECTION_TRANS)
  !insertmacro MUI_DESCRIPTION_TEXT ${SECTION_TR_JA}            $(DESC_SECTION_TR_JA)
  !insertmacro MUI_DESCRIPTION_TEXT ${SECTION_TR_NL}            $(DESC_SECTION_TR_NL)
  !insertmacro MUI_DESCRIPTION_TEXT ${SECTION_DESKTOP_SHORTCUT} $(DESC_SECTION_DESKTOP_SHORTCUT)
!insertmacro MUI_FUNCTION_DESCRIPTION_END
