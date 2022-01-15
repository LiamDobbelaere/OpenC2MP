# Main

It speaks for itself that this is one of the biggest files, so there's a lot to unpack here. But let's start where it all begins. Aside from having references to nearly every corner of the code base, there's also a ServerSetupThread being fired in the constructor. So, let's start there.

## ServerSetupThread

### Constructor

1. Log "## welcome to the C2O-Server: Version " + this$0.GetVersionNumber()
2. Try-catch:
   1. Read out C2O_CONFIG.txt
      1. If line starts with "NICK_NAME":
         1.  Set nickname to the thing after the space
      2. If line starts with "USING_MOD":
         1.  Set using mod to the boolean value of the thing after the space
      3. If line starts with "MOD_NAME":
         1.  Set mod name to the thing after the space
      4. If line starts with "GAME_PATH":
         1.  Set game path to the thing after the space
      5. If line starts with "GAME_VERSION":
         1.  Set game version to the thing after the space
      6. If line starts with "GLIDE_WRAPPER_PATH":
         1.  Set glide wrapper path to the thing after the space
      7. If line starts with "MASTER_SERVER_ADDRESS":
         1.  Set master server address to the thing after the space
      8. If line starts with "RECOVERY_KEY":
         1.  Set recovery key to the parsed integer of the thing after the space
   2.  If a nickname and gamepath is set:
       1.  If gamePath + "DATA/TEMP_OPPONENT.txt" does not exist, run `PerformFirstTimeSetup` (which just starts the PerformFirstTimeSetupThread)
   3.  Else:
       1.  Log "## unable to load c2o configuration file: check C2O_CONFIG.TXT"
   4.  Start the this thread with priority 5
3. On error:
   1. Log "## unable to load c2o configuration file: check C2O_CONFIG.TXT" 

## PerformFirstTimeSetupThread

### Constructor
1. Start itself with priority 5

