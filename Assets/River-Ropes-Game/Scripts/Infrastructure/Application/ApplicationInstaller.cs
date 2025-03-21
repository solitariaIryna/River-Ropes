using RiverRopes.Infrastructure.Application.StatesMachine;
using RiverRopes.Services.AssetProvider;
using RiverRopes.Services.ConfigsProvider;
using Zenject;

namespace RiverRopes.Infrastructure.Application
{
    public class ApplicationInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
               .Bind<ApplicationStatesFactory>()
               .AsSingle();

            Container
                .BindInterfacesAndSelfTo<ApplicationStateMachine>()
                .AsSingle();

            Container
                .Bind<IAssetProvider>()
                .To<AddressablesAssetProvider>()
                .AsSingle();

            Container
                .Bind<IConfigsProvider>()
                .To<ConfigsProvider>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<SceneLoader>()
                .AsSingle();
        }
    }
}
