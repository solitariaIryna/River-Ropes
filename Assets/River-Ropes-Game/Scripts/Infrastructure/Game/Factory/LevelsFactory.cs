using Cysharp.Threading.Tasks;
using RiverRopes.Gameplay.Levels;
using RiverRopes.Services.AssetProvider;
using RiverRopes.Services.ConfigsProvider;
using UnityEngine.AddressableAssets;
using Zenject;

namespace RiverRopes.Infrastructure.Game.Factory
{
    public class LevelsFactory
    {
        private readonly DiContainer _container;
        private readonly IAssetProvider _assetProvider;
        private readonly IConfigsProvider _configsProvider;

        public LevelsFactory(DiContainer container, IAssetProvider assetProvider, IConfigsProvider configsProvider)
        {
            _container = container;
            _assetProvider = assetProvider;
            _configsProvider = configsProvider;
        }

        public async UniTask<Level> CreateLevel(int number)
        {
            AssetReference levelReference = _configsProvider.GetLevel(number);
            Level level = await _assetProvider.InstantiateAsync<Level>(levelReference, _container);
            level.Construct();
            return level;

        }
    }
}
