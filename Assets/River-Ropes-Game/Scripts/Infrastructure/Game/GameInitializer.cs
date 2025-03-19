using RiverRopes.Infrastructure.Gameplay.StatesMachine;
using Zenject;

namespace RiverRopes.Infrastructure
{
    public class GameInitializer : IInitializable
    {
        private readonly GameplayStateMachine _gameplayStateMachine;

        public GameInitializer(GameplayStateMachine gameplayStateMachine)
        {
            _gameplayStateMachine = gameplayStateMachine;
        }
        public void Initialize()
        {
            _gameplayStateMachine.Initialize();
            _gameplayStateMachine.Enter<LoadLevelState>();
        }
    }
}
