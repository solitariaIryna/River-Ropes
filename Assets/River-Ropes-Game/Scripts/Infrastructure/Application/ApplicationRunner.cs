using Zenject;

namespace RiverRopes.Infrastructure.Application
{
    public class ApplicationRunner : MonoInstaller
    {
        public override void InstallBindings()
        {           
            Container
                .BindInterfacesAndSelfTo<ApplicationInitializer>()
                .AsSingle();
        }
    }
}
