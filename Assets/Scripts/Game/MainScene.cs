using System.Collections;
using Attributes;
using Game.Windows;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class MainScene : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _infoButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private WindowSystem _windowSystem;
        [SceneListPopupField] [SerializeField] private string _infoSceneName = string.Empty;
        [SceneListPopupField] [SerializeField] private string _gameSceneName = string.Empty;
        
        [CanBeNull]
        private Coroutine _sceneCoroutine;
        
        private void OnEnable()
        {
            _playButton.onClick.AddListener(OnPlayButtonClicked);
            _infoButton.onClick.AddListener(OnInfoButtonClicked);
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnDisable()
        {
            _playButton.onClick.RemoveListener(OnPlayButtonClicked);
            _infoButton.onClick.RemoveListener(OnInfoButtonClicked);
            _exitButton.onClick.RemoveListener(OnExitButtonClicked);
        }
        
        private void OnPlayButtonClicked()
        {
            if (_sceneCoroutine != null)
                StopCoroutine(_sceneCoroutine);

            _sceneCoroutine = StartCoroutine(LoadScene(_gameSceneName));
        }

        private void OnInfoButtonClicked()
        {
            if (_sceneCoroutine != null)
                StopCoroutine(_sceneCoroutine);

            _sceneCoroutine = StartCoroutine(LoadScene(_infoSceneName));
        }
        
        private void OnExitButtonClicked()
        {
            var setup = new ExitWindowSetup
            {
                ExitCallback = () =>
                {
                    StopAllCoroutines();
                    Application.Quit();
                },
                WindowSystem = _windowSystem
            };
            _windowSystem.Open<ExitWindow, ExitWindowSetup>(setup);
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
