using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Enums;
using Game.Balls;
using Game.Controllers;
using Game.Windows;
using Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Level
{
    public class LevelController : IServisable
    {
        private readonly GameSettingsData _data;
        private readonly List<BallEnum> _availableBalls = new();
        private readonly ServiceLocator _serviceLocator;
        private readonly WindowSystem _windowSystem;

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
            var gameSubscriber = serviceLocator.GetService<GameSubscriber>();
            var gameWindow = serviceLocator.GetService<GameWindow>();

            gameWindow.Init(_serviceLocator);
            gameSubscriber.AddListener(gameWindow);
            
            var levelDataLoader = serviceLocator.GetService<LevelDataLoader>();

            _availableBalls.AddRange(levelDataLoader.LevelRowSettings.Where(o => o.IsAvailable).Select(o => o.Type).ToList());

            NextBallType = _availableBalls[Random.Range(0, _availableBalls.Count)];
            CurrentBallType = _availableBalls[Random.Range(0, _availableBalls.Count)];
            _leftShotCount = _data.LevelShotsCount;
        }

        public void StartLevel()
        {
            _serviceLocator.GetService<StaticBallsController>().StartLevel();

            ChangeScoreEvent(_currentScore);
            ChangeShotsCountEvent(NextBallType, _leftShotCount);
        }

        public void ChangeScore(int releasedBalls)
        {
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

        public void CheckWinCondition(IReadOnlyList<Ball> activeBalls)
        {
            var firstRowY = activeBalls.Max(o => o.transform.position.y);
            var firstRowLeftBall = activeBalls.Where(activeBall => Math.Abs(activeBall.transform.position.y - firstRowY) < Mathf.Epsilon).ToList();

            if (firstRowLeftBall.Count > _data.FirstRowVictoryBallsRate)
                return;
            
            var setup = new EndGameWindowSetup
            {
                PlayAgainCallback = Replay,
                Score = _currentScore,
                WindowSystem = _windowSystem
            };
            _windowSystem.Open<GameWinWindow, EndGameWindowSetup>(setup);
        }

        public void CheckLoseCondition()
        {
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
            _currentScore = 0;
            
            NextBallType = _availableBalls[Random.Range(0, _availableBalls.Count)];
            CurrentBallType = _availableBalls[Random.Range(0, _availableBalls.Count)];
            _leftShotCount = _data.LevelShotsCount;
            
            StartLevel();
        }
    }
}
