using RiverRopes.Configs.Levels;
using RiverRopes.Services.AssetProvider;
using RiverRopes.Services.Cameras;
using UnityEngine.AddressableAssets;

namespace RiverRopes.Services.ConfigsProvider
{
    public class ConfigsProvider : IConfigsProvider
    {
        private readonly IAssetProvider _assetProvider;

        private LevelsCollection _levelsCollection;
        public CameraStorage Cameras { get; private set; }
        public ConfigsProvider(IAssetProvider assetProvider) => 
            _assetProvider = assetProvider;
        public void LoadAll()
        {
            LoadCameras();
            LoadLevels();
        }

        private void LoadLevels() =>
            _levelsCollection = _assetProvider.Load<LevelsCollection>("Configs/Gameplay/Levels/LevelsCollection");

        private void LoadCameras() =>
            Cameras = _assetProvider.Load<CameraStorage>("Configs/Gameplay/Cameras/CameraStorage");
        public AssetReference GetLevel(int number) =>
            _levelsCollection[number];
    }
}
