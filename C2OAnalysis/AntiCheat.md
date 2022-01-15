# AntiCheat

I picked this one first to test the analyze-document-implement cycle. It seems like a relatively straightforward file to begin with.

AntiCheat is instantiated in Main as a member, however in my opinion it may very well have been a static class instead. There's no reason for having an AntiCheat instance as it doesn't have any instance members. Interestingly, AntiCheat also contains a reference back to Main -> very tight coupling and thus an anti-pattern.

## Members
```C#
string GetFileHashFor(string filePath)
```
This might as well be a static function. The way this works is as follows:

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

1. Define a Vector `repairedCrushValues`
2. Define an integer `num` with value 0
3. Define an integer `numCarCrushFiles` with value 0
4. Logs "## downloading latest car crush files..." (honestly, this shouldn't be the responsibility of the AntiCheat class but of its caller)
5. Try-catch, but in a block that is in my opinion too broad:
   1. Get the master server address + "/C2O_DATA/CarCrushFileList.txt"
   2. Read this into a buffered input stream reader (essentially: just download the file)
   3. As long as there are lines to read, read a line and:
      1. Grab a file, namely the game path + "/C2O_DATA/CRUSH_FILES/" + current line being read from CarCrushFileList.txt
      2. If the file does not exist:
         1. Download master server address + "/C2O_DATA/CRUSH_FILES/" + current line being read from CarCrushFileList.txt
         2. Read the file line by line, writing the contents to the local car crush file (basically just copying the crush file from the server to the own game path + "/C2O_DATA/CRUSH_FILES/" directory), and for every line of the file:
            1. If the car is in the CarRecord and it's the first time we're doing this (only do this once, `num` == 0)
               1. Get the car record and set this car's NumCrushPointers to the integer in the file line
            2. Add a float conversion of the file line to repairedCrushValues
            3. Increment `num`
         3. Flush the file & close it
         4. If the car is in the CarRecord:
            1. `SetRepairedCrushValues` on the car to the `repairedCrushValues` and wipe `repairedCrushValues`
         5. Increment `numCarCrushFiles` and `continue`
      3. If the car is in the CarRecord:
         1. Read out the car crush file from the gamepath
         2. Set the crush pointers of the car to a parsed integer of the read line from said car crush file
         3. As long as there are lines left to read, add a float conversion of the line to `repairedCrushValues`
         4. `SetRepairedCrushValues` on the car to a clone of `repairedCrushValues`
   4. If `numCarCrushFiles` is greater than 0:
      1. Log "## " + `numCarCrushFiles` + " car crush files have been downloaded successfully
   5. Else:
      1. Log "## car crush files are already up to date"
   6. If it errors at any point, log "## unable to download car crush files: check internet connection", again this shouldn't be the responsibility of the AntiCheat class to log stuff

```C#
void GetLatestCarDefinitions()
```

```C#
void GetLatestGameDefinitions()
```

```C#
void GetLatestTrackDefinitions()
```