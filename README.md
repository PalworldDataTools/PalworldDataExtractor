# Palworld data extractor

This tool extracts data from the Palworld .pak file. Use either the `PalworldDataExtractor` library for a programmatic interface or the `PalworldDataExtractor.exe` CLI.

## Library

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