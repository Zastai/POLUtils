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
!define MUI_FINISHPAGE_RUN "$INSTDIR\POLUtils.exe"
!define MUI_FINISHPAGE_RUN_TEXT "&Run POLUtils"
!define MUI_FINISHPAGE_RUN_NOTCHECKED
!define MUI_FINISHPAGE_LINK "Pebbles' Program Page"
!define MUI_FINISHPAGE_LINK_LOCATION ${SITE_URL}

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

!insertmacro MUI_LANGUAGE "English"

;; Enable these once all relevant strings are LangString'ed (and translated)
;;!insertmacro MUI_LANGUAGE "Dutch"
;;!insertmacro MUI_LANGUAGE "Japanese"
;;!insertmacro MUI_RESERVEFILE_LANGDLL
;;Function .onInit
;;  !insertmacro MUI_LANGDLL_DISPLAY
;;FunctionEnd

InstType "Basic"
InstType "Full"

!include "DotNet.nsh"

;; --- Sections ---

Section "-DotNetCheck"
  Push "v1.1"
  Call CheckDotNet
  StrCmp $DOTNET_VERSION "" "" NoAbort
    MessageBox MB_OK|MB_ICONSTOP "Version 1.1 of the Microsoft .NET Framework was not found on this system.$\r$\nYou should be able to install it using Windows Update (Windows 98 or higher required).$\r$\n$\r$\nUnable to continue this installation."
    Abort
  NoAbort:
  DetailPrint "Found .NET framework $DOTNET_VERSION (build $DOTNET_BUILD) (mscorlib.dll version $DOTNET_DLLVERSION)"
SectionEnd

Section "-ManagedDirectXCheck"
  IfFileExists $WINDIR\Assembly\GAC\Microsoft.DirectX.* MDXFound
    MessageBox MB_OK|MB_ICONINFORMATION "Managed DirectX does not seem to be installed on this system$\r$\naAudio playback will not be available.$\r$\n$\r$\nAn installer for Managed DirectX is available on the web page."
    DetailPrint "WARNING: Managed DirectX not found - no audio playback possible until it is installed."
    GoTo MDXTestDone
  MDXFound:
    DetailPrint "Found Managed DirectX - audio playback will be available."
  MDXTestDone:
SectionEnd

Section "Main Program" SECTION_MAIN
  SectionIn 1 2 RO
  SetOutPath "$INSTDIR"
  File "${BUILDDIR}\PlayOnline.Core.dll"
  File "${BUILDDIR}\PlayOnline.FFXI.dll"
  File "${BUILDDIR}\PlayOnline.Utils.AudioManager.dll"
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
    WriteINIStr "$SMPROGRAMS\$START_MENU_FOLDER\Pebbles' Program Page.url" "InternetShortCut" "URL" ${SITE_URL}
    CreateShortCut "$SMPROGRAMS\$START_MENU_FOLDER\Uninstall POLUtils.lnk" "$INSTDIR\uninstall.exe" "" "" 0 SW_SHOWNORMAL "" ""
  !insertmacro MUI_STARTMENU_WRITE_END
  ; Store install folder
  WriteRegStr HKCU "Software\Pebbles\POLUtils" "Install Location" $INSTDIR
SectionEnd

Section "un.Main Program"
  Delete "$INSTDIR\PlayOnline.Core.dll"
  Delete "$INSTDIR\PlayOnline.FFXI.dll"
  Delete "$INSTDIR\PlayOnline.Utils.AudioManager.dll"
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
    Delete "$SMPROGRAMS\$START_MENU_FOLDER\Pebbles' Program Page.url"
    Delete "$SMPROGRAMS\$START_MENU_FOLDER\Uninstall POLUtils.lnk"
    RMDir  "$SMPROGRAMS\$START_MENU_FOLDER"
    GoTo EndSMClean
  NoSMSubDir:
    Delete "$SMPROGRAMS\POLUtils.lnk"
    Delete "$SMPROGRAMS\Pebbles' Program Page.url"
  EndSMClean:
SectionEnd

;; --- Section Descriptions ---

LangString DESC_SECTION_MAIN                      ${LANG_ENGLISH} "POLUtils itself."
LangString DESC_SECTION_TRANS                     ${LANG_ENGLISH} "Translated resources for POLUtils (optional)."
LangString DESC_SECTION_TR_JA                     ${LANG_ENGLISH} "Japanese resources for POLUtils."
LangString DESC_SECTION_TR_NL                     ${LANG_ENGLISH} "Dutch resources for POLUtils."
LangString DESC_SECTION_DESKTOP_SHORTCUT          ${LANG_ENGLISH} "A shortcut to POLUtils on the Desktop."
LangString DESC_SHORTCUT                          ${LANG_ENGLISH} "A collection of PlayOnline-related utilities."

!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
  !insertmacro MUI_DESCRIPTION_TEXT ${SECTION_MAIN}             $(DESC_SECTION_MAIN)
  !insertmacro MUI_DESCRIPTION_TEXT ${SECTION_TRANS}            $(DESC_SECTION_TRANS)
  !insertmacro MUI_DESCRIPTION_TEXT ${SECTION_TR_JA}            $(DESC_SECTION_TR_JA)
  !insertmacro MUI_DESCRIPTION_TEXT ${SECTION_TR_NL}            $(DESC_SECTION_TR_NL)
  !insertmacro MUI_DESCRIPTION_TEXT ${SECTION_DESKTOP_SHORTCUT} $(DESC_SECTION_DESKTOP_SHORTCUT)
!insertmacro MUI_FUNCTION_DESCRIPTION_END
