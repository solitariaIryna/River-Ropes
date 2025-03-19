using Cysharp.Threading.Tasks;
using RiverRopes.Infrastructure.StateMachine;

namespace RiverRopes.Infrastructure.Application.StatesMachine
{
    public class LoadingSceneApplicationState : IPayloadState<string>
    {
        private ApplicationStateMachine _applicationStateMachine;
        private SceneLoader _sceneLoader;

        public LoadingSceneApplicationState(ApplicationStateMachine applicationStateMachine,
            SceneLoader sceneLoader)
        {
            _applicationStateMachine = applicationStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter(string sceneName)
        {
            LoadScene(sceneName);
        }

        public void Exit()
        {

        }

        private async UniTask LoadScene(string sceneName)
        {
            await _sceneLoader.Load(sceneName);

            _applicationStateMachine.Enter<GameApplicationState>();
        }
    }

}


