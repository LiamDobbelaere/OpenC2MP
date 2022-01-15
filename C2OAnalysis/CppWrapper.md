# CppWrapper

The CppWrapper is responsible for interfacing with the CppWrapper.dll from managed code. Most of these speak for themselves honestly. The read/write functions take a memory address I assume.

```C#
public static extern long FindWindow(String paramString);
```

```C#
public static extern long GetOwnCarPointer();
```

```C#
public static extern long GetOpp1CarPointer();
```

```C#
public static extern long GetOpp2CarPointer();
```

```C#
public static extern long GetOpp3CarPointer();
```

```C#
public static extern float ReadFloat(long paramLong);
```

```C#
public static extern void WriteFloat(float paramFloat, long paramLong);
```

```C#
public static extern long ReadLong(long paramLong);
```

```C#
public static extern void WriteLong(long paramLong1, long paramLong2);
```

```C#
public static extern void WriteDouble(double paramDouble, long paramLong);
```

```C#
public static extern double ReadDouble(long paramLong);
```

```C#
public static extern boolean PressedPlus();
```

```C#
public static extern boolean PressedMinus();
```

```C#
public static extern boolean PressedSlash();
```

```C#
public static extern boolean PressedEnter();
```

```C#
public static extern boolean PressedDelete();
```

```C#
public static extern boolean PressedZero();
```

```C#
public static extern void EnableOpponentAi();
```

```C#
public static extern void DisableOpponentAi();
```

```C#
public static extern void EnableMinimap();
```

```C#
public static extern void DisableMinimap();
```

```C#
public static extern void DisablePeds();
```

```C#
public static extern void EnablePeds();
```

```C#
public static extern boolean PressedPrintScreen();
```

```C#
public static extern void EnableSelfWastage();
```

```C#
public static extern void DisableSelfWastage();
```

```C#
public static extern void ForceOpponentDamagedModels();
```

```C#
public static extern void LockOwnDriveshaft();
```

```C#
public static extern void UnlockOwnDriveshaft();
```

```C#
public static extern void DisableCrushage();
```

```C#
public static extern void DisableEngineSmoke();
```

```C#
public static extern void EnableEngineSmoke();
```
