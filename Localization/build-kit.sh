#!/bin/bash

OUTDIR=translator-kit

# Always create a kit from scratch
test -d "$OUTDIR" || mkdir "$OUTDIR"
rm -rf "$OUTDIR"/*

cp -p solution.base "$OUTDIR/POLUtils-Localization.sln"

exec 5>"$OUTDIR/POLUtils-Localization.csproj"
cat project.top >&5

# Locate designer sources
for FILE in $(find .. -type f -name '*.Designer.cs' -o -name '*.resx'); do
  dir=$(dirname $FILE)
  project=$(dirname $FILE | cut -d/ -f2)
  file=$(basename $FILE)
  echo "adding $file from $project"
  test -d "$OUTDIR/$project" || mkdir "$OUTDIR/$project"
  cp -p "$FILE" "$OUTDIR/$project/"
  case $file in
    *.resx)
      base=$(printf %q "$file" | sed -e 's/\([.][a-z][a-z]\(_[A-Z][A-Z]\|\)\|\)[.]resx$//')
      echo "    <EmbeddedResource Include=\"$project\\$file\">" >&5
      if test -f "$dir/$base.Designer.cs" && test -f "$dir/$base.cs"; then
        echo "      <DependentUpon>$base.cs</DependentUpon>" >&5
      fi
      echo "    </EmbeddedResource>" >&5
      ;;
    *.Designer.cs)
      base=$(printf %q "$file" | sed -e 's/[.]Designer[.]cs$//')
      if test -f "$dir/$base.cs"; then
        if test ! -f "$OUTDIR/$project/$base.cs"; then
          echo "    <Compile Include=\"$project\\$base.cs\" />" >&5
          awk -fextract_base.awk < "$dir/$base.cs" > $OUTDIR/$project/$base.cs
        fi
      fi
      echo "    <Compile Include=\"$project\\$file\">" >&5
      if test -f "$dir/$base.cs"; then
        echo "      <DependentUpon>$base.cs</DependentUpon>" >&5
      fi
      echo "    </Compile>" >&5
      ;;
  esac
done

# Addition localization resources
test -d "$OUTDIR/Installer" || mkdir "$OUTDIR/Installer"
cp "../Binaries/Languages.nsh" "$OUTDIR/Installer/"
echo "    <Content Include=\"Installer\\Languages.nsh\" />" >&5

# Finish up
cat project.bottom >&5
exec 5>&-
