using Zenject;

namespace RiverRopes.Infrastructure
{
    public class GameRunner : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<GameInitializer>()
                .AsSingle();
        }
    }
}
