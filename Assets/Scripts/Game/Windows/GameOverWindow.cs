using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Windows
{
    public class GameOverWindow : AWindow<EndGameWindowSetup>
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private Button _playAgainButton;

        [CanBeNull] private WindowSystem _windowSystem;
        [CanBeNull] private Action _playAgainCallback;

        private void OnEnable()
        {
            _playAgainButton.onClick.AddListener(OnPlayAgainClicked);
        }

        private void OnDisable()
        {
            _playAgainButton.onClick.RemoveListener(OnPlayAgainClicked);
        }

        public override void Init(EndGameWindowSetup windowSetup)
        {
            _playAgainCallback = windowSetup.PlayAgainCallback;
            _windowSystem = windowSetup.WindowSystem;
            _scoreText.text = windowSetup.Score.ToString();
        }

        private void OnPlayAgainClicked()
        {
            if (_windowSystem == null)
                return;
            
            _windowSystem.Close<GameOverWindow>();
            
            _playAgainCallback?.Invoke();
        }
    }
}