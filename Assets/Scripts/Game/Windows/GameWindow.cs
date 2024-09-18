using System;
using Configs;
using Enums;
using Game.Level;
using Interfaces;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Windows
{
    public class GameWindow : MonoBehaviour, ISubscribable, IServisable
    {
        [SerializeField] private Button _exitButton;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _fireBallLeftText;
        [SerializeField] private Image _nextFireBallImage;
        
        [CanBeNull] private ServiceLocator _serviceLocator;
        [CanBeNull] private LevelController _levelController;
        [CanBeNull] private GameSettingsData _gameSettingsData;

        public event Action OnExitClickEvent = delegate { };

        private void OnEnable()
        {
            _exitButton.onClick.AddListener(OnExitClicked);
        }

        private void OnDisable()
        {
            _exitButton.onClick.RemoveListener(OnExitClicked);
        }
        
        public void Init(ServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
            if (_serviceLocator == null)
                return;

            _levelController = _serviceLocator.GetService<LevelController>();
            _gameSettingsData = _serviceLocator.GetService<GameSettingsData>();
            if (_gameSettingsData == null)
                return;
            
            _fireBallLeftText.text = _gameSettingsData.LevelShotsCount.ToString();
        }

        public void Subscribe()
        {
            if (_levelController == null)
                return;
            
            _levelController.ChangeScoreEvent += OnScoreChanged;
            _levelController.ChangeShotsCountEvent += OnShotCountChanged;
        }

        public void Unsubscribe()
        {
            if (_levelController == null)
                return;
            
            _levelController.ChangeScoreEvent -= OnScoreChanged;
            _levelController.ChangeShotsCountEvent -= OnShotCountChanged;
        }
        
        private void OnExitClicked()
        {
            OnExitClickEvent();
        }

        private void OnScoreChanged(int score)
        {
            _scoreText.text = score.ToString();
        }

        private void OnShotCountChanged(BallEnum nextFireBallSprite, int leftFireBallCount)
        {
            if (_gameSettingsData == null)
                return;
            
            _fireBallLeftText.text = leftFireBallCount.ToString();
            _nextFireBallImage.sprite = _gameSettingsData.GetBallSprite(nextFireBallSprite);
        }
    }
}
