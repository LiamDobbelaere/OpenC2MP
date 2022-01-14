# AntiCheat

I picked this one first to test the analyze-document-implement cycle. It seems like a relatively straightforward file to begin with.

AntiCheat is instantiated in Main as a member, however in my opinion it may very well have been a static class instead. There's no reason for having an AntiCheat instance as it doesn't have any instance members. Interestingly, AntiCheat also contains a reference back to Main -> very tight coupling and thus an anti-pattern.

## Members
```C#
string GetFileHashFor(string filePath)
```
This might as well be a static method. The way this works is as follows:

1. Define `result` as an empty string
1. Get the file as a `File`
1. Make sure it exists
1. Try-catch: (not sure why this is necessary, I'd prefer it crashes rather than break silently)
   1. Get an MD5 hasher
   1. Get a byte stream based on the file's bytes
   1. Feed this into the MD5 hasher
   1. Get the output bytes of the MD5 hasher
   1. For every byte in the output bytes
      - Append the string value of the byte to the `result`
2. Return `result`

Honestly, it's pretty weird as commonly you'd have an md5 hex string. What C2O does accomplishes the same purpose, however.

Sidenote: md5 is known to have collisions so maybe using this algorithm for anti-cheat wouldn't be recommended nowadays.

```C#
bool ValidateFile(string filePath, string fileHash)
```

This is pretty straightforward.
1. Define `sourceFileHash` as the result of GetFileHashFor `filePath`
2. Return its equality to `fileHash`

```C#
void GetLatestCarCrushFiles()
```

```C#
void GetLatestCarDefinitions()
```

```C#
void GetLatestGameDefinitions()
```

```C#
void GetLatestTrackDefinitions()
```