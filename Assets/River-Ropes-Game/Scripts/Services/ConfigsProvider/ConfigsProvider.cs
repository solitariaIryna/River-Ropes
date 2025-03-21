using RiverRopes.Configs.Levels;
using RiverRopes.Services.AssetProvider;
using UnityEngine.AddressableAssets;

namespace RiverRopes.Services.ConfigsProvider
{
    public class ConfigsProvider : IConfigsProvider
    {
        private readonly IAssetProvider _assetProvider;

        private LevelsCollection _levelsCollection;
        public ConfigsProvider(IAssetProvider assetProvider) => 
            _assetProvider = assetProvider;
        public void LoadAll() =>
            LoadLevels();

        private void LoadLevels() =>
            _levelsCollection = _assetProvider.Load<LevelsCollection>("Configs/Gameplay/Levels/LevelsCollection");
        public AssetReference GetLevel(int number) =>
            _levelsCollection[number];
    }
}
