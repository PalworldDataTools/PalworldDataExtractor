using CUE4Parse.FileProvider;
using CUE4Parse.FileProvider.Vfs;
using CUE4Parse.MappingsProvider;
using CUE4Parse.UE4.VirtualFileSystem;
using PalworldDataExtractor.Extractors;
using PalworldDataExtractor.Models;
using PalworldDataExtractor.Models.Pals;

namespace PalworldDataExtractor;

public class DataExtractor : IDisposable
{
    readonly DefaultFileProvider _provider;

    public DataExtractor(string pakFileDirectory, Action<DataExtractorConfiguration>? configure = null)
    {
        DataExtractorConfiguration configuration = new();
        configure?.Invoke(configuration);

        _provider = Create(pakFileDirectory, configuration);
    }

    public async Task<ExtractedData> Extract()
    {
        IEnumerable<Pal> pals = await new PalsExtractor(_provider).ExtractPalsAsync();
        PalTribe[] tribes = pals.GroupBy(p => p.TribeName).Select(g => new PalTribe { Name = g.Key ?? "???", Pals = g.ToArray() }).ToArray();

        IReadOnlyDictionary<string, byte[]> palIcons = await new PalIconsExtractor(_provider).ExtractPalsAsync();

        return new ExtractedData { Tribes = tribes, TribeIcons = palIcons };
    }

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
