using Unity.Cinemachine;
using UnityEngine;

namespace RiverRopes.Services.Cameras
{
    [CreateAssetMenu(fileName = nameof(CameraStorage), menuName = "Configs/Gameplay/Cameras/" + nameof(CameraStorage))]
    public class CameraStorage : ScriptableObject
    {
        [field: SerializeField] public GenericDictionary<CameraType, CinemachineCamera> Cameras { get; private set; }

    }
}
