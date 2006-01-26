Translating the POLUtils installer is discussed in the readme.txt in
the Installer subdirectory.

Translating the main POLUtils application consists of basically 2 parts:

- The UI resources (form/control text & layout)
- The string table resources

Translating UI Resources
------------------------

Once you perform an initial compile of the solution in Visual Studio,
you should be able to open any of the forms and/or user controls in
the designer.
Set the Language property of the form or control to the language you're
translating into.  Please use the general version (e.g. "French"), not
the country-specific version unless it's actualy relevant (e.g.
"Spanish (Spain)" vs "Spanish (Mexico)").  Setting this property
should trigger Visual Studio to create a <Form>.<language>.resx file
for that language.
Then you're free to adjust the text on the form/control and/or change
the layout if it's necessary (but try to keep things looking more or
less the same than the basic English version).

Translating String Table Resources
----------------------------------

These are in files called Messages[.language].resx and unlike the UI
resources can be translated without having to use Visual Studio.
To get started, copy the Messages.resx file to Messages.[lang].resx, where
[lang] is the language code for the language you're translating into (e.g.
"de" for German, "it" for Italian, "fr_CA" for Canadian French)
Then it's just a matter of editing the file, translating the English values
(but not the names!). The .resx files are simple XML, so any text editor will
do - alternatively, you can use the Resourcer tool written by Lutz Roeder
(http://www.aisto.com/roeder/dotnet/ - use the executable built for .NET 2.0)
