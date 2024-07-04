using System;
using System.Collections.Generic;

namespace Util.SceneNavigation
{
    public class SceneNavigator : MonoSingleton<SceneNavigator>
    {
        private readonly Dictionary<SceneType, Action> _sceneChangeAction = new ();
        private SceneType _currentScene;

        public void RegisterToSceneChange(Action callback, SceneType sceneType)
        {
            _sceneChangeAction.TryAdd(sceneType, null);
            _sceneChangeAction[sceneType] += callback;
        }
        public void UnregisterFromSceneChange(Action callback, SceneType sceneType)
        {
            _sceneChangeAction[sceneType] -= callback;
        }

        public void ChangeScene(SceneType sceneType)
        {
            if (_currentScene == sceneType)
            {
                throw new Exception("Trying to change into current scene");
            }

            _currentScene = sceneType;
            _sceneChangeAction[sceneType]?.Invoke();
        }
    }
}
