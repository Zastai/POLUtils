# Requirements for Building POLUtils

Main requirement is Visual Studio 2012 or higher. 2013 or higher is recommended, for the built-in Git support.

Note: If I make any further changes, there's a reasonable chance C# 6 language features will slip in. At that point VS2015 or higher will be required.

Target framework should remain at v4.0 until further notice, to keep support for Windows XP.

Building the installer currently requires [NSIS](http://nsis.sourceforge.net/).
Attempts were made to use WiX instead, but that wasn't a big success. It may still happen at some point.

See also CODING.md for coding and style guidelines, if you want to modify the code.
