## Algorithm Description ##

Several of the FFXI DATs are "encrypted" using a simple per-byte bit rotation.
For some DATs (like item data) it's a fixed-size rotation, for others (like the ability and spell info), it's based on the data (with a distinction between text-only and other data).

_Note:_ all rotations are to the **right**.

For text-only data (at least 2 bytes of data required) the variable rotation is:
```
  if (data[0] == 0 && data[1] == 0)
    return 0;
int seed = countbits (data[1]) - countbits (data[0]);
  switch (abs (seed) % 5) {
    case 0: return 1;
    case 1: return 7;
    case 2: return 2;
    case 3: return 6;
    case 4: return 3;
  }
```

For all other data (at least 13 bytes of data required) it's:
```
int seed = countbits (data[2]) - countbits (data[11]) + countbits (data[12]);
  switch (abs (seed) % 5) {
    case 0: return 7;
    case 1: return 1;
    case 2: return 6;
    case 3: return 2;
    case 4: return 5;
  }
```

In both cases, `countbits()` is a function that returns the number of bits set in that byte.

As an example, say you have an ability data block that you want to decode.  You'll need bytes 3, 12 and 13 from it - let's say they're `0x04`, `0xCA` and `0xD8`. That means they are `00000100`, `11001010` and `11011000` in binary, so they have 1, 4, and 4 bits set, respectively.
This means the 'seed' is `1 - 4 + 4 = 1`, which leads to a rotation size of 1 - so to decrypt the ability block, you need to rotate each byte 1 bit to the right.

If someone can suggest a simpler calculation that returns the same result as these for both cases, please let me know ^^

## Using POLUtils to Decode Data ##

The decryption routines are available from the POLUtils PlayOnline.FFXI assembly:
```
using PlayOnline.FFXI;
...
  // For the fixed rotations:
  FFXIEncryption.Rotate(mydata, bits);
  // For the variable rotations:
  FFXIEncryption.DecodeDataBlock(mydata);
  FFXIEncryption.DecodeTextBlock(mydata);
...
```

Note that for POLUtils 0.9.0 and up, the assemblies are target-specific (x86 or x64); this may mean they won't load if your app is built as Any CPU.  By changing the build to x86 (and including the x86 POLUtils assembly), your app will run on all platforms; there's no real need for a separate x64 build unless your app contains CPU- or memory-intensive processing that might benefit from it.