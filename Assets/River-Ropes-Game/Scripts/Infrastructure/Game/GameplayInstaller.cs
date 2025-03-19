using RiverRopes.Infrastructure.Gameplay.StatesMachine;
using Zenject;

namespace RiverRopes.Infrastructure.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
               .Bind<GameplayStatesFactory>()
               .AsSingle();

            Container
                .BindInterfacesAndSelfTo<GameplayStateMachine>()
                .AsSingle();

        }
    }
}
