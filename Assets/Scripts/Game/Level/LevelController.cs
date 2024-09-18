using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Enums;
using Game.Balls;
using Game.Controllers;
using Game.Factories;
using Game.Windows;
using Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Level
{
    public class LevelController : IServisable, ISubscribable
    {
        private readonly GameSettingsData _data;
        private readonly List<BallEnum> _availableBalls = new();
        private readonly ServiceLocator _serviceLocator;
        private readonly FireBallFactory _fireBallFactory;
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
            _fireBallFactory = serviceLocator.GetService<FireBallFactory>();
            _data = serviceLocator.GetService<GameSettingsData>();

            var levelDataLoader = serviceLocator.GetService<LevelDataLoader>();
            var gameSubscriber = serviceLocator.GetService<GameSubscriber>();

            gameSubscriber.AddListener(this);
            serviceLocator.AddService(this);

            _availableBalls.AddRange(levelDataLoader.LevelRowSettings.Where(o => o.IsAvailable).Select(o => o.Type).ToList());

            NextBallType = _availableBalls[Random.Range(0, _availableBalls.Count)];
            CurrentBallType = _availableBalls[Random.Range(0, _availableBalls.Count)];
            _leftShotCount = _data.LevelShotsCount;
        }
        
        public void Subscribe()
        {
            _fireBallFactory.CollisionEvent += OnFireBallCollided;
        }

        public void Unsubscribe()
        {
            _fireBallFactory.CollisionEvent += OnFireBallCollided;
        }

        public void StartLevel()
        {
            _serviceLocator.GetService<StaticBallsController>().StartLevel();
            var gameSubscriber = _serviceLocator.GetService<GameSubscriber>();
            var gameWindow = _serviceLocator.GetService<GameWindow>();

            gameWindow.Init(_serviceLocator);
            gameSubscriber.AddListener(gameWindow);

            ChangeScoreEvent(_currentScore);
            ChangeShotsCountEvent(NextBallType, _leftShotCount);
        }

        public void AddScore(int releasedBalls)
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
        
        private void OnFireBallCollided(Ball ball, Collision2D collision)
        {
            if (_leftShotCount > 0) 
                return;
            
            var setup = new EndGameWindowSetup
            {
                Score = _currentScore,
                WindowSystem = _windowSystem
            };
            _windowSystem.Open<GameOverWindow, EndGameWindowSetup>(setup);
        }
    }
}
