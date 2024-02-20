# Palworld data extractor

This tool extracts data from the Palworld .pak file.
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