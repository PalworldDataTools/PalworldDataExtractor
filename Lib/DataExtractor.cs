using CUE4Parse.FileProvider;
using CUE4Parse.FileProvider.Vfs;
using CUE4Parse.MappingsProvider;
using CUE4Parse.UE4.VirtualFileSystem;
using PalworldDataExtractor.Abstractions;
using PalworldDataExtractor.Abstractions.Breeding;
using PalworldDataExtractor.Abstractions.L10N;
using PalworldDataExtractor.Abstractions.Pals;
using PalworldDataExtractor.Abstractions.Steam;
using PalworldDataExtractor.Extractors;

namespace PalworldDataExtractor;

public class DataExtractor : IDisposable
{
    readonly string _pakFileDirectory;
    readonly DefaultFileProvider _provider;

    public DataExtractor(string pakFileDirectory, Action<DataExtractorConfiguration>? configure = null)
    {
        _pakFileDirectory = pakFileDirectory;
        DataExtractorConfiguration configuration = new();
        configure?.Invoke(configuration);

        _provider = Create(pakFileDirectory, configuration);
    }

    public async Task<ExtractedData> Extract()
    {
        SteamManifest? steamManifest = await ExtractSteamManifestAsync();
        PalTribe[] tribes = await ExtractTribesAsync();
        IReadOnlyDictionary<string, byte[]> palIcons = await ExtractPalIconsAsync();
        PalBreedingCombination[] uniqueBreedingCombinations = await ExtractUniqueBreedingCombinationsAsync();
        Dictionary<string, LocalizationFile> localizationFiles = await ExtractLocalizationFilesAsync();

        return new ExtractedData
        {
            SteamManifest = steamManifest, Tribes = tribes, TribeIcons = palIcons, UniqueBreedingCombinations = uniqueBreedingCombinations, LocalizationFiles = localizationFiles
        };
    }

    async Task<SteamManifest?> ExtractSteamManifestAsync()
    {
        string absoluteDir = Path.GetFullPath(_pakFileDirectory);
        int indexOfSteamApps = absoluteDir.IndexOf("steamapps", StringComparison.InvariantCultureIgnoreCase);
        if (indexOfSteamApps < 0)
        {
            return null;
        }

        string steamAppsDirectory = absoluteDir[..(indexOfSteamApps + 9)];
        SteamManifestExtractor extractor = new(steamAppsDirectory);

        return await extractor.Extract();
    }

    async Task<PalTribe[]> ExtractTribesAsync()
    {
        IEnumerable<Pal> pals = await new PalsExtractor(_provider).ExtractPalsAsync();
        PalTribe[] tribes = pals.GroupBy(p => p.TribeName).Select(g => new PalTribe { Name = g.Key ?? "???", Pals = g.ToArray() }).ToArray();
        return tribes;
    }

    async Task<IReadOnlyDictionary<string, byte[]>> ExtractPalIconsAsync()
    {
        IReadOnlyDictionary<string, byte[]> palIcons = await new PalIconsExtractor(_provider).ExtractIconsAsync();
        return palIcons;
    }

    async Task<PalBreedingCombination[]> ExtractUniqueBreedingCombinationsAsync() =>
        (await new UniquePalBreedingCombinationExtractor(_provider).ExtractUniquePalBreedingCombinationsAsync()).ToArray();

    async Task<Dictionary<string, LocalizationFile>> ExtractLocalizationFilesAsync() => await new LocalizationFilesExtractor(_provider).ExtractLocalizationFilesAsync();

    public void Dispose()
    {
        _provider.Dispose();

        GC.SuppressFinalize(this);
    }

    static DefaultFileProvider Create(string pakFileDirectory, DataExtractorConfiguration configuration)
    {
        DefaultFileProvider provider = new(pakFileDirectory, SearchOption.AllDirectories, false, configuration.UnrealEngineVersion);

        if (configuration.MappingsFilePath != null)
        {
            provider.MappingsContainer = new FileUsmapTypeMappingsProvider(configuration.MappingsFilePath);
        }

        provider.Initialize();

        IAesVfsReader? file = provider.UnloadedVfs.FirstOrDefault(f => f.Name.Contains(configuration.PakFileName));
        if (file == null)
        {
            throw new InvalidOperationException($"Could not find pak file {configuration.PakFileName} in directory {pakFileDirectory}");
        }

        file.MountTo((FileProviderDictionary)provider.Files, false);

        return provider;
    }
}
