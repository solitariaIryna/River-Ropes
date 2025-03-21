using RiverRopes.Gameplay.Entities;
using RiverRopes.Gameplay.Levels;
using RiverRopes.Infrastructure.Game.Factory;
using RiverRopes.Infrastructure.StateMachine;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

namespace RiverRopes.Infrastructure.Gameplay.StatesMachine
{
    public class LoadLevelState : IState
    {
        private readonly LevelsFactory _levelsFactory;
        private readonly GameFactory _gameFactory;

        public LoadLevelState(LevelsFactory levelsFactory, GameFactory gameFactory)
        {
            _levelsFactory = levelsFactory;
            _gameFactory = gameFactory;
        }

        public void Enter()
        {
            InitLevelAsync();

        }

        private async Task InitLevelAsync()
        {
            Level level = await _levelsFactory.CreateLevel(1);

            Hero hero = await _gameFactory.CreateHero(level.SpawnPoint.position, level.SpawnPoint.rotation);

            CinemachineCamera camera = GameObject.FindAnyObjectByType<CinemachineCamera>();
            camera.Target.TrackingTarget = hero.transform;

            hero.SetMovePath(level.RiverWay);
            hero.Initialize();

        }
        public void Exit()
        {
        }
    }        
}
