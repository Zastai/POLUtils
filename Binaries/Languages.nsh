;; $Id$

!insertmacro MUI_LANGUAGE "English"

;; Enable these once all relevant strings are LangString'ed (and translated)
;;!insertmacro MUI_LANGUAGE "Dutch"
;;!insertmacro MUI_LANGUAGE "Japanese"

;; Enable these as soon as multiple languages are available
;; NOTE: Make sure the uninstaller is then also set up to reuse the installation language!
;;!insertmacro MUI_RESERVEFILE_LANGDLL
;;Function .onInit
;;  !insertmacro MUI_LANGDLL_DISPLAY
;;FunctionEnd

LangString DESC_SECTION_DESKTOP_SHORTCUT ${LANG_ENGLISH} "A shortcut to POLUtils on the Desktop."
LangString DESC_SECTION_ENGRISH_ONRY     ${LANG_ENGLISH} "A small utility to translate parts of JP FFXI into English (alpha release)."
LangString DESC_SECTION_MAIN             ${LANG_ENGLISH} "POLUtils itself."
LangString DESC_SECTION_TRANS            ${LANG_ENGLISH} "Translated resources for POLUtils (optional)."
LangString DESC_SECTION_TR_JA            ${LANG_ENGLISH} "Japanese resources for POLUtils."
LangString DESC_SECTION_TR_NL            ${LANG_ENGLISH} "Dutch resources for POLUtils."
LangString DESC_SHORTCUT                 ${LANG_ENGLISH} "A collection of PlayOnline-related utilities."
LangString LOG_DOTNET_FOUND              ${LANG_ENGLISH} "Found .NET framework 1.1"
LangString LOG_MDX_FOUND                 ${LANG_ENGLISH} "Found Managed DirectX - audio playback will be available."
LangString LOG_MDX_NOT_FOUND             ${LANG_ENGLISH} "WARNING: Managed DirectX not found - no audio playback possible until it is installed."
LangString NAME_SECTION_DESKTOP_SHORTCUT ${LANG_ENGLISH} "Desktop Shortcut"
LangString NAME_SECTION_ENGRISH_ONRY     ${LANG_ENGLISH} "$\"Engrish Onry$\""
LangString NAME_SECTION_MAIN             ${LANG_ENGLISH} "Main Program"
LangString NAME_SECTION_TRANS            ${LANG_ENGLISH} "Translations"
LangString NAME_SECTION_TR_JA            ${LANG_ENGLISH} "Japanese"
LangString NAME_SECTION_TR_NL            ${LANG_ENGLISH} "Dutch"
LangString MB_CODEPAGE_MISSING           ${LANG_ENGLISH} "At least one required codepage (Western, Cyrillic or Shift-JIS) is not available on this system.$\r$\nThe FFXI Data Browser and Macro Manager will not run until they are installed."
LangString MB_DOTNET_NOT_FOUND           ${LANG_ENGLISH} "Version 1.1 of the Microsoft .NET Framework was not found on this system.$\r$\nYou should be able to install it using Windows Update (Windows 98 or higher required).$\r$\n$\r$\nUnable to continue this installation."
LangString MB_ENSURE_TRUSTED_SOURCE      ${LANG_ENGLISH} "Please ensure you downloaded this file from ${SITE_URL} and nowhere else.$\r$\nFiles from other sources may have been modified to contain viruses and/or keyloggers that could result in your POL account being hacked."
LangString MB_SPECIAL_BUILD              ${LANG_ENGLISH} "This is a special (${BUILD}) build; please do not distribute it without the explicit consent of the author."
LangString MB_MDX_NOT_FOUND              ${LANG_ENGLISH} "Managed DirectX does not seem to be installed on this system$\r$\naAudio playback will not be available.$\r$\n$\r$\nAn installer for Managed DirectX is available on the web page."
LangString MB_DELETE_MACROLIB_V4         ${LANG_ENGLISH} "Do you want to remove your old (pre-0.5.0) macro library?$\r$\nIts contents have been copied to the new location for 0.5.0 and higher, so you only need this if you want to go back to version 0.4.0 or earlier."
LangString MB_DELETE_CURRENT_MACROLIB    ${LANG_ENGLISH} "Do you want to remove your macro library?"
LangString SITE_NAME                     ${LANG_ENGLISH} "Pebbles' Program Page"
LangString UI_RUNPROG                    ${LANG_ENGLISH} "&Run POLUtils"
LangString UNINSTALL_SHORTCUT            ${LANG_ENGLISH} "Uninstall POLUtils"

;; Local Variables:
;; truncate-lines: t
;; End:
