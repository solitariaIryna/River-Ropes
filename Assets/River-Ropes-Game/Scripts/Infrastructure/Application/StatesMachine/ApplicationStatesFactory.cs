using RiverRopes.Infrastructure.StateMachine;
using System;
using System.Collections.Generic;
using Zenject;

namespace RiverRopes.Infrastructure.Application.StatesMachine
{
    public class ApplicationStatesFactory
    {
        private readonly DiContainer _container;

        public ApplicationStatesFactory(DiContainer container)
        {
            _container = container;
        }

        public Dictionary<Type, IExitableState> CreateStates() => new Dictionary<Type, IExitableState>
        {
            { typeof(StartupApplicationState), _container.Instantiate<StartupApplicationState>() },
            { typeof(LoadingSceneApplicationState), _container.Instantiate<LoadingSceneApplicationState>() },
            { typeof(GameApplicationState), _container.Instantiate<GameApplicationState>() }
        };
    }

}
