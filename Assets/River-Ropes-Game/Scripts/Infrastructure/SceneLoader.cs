using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RiverRopes.Infrastructure
{
    public class SceneLoader
    {
        public async UniTask Load(string name, Action onLoaded = null, bool canReloadScene = false)
        {
            if (SceneManager.GetActiveScene().name == name && !canReloadScene)
            {
                onLoaded?.Invoke();
                return;
            }

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);
            await UniTask.WaitUntil(() => asyncOperation.isDone);

            onLoaded?.Invoke();
        }
    }
}
    
