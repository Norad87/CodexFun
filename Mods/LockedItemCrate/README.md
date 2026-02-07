# Locked Item Crate

This modlet adds a steel-strength crate that locks to the first item type stored and auto-renames to that item.

## Build (DLL)
7 Days to Die does **not** compile `.cs` files placed in the `Mods` folder. The Harmony patch must be built into a DLL.

### Requirements
- .NET Framework build tools (MSBuild)
- A local 7 Days to Die install

### Build steps
From this folder, run MSBuild and point `GameDir` at your game install folder (the folder that contains `7DaysToDie.exe`).

```bash
msbuild LockedItemCrate.csproj /p:GameDir="C:\\Path\\To\\7DaysToDie"
```

This produces `bin/Debug/LockedItemCrate.dll` (or `bin/Release` if you build Release). Copy the DLL into this mod folder:

```
Mods/
  LockedItemCrate/
    LockedItemCrate.dll
    Config/
    Scripts/
```

Once the DLL is present, launch the game/server with the modlet in the `Mods` folder.
