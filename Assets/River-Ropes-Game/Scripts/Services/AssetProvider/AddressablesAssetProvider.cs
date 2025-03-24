using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;
using Object = UnityEngine.Object;

namespace RiverRopes.Services.AssetProvider
{
    public class AddressablesAssetProvider : IAssetProvider
    {
        public T Load<T>(string path) where T : Object =>
            Resources.Load<T>(path);

        public T Instantiate<T>(T prefab, Vector3 position = default, Quaternion rotation = default, Transform parent = null) where T : Object =>
            Object.Instantiate<T>(prefab, position, rotation, parent);
        public async UniTask<T> InstantiateAsync<T>(string key, Vector3 position = default, Quaternion rotation = default, Transform parent = null) where T : MonoBehaviour
        {
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(key, position, rotation, parent);
            await handle.ToUniTask();

            if (handle.Status == AsyncOperationStatus.Succeeded)
                return handle.Result.GetComponent<T>();

            Debug.LogError($"Failed to instantiate object with key: {key}");
            return null;
        }
        public async UniTask<T> InstantiateAsync<T>(string key, DiContainer container, Vector3 position = default,
            Quaternion rotation = default, Transform parent = null) where T : MonoBehaviour
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);
            await handle.ToUniTask();

            if (handle.Status == AsyncOperationStatus.Succeeded)
                return container.InstantiatePrefabForComponent<T>(handle.Result, position, rotation, parent);

            Debug.LogError($"Failed to instantiate object with key: {key}");
            return null;
        }
        public async UniTask<T> InstantiateAsync<T>(AssetReference reference, Vector3 position = default, Quaternion rotation = default, Transform parent = null) where T : MonoBehaviour
        {
            AsyncOperationHandle<GameObject> handle = reference.InstantiateAsync(position, rotation, parent);
            await handle.ToUniTask();

            if (handle.Status == AsyncOperationStatus.Succeeded)
                return handle.Result.GetComponent<T>();

            Debug.LogError($"Failed to instantiate object with reference: {reference}");
            return null;
        }

        public async UniTask<T> InstantiateAsync<T>(AssetReference reference, DiContainer container, Vector3 position = default,
          Quaternion rotation = default, Transform parent = null) where T : MonoBehaviour
        {
            AsyncOperationHandle<GameObject> handle = reference.LoadAssetAsync<GameObject>();
            await handle.ToUniTask();

            if (handle.Status == AsyncOperationStatus.Succeeded)
                return container.InstantiatePrefabForComponent<T>(handle.Result, position, rotation, parent);

            Debug.LogError($"Failed to instantiate object with key: {reference}");
            return null;
        }
        public async UniTask<T> LoadAsync<T>(string key) where T : Object
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);
            await handle.ToUniTask();

            if (handle.Status == AsyncOperationStatus.Succeeded)
                return handle.Result;

            Debug.LogError($"Failed to load asset with key: {key}");
            return null;
        }
        public async UniTask<T> LoadAsync<T>(AssetReference reference) where T : Object
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(reference);
            await handle.ToUniTask();

            if (handle.Status == AsyncOperationStatus.Succeeded)
                return handle.Result;

            Debug.LogError($"Failed to load asset with key: {reference}");
            return null;
        }
        public async UniTask<List<T>> LoadAllAsync<T>(string key) where T : Object
        {
            AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(key, null);
            await handle.ToUniTask();

            if (handle.Status == AsyncOperationStatus.Succeeded)
                return handle.Result.ToList();

            Debug.LogError($"Failed to load assets with key: {key}");
            return null;
        }

        public void Release<T>(T asset) where T : Object => 
            Addressables.Release(asset);

        public TAsset[] LoadAll<TAsset>(string path) where TAsset : Object =>
            Resources.LoadAll<TAsset>(path);
    }
}
