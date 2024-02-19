using CUE4Parse.UE4.Versions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PalworldDataExtractor;
using PalworldDataExtractor.Abstractions;
using Tests.Tools;

namespace Tests;

[TestClass]
public class DataExtractorTest
{
    const string PalDir = @"E:\SteamLibrary\steamapps\common\Palworld";
    const string PalPakFolder = @"Pal\Content\Paks";

    static string PalPakPath => Path.Combine(PalDir, PalPakFolder);

    [TestMethod]
    [Ignore] // must be ignored because it requires access to the .pak file which is not there when running in the CI 
    public async Task ShouldExtractData()
    {
        DataExtractor extractor = new(
            PalPakPath,
            config =>
            {
                config.UnrealEngineVersion = new VersionContainer(EGame.GAME_UE5_1);
                config.MappingsFilePath = "../../../TestFiles/palworld-mappings-13312439.usmap";
            }
        );

        ExtractedData data = await extractor.Extract();

        data.DumpCompare(
            "TestFiles/data.expected",
#if DEBUG
            true
#else
            false
#endif
        );
    }
}
