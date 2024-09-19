using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Enums;
using Game.Balls;
using Game.Controllers;
using Game.Windows;
using Interfaces;
using JetBrains.Annotations;
using Random = UnityEngine.Random;

namespace Game.Level
{
    public class LevelController : IServisable
    {
        [CanBeNull] private readonly GameSettingsData _data;
        [CanBeNull] private readonly ServiceLocator _serviceLocator;
        [CanBeNull] private readonly WindowSystem _windowSystem;
        
        private readonly List<BallEnum> _availableBalls = new();

        private int _currentScore;
        private int _leftShotCount;

        private BallEnum NextBallType { get; set; }

        public event Action<int> ChangeScoreEvent = delegate { };
        public event Action<BallEnum, int> ChangeShotsCountEvent = delegate { };

        public BallEnum CurrentBallType { get; private set; }
        public bool IsEnoughShots => _leftShotCount > 0;

        public LevelController(ServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;

            _windowSystem = serviceLocator.GetService<WindowSystem>();
            _data = serviceLocator.GetService<GameSettingsData>();
            if (_data == null)
                return;

            var levelDataLoader = serviceLocator.GetService<LevelDataLoader>();

            _availableBalls.AddRange(levelDataLoader.LevelRowSettings.Where(o => o.IsAvailable).Select(o => o.Type).ToList());

            NextBallType = _availableBalls[Random.Range(0, _availableBalls.Count)];
            CurrentBallType = _availableBalls[Random.Range(0, _availableBalls.Count)];
            _leftShotCount = _data.LevelShotsCount;
        }

        public void StartLevel()
        {
            if (_serviceLocator == null)
                return;
            
            var gameSubscriber = _serviceLocator.GetService<GameSubscriber>();
            var gameWindow = _serviceLocator.GetService<GameWindow>();

            gameWindow.Init(_serviceLocator);
            gameSubscriber.AddListener(gameWindow);
            
            _serviceLocator.GetService<StaticBallsController>().StartLevel();

            ChangeScoreEvent(_currentScore);
            ChangeShotsCountEvent(NextBallType, _leftShotCount);
        }

        public void ChangeScore(int releasedBalls)
        {
            if (_data == null)
                return;
            
            _currentScore += _data.ScorePerBall * releasedBalls;

            ChangeScoreEvent(_currentScore);
        }

        public void ChangeBallType()
        {
            CurrentBallType = NextBallType;
            NextBallType = _availableBalls[Random.Range(0, _availableBalls.Count)];

            _leftShotCount--;

            ChangeShotsCountEvent(NextBallType, _leftShotCount);
        }

        public void CheckGameState(Dictionary<float, List<Ball>> balls)
        {
            if (_windowSystem == null || _data == null)
                return;

            var firstRowKey = balls.Keys.Max();
            if (!balls.TryGetValue(firstRowKey, out var firstRowBalls))
                return;

            var firstRowLeftBall = firstRowBalls.Where(o => o.gameObject.activeSelf).ToList();
            var rate = firstRowBalls.Count == 0 ? 0 : (float)firstRowLeftBall.Count / firstRowBalls.Count;

            if (rate <= _data.FirstRowVictoryBallsRate)
            {
                var endGameWindowSetup = new EndGameWindowSetup
                {
                    PlayAgainCallback = Replay,
                    Score = _currentScore,
                    WindowSystem = _windowSystem
                };
                _windowSystem.Open<GameWinWindow, EndGameWindowSetup>(endGameWindowSetup);
                return;
            }

            if (_leftShotCount > 0)
                return;

            var setup = new EndGameWindowSetup
            {
                PlayAgainCallback = Replay,
                Score = _currentScore,
                WindowSystem = _windowSystem
            };
            _windowSystem.Open<GameOverWindow, EndGameWindowSetup>(setup);
        }

        private void Replay()
        {
            if (_serviceLocator == null || _data == null)
                return;
            
            _currentScore = 0;
            
            NextBallType = _availableBalls[Random.Range(0, _availableBalls.Count)];
            CurrentBallType = _availableBalls[Random.Range(0, _availableBalls.Count)];
            _leftShotCount = _data.LevelShotsCount;
            
            _serviceLocator.GetService<StaticBallsController>().StartLevel();

            ChangeScoreEvent(_currentScore);
            ChangeShotsCountEvent(NextBallType, _leftShotCount);
        }
    }
}
