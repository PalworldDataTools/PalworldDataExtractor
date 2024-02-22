using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PalworldDataExtractor.Abstractions;
using PalworldDataExtractor.Abstractions.Breeding;
using PalworldDataExtractor.Abstractions.L10N;
using PalworldDataExtractor.Abstractions.Pals;
using PalworldDataExtractor.Abstractions.Steam;
using Tests.Tools;

namespace Tests.UnitTests;

[TestClass]
public class ExtractedDataTest
{
    ExtractedData _extractedData = null!;

    [TestInitialize]
    public void Initialize() =>
        _extractedData = new ExtractedData
        {
            SteamManifest = new SteamManifest
            {
                AppId = "APP_ID",
                BuildId = "BUILD_ID",
                AppName = "APP_NAME",
                AppSize = 987654321,
                UpdateDate = new DateOnly(123, 4, 5)
            },
            Tribes = new[]
            {
                new PalTribe
                {
                    Name = "TRIBE",
                    Pals = new[]
                    {
                        new Pal
                        {
                            GameIndex = -1,
                            TribeName = "PAL_TRIBE",
                            Name = "PAL_NAME",
                            ZukanIndex = 0,
                            ZukanIndexSuffix = "ZUKAN_SUFFIX",
                            Rarity = 1,
                            Size = "PAL_SIZE",
                            ElementType1 = "PAL_ELEMENT1",
                            ElementType2 = "PAL_ElEMENT2",
                            Price = 2,
                            IsNocturnal = true,
                            IsEdible = true,
                            IsBoss = true,
                            IsTowerBoss = true,
                            IsPredator = true,
                            CraftSpeed = 3,
                            CaptureRate = 4,
                            ExpRatio = 5,
                            SlowWalkSpeed = 6,
                            WalkSpeed = 7,
                            RunSpeed = 8,
                            RideSprintSpeed = 9,
                            TransportSpeed = 10,
                            MaxFullStomach = 11,
                            FullStomachDecreaseRate = 12,
                            FoodAmount = 13,
                            Stamina = 14,
                            ViewingDistance = 15,
                            ViewingAngle = 16,
                            HearingRate = 17,
                            Hp = 18,
                            MeleeAttack = 19,
                            ShotAttack = 20,
                            Defense = 21,
                            Support = 22,
                            EnemyReceiveDamageRate = 23,
                            MaleProbability = 24,
                            CombiRank = 25,
                            EmitFlame = 26,
                            Watering = 27,
                            Seeding = 28,
                            GenerateElectricity = 29,
                            Handcraft = 30,
                            Collection = 31,
                            Deforest = 32,
                            Mining = 33,
                            OilExtraction = 34,
                            ProduceMedicine = 35,
                            Cool = 36,
                            Transport = 37,
                            MonsterFarm = 38
                        }
                    }
                }
            },
            TribeIcons = new Dictionary<string, byte[]>
            {
                { "tribe1", [0x1, 0x2, 0x3] }
            },
            UniqueBreedingCombinations = new[]
            {
                new PalBreedingCombination("tribe1", "tribe2", "child")
            },
            LocalizationFiles = new Dictionary<string, LocalizationFile>
            {
                {
                    "language1",
                    new LocalizationFile
                    {
                        Language = "language1",
                        Namespaces = new Dictionary<string, LocalizationNamespace>
                        {
                            {
                                "namespace1",
                                new LocalizationNamespace
                                {
                                    Namespace = "namespace1",
                                    Fields = new Dictionary<string, string>
                                    {
                                        { "field1", "value1 language1" },
                                        { "field2", "value2 language1" }
                                    }
                                }
                            },
                            {
                                "namespace2",
                                new LocalizationNamespace
                                {
                                    Namespace = "namespace2",
                                    Fields = new Dictionary<string, string>
                                    {
                                        { "field3", "value3 language1" },
                                        { "field4", "value4 language1" }
                                    }
                                }
                            }
                        }
                    }
                },
                {
                    "language2",
                    new LocalizationFile
                    {
                        Language = "language2",
                        Namespaces = new Dictionary<string, LocalizationNamespace>
                        {
                            {
                                "namespace1",
                                new LocalizationNamespace
                                {
                                    Namespace = "namespace1",
                                    Fields = new Dictionary<string, string>
                                    {
                                        { "field1", "value1 language2" },
                                        { "field2", "value2 language2" }
                                    }
                                }
                            },
                            {
                                "namespace2",
                                new LocalizationNamespace
                                {
                                    Namespace = "namespace2",
                                    Fields = new Dictionary<string, string>
                                    {
                                        { "field3", "value3 language2" },
                                        { "field4", "value4 language2" }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

    [TestMethod]
    public async Task ShouldSerialize()
    {
        await _extractedData.DumpStreamAndCompare(
            ExtractedData.Serialize,
            "extracted_data.expected",
#if DEBUG
            true
#else
            false
#endif
        );
    }

    [TestMethod]
    public async Task ShouldDeserialize()
    {
        ExtractedData? deserializedData;
        await using (FileStream stream = File.OpenRead(TestFiles.GetTestFilePath("extracted_data.expected")))
        {
            deserializedData = await ExtractedData.Deserialize(stream);
        }

        deserializedData.Should().BeEquivalentTo(_extractedData);
    }
}
