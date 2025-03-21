using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RiverRopes.Configs.Levels
{
    [CreateAssetMenu(fileName = nameof(LevelsCollection), menuName = "Configs/Gameplay/Levels/" + nameof(LevelsCollection))]
    public class LevelsCollection : ScriptableObject
    {
        [SerializeField] private AssetReference[] _levels;

        public AssetReference this[int number]
        {
            get
            {
                if (_levels.Length <= number)
                    return _levels[number - 1];

                new IndexOutOfRangeException($"Does not exist level number {number} ");
                return _levels[0];
            }
        }
    }
}
