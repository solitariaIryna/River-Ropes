using RiverRopes.Infrastructure.Game.Factory;
using RiverRopes.Infrastructure.Gameplay.StatesMachine;
using RiverRopes.Services.Cameras;
using RiverRopes.Services.SlowMotion;
using Zenject;

namespace RiverRopes.Infrastructure.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<SlowMotionService>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<CameraService>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<LevelsFactory>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<GameFactory>()
                .AsSingle();

            Container
               .Bind<GameplayStatesFactory>()
               .AsSingle();

            Container
                .BindInterfacesAndSelfTo<GameplayStateMachine>()
                .AsSingle();


        }
    }
}
