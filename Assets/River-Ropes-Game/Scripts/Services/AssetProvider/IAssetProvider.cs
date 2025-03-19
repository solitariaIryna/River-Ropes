using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RiverRopes.Services.AssetProvider
{
    public interface IAssetProvider
    {
        TAsset Load<TAsset>(string path) where TAsset : Object;
        TAsset[] LoadAll<TAsset>(string path) where TAsset : Object;
        UniTask<TAsset> LoadAsync<TAsset>(string path) where TAsset : Object;
        UniTask<TAsset[]> LoadAllAsync<TAsset>(string path) where TAsset : Object;
    }
}