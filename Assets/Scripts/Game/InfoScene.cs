using System.Collections;
using Attributes;
using Configs;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class InfoScene : MonoBehaviour
    {
        [SerializeField] private GameSettingsData _gameSettingsData;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _linkedInButton;
        [SceneListPopupField] [SerializeField] private string _mainMenuSceneName = string.Empty;
        
        [CanBeNull]
        private Coroutine _sceneCoroutine;
        
        private void OnEnable()
        {
            _mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
            _linkedInButton.onClick.AddListener(OnLinkedInButtonClicked);
        }

        private void OnDisable()
        {
            _mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClicked);
            _linkedInButton.onClick.RemoveListener(OnLinkedInButtonClicked);
        }
        
        private void OnMainMenuButtonClicked()
        {
            if (_sceneCoroutine != null)
                StopCoroutine(_sceneCoroutine);

            _sceneCoroutine = StartCoroutine(LoadScene(_mainMenuSceneName));
        }

        private void OnLinkedInButtonClicked()
        {
            Application.OpenURL(_gameSettingsData.LinkedInURL);
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
