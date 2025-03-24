using RiverRopes.Services.Cameras;
using UnityEngine.AddressableAssets;

namespace RiverRopes.Services.ConfigsProvider
{
    public interface IConfigsProvider
    {
        CameraStorage Cameras { get; }

        AssetReference GetLevel(int number);
        void LoadAll();
    }
}