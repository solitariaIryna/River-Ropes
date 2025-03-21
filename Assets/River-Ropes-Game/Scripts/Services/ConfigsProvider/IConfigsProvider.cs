using UnityEngine.AddressableAssets;

namespace RiverRopes.Services.ConfigsProvider
{
    public interface IConfigsProvider
    {
        AssetReference GetLevel(int number);
        void LoadAll();
    }
}