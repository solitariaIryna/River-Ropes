using RiverRopes.Gameplay.Entities;
using RiverRopes.Gameplay.Levels;
using RiverRopes.Infrastructure.Game.Factory;
using RiverRopes.Infrastructure.StateMachine;
using RiverRopes.Services.Cameras;
using System.Threading.Tasks;
using CameraType = RiverRopes.Services.Cameras.CameraType;

namespace RiverRopes.Infrastructure.Gameplay.StatesMachine
{
    public class LoadLevelState : IState
    {
        private readonly LevelsFactory _levelsFactory;
        private readonly GameFactory _gameFactory;
        private readonly CameraService _cameraService;

        public LoadLevelState(LevelsFactory levelsFactory, GameFactory gameFactory, CameraService cameraService)
        {
            _levelsFactory = levelsFactory;
            _gameFactory = gameFactory;
            _cameraService = cameraService;
        }

        public void Enter()
        {
            InitLevelAsync();

        }

        private async Task InitLevelAsync()
        {

            Level level = await _levelsFactory.CreateLevel(1);

            Hero hero = await _gameFactory.CreateHero(level.SpawnPoint.position, level.SpawnPoint.rotation);

            _cameraService.CreateCameras();
            _cameraService.SetFollowTarget(hero.transform);
            _cameraService.TurnOn(CameraType.Idle);
            hero.SetMovePath(level.RiverWay);
            hero.Initialize();

        }
        public void Exit()
        {
        }
    }        
}
