BEGIN {
  FS = " ";
  controlcount = 0;
}

/[+]= new .*[(].*[)];/ { # eliminate event handlers
  next;
}

/PlayOnline[.]FFXI[.]FFXIItemEditor (.*);/ { # keep track of any item editor controls
  controlname = $NF;
  sub (/;[[:space:]]*$/, "", controlname);
  controlnames[controlcount++] = controlname;
}

{ # keep every line, except settings of an item editor's Item property
  for (i = 0; i < controlcount; ++i) {
    if (match($0, controlnames[i] "[.]Item")) {
      next;
    }
  }
  print $0;
}
