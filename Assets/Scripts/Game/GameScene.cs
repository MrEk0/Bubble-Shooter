using System.Collections;
using Attributes;
using Game.Windows;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameScene : MonoBehaviour
    {
        [SerializeField] private GameWindow _gameWindow;
        [SceneListPopupField] [SerializeField] private string _mainMenuSceneName = string.Empty;
        
        [CanBeNull]
        private Coroutine _sceneCoroutine;
        
        private void OnEnable()
        {
            _gameWindow.OnExitClickEvent += OnExitButtonClicked;
        }

        private void OnDisable()
        {
            _gameWindow.OnExitClickEvent += OnExitButtonClicked;
        }
        
        private void OnExitButtonClicked()
        {
            if (_sceneCoroutine != null)
                StopCoroutine(_sceneCoroutine);

            _sceneCoroutine = StartCoroutine(LoadScene(_mainMenuSceneName));
        }

        private static IEnumerator LoadScene(string sceneName)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            if (asyncOperation == null)
            {
                yield break;
            }

            asyncOperation.allowSceneActivation = true;

            while (!asyncOperation.isDone)
            {
                yield return null;
            }
        }
    }
}
