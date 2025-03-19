using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RiverRopes.Services.AssetProvider
{
    public class ResourcesAssetProvider : IAssetProvider
    {
        public T Load<T>(string path) where T : Object =>
            Resources.Load<T>(path);

        public TAsset[] LoadAll<TAsset>(string path) where TAsset : Object =>
            Resources.LoadAll<TAsset>(path);

        public async UniTask<TAsset> LoadAsync<TAsset>(string path) where TAsset : Object
        {
            ResourceRequest request = await LoadAsyncInternal<TAsset>(path);
            return request.asset as TAsset;
        }

        public async UniTask<TAsset[]> LoadAllAsync<TAsset>(string path) where TAsset : Object
        {
            ResourceRequest request = await LoadAsyncInternal<TAsset>(path);
            return request.asset as TAsset[];
        }

        private async UniTask<ResourceRequest> LoadAsyncInternal<T>(string key) where T : Object
        {
            ResourceRequest request = Resources.LoadAsync<T>(key);
            await request;
            return request;
        }
    }
}