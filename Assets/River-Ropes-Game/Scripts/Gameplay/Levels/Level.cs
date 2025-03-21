using RiverRopes.Gameplay.Entities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RiverRopes.Gameplay.Levels
{
    public class Level : MonoBehaviour
    {
        [field: SerializeField] public HeroPathWay RiverWay { get; private set; }

        [SerializeField] private Transform _enemiesContainer;
        [SerializeField] private List<Enemy> _enemies;
        [field: SerializeField] public Transform SpawnPoint { get; private set; }


        public void Construct()
        {
            if (_enemiesContainer.childCount > 0)
                _enemies = _enemiesContainer.GetComponentsInChildren<Enemy>().ToList();
        }

    }
}
