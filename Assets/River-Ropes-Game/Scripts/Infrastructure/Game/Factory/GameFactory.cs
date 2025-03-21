using Cysharp.Threading.Tasks;
using RiverRopes.Gameplay.Entities;
using RiverRopes.Services.AssetProvider;
using UnityEngine;

namespace RiverRopes.Infrastructure.Game.Factory
{
    public class GameFactory
    {
        private readonly IAssetProvider _assetProvider;

        public GameFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }
        public async UniTask<Hero> CreateHero(Vector3 position, Quaternion rotation)
        {
            Hero hero = await _assetProvider.InstantiateAsync<Hero>("Hero", position, rotation);
            return hero;
        }
    }
}
