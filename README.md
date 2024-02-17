# Palworld data extractor

This tool extracts data from the Palworld .pak file. Use either the `PalworldDataExtractor` library for a programmatic interface or the `PalworldDataExtractor.exe` CLI.

## Library

The data is extracted from:
- The game's .pak file:
  - `Pal\Content\Pal\DataTable\Character\DT_PalMonsterParameter.uasset`: data about all the pals of the game
  - `Pal\Content\Pal\DataTable\Character\DT_PalCharacterIconDataTable.DT_PalCharacterIconDataTable.uasset`: mapping from pal tribe names to icon assets
- The content of steam's app manifest file if the `steamapps` directory is a parent of the .pak file directory:
  - `appid`: application id
  - `buildid`: build id
  - `name`: game name on steam
  - `SizeOnDisk`: application size

### Usage

```csharp
using CUE4Parse.UE4.Versions;
using PalworldDataExtractor;
using PalworldDataExtractor.Models;

DataExtractor extractor = new(
    @"Palworld\Pal\Content\Paks",
    config =>
    {
        config.UnrealEngineVersion = new VersionContainer(EGame.GAME_UE5_1);
        config.PakFileName = "Pal-Windows.pak";
    }
);

ExtractedData data = await extractor.Extract();
```

## CLI

The CLI exports all the data in the `ExtractedData` result of the extraction to the specified output directory. The layout of the extracted data is the following:
- _root_
  - `steam.json`: if the provided path for the .pak file is in the `steamapps` folder, the extraction will extract steam-related information about the game, and export it to this file.
    The content of the file looks like the following:
    ```
    {
      "AppId": "1623730",
      "BuildId": "13390747",
      "AppName": "Palworld",
      "AppSize": 20641809797
    }
    ```
  - `Enums`: contains the various enumerations that are used in the other files
    - `ElementType.json`: the possible elements of pals
    - `SizeType.json`: size of the pal, used to scale the model of the pal
  - `Pals`: contains all the pals of the game
    - `pals.json`: manifest containing all the pals. Each entry contains the following structure:
      ```
      {
        "Icon": "path/to/icon.png", 
        "Main": "path/to/main/pal/variant.json", 
        "Boss": "path/to/boss/pal/variant.json", 
        "Gym": "path/to/gym/pal/variant.json", 
        "OtherVariants": ["path/to/other/pal/variant1.json", "path/to/other/pal/variant2.json"], 
      }
      ```
      The `Main` field is always present, the `Icon` field might be absent if the icon could not be found in the .pak file, and the `Boss`, `Gym` and `OtherVariants` fields are absent when the variants don't exist.
      All the paths that are provided are relative to the manifest file, they point to the other subdirectories of the `Pals/` directory.
    - one directory per pal tribe: in each folder there is the icon representing the tribe and the data of all the existing variants (main, boss, gym, etc..)

### Usage

```
Palworld Data Extractor v0.1.0
Copyright (C) 2024 Ismail Bennani

USAGE:
minimal:
  PalworldDataExtractor.exe Palworld\Pal\Content\Paks

  --help          Display this help screen.
  -o, --out       (Default: Export) Output directory
  -p, --pak       (Default: Pal-Windows.pak) .pak file name
  -q, --quiet     (Default: false) Do not print anything else than errors
  --ue-version    (Default: 5.1) Version of UnrealEngine to use
  --usmap         (Default: mappings.usmap) .usmap mapping file to use
  --version       Display version information.
  dir (pos. 0)    Required. .pak file directory
```
