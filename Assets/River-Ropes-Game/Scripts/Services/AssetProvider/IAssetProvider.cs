using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace RiverRopes.Services.AssetProvider
{
    public interface IAssetProvider
    {
        TAsset Load<TAsset>(string path) where TAsset : Object;
        TAsset[] LoadAll<TAsset>(string path) where TAsset : Object;
        UniTask<TAsset> LoadAsync<TAsset>(string path) where TAsset : Object;
        UniTask<List<TAsset>> LoadAllAsync<TAsset>(string path) where TAsset : Object;
        void Release<T>(T asset) where T : Object;
        UniTask<T> LoadAsync<T>(AssetReference reference) where T : Object;
        UniTask<T> InstantiateAsync<T>(string key, Vector3 position = default, Quaternion rotation = default, Transform parent = null) where T : MonoBehaviour;
        UniTask<T> InstantiateAsync<T>(AssetReference reference, Vector3 position = default, Quaternion rotation = default, Transform parent = null) where T : MonoBehaviour;
        UniTask<T> InstantiateAsync<T>(string key, DiContainer container, Vector3 position = default, Quaternion rotation = default, Transform parent = null) where T : MonoBehaviour;
        UniTask<T> InstantiateAsync<T>(AssetReference reference, DiContainer container, Vector3 position = default, Quaternion rotation = default, Transform parent = null) where T : MonoBehaviour;
        T Instantiate<T>(T prefab, Vector3 position = default, Quaternion rotation = default, Transform parent = null) where T : Object;
    }
}