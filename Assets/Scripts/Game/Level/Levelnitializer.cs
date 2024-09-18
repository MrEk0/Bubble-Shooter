using Configs;
using Game.Balls;
using Game.Controllers;
using Game.Factories;
using Game.Windows;
using UnityEngine;

namespace Game.Level
{
    public class LevelInitializer : MonoBehaviour
    {
        [SerializeField] private GameSettingsData _gameSettingsData;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private Ball _fireBall;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private DragButton _dragButton;
        [SerializeField] private FireBallFactory _fireBallFactory;
        [SerializeField] private StaticBallFactory _staticBallFactory;
        [SerializeField] private GameUpdater _gameUpdater;
        [SerializeField] private Walls _walls;
        [SerializeField] private LevelDataLoader _levelDataLoader;
        [SerializeField] private GameWindow _gameWindow;
        [SerializeField] private WindowSystem _windowSystem;

        private GameSubscriber _gameSubscriber;
        private ServiceLocator _serviceLocator;

        private void Start()
        {
            _walls.SetupWalls(_mainCamera);

            _serviceLocator = new ServiceLocator();
            _gameSubscriber = new GameSubscriber();

            _serviceLocator.AddService(_gameSubscriber);
            _serviceLocator.AddService(_gameUpdater);
            _serviceLocator.AddService(_gameSettingsData);
            _serviceLocator.AddService(_fireBallFactory);
            _serviceLocator.AddService(_staticBallFactory);
            _serviceLocator.AddService(_walls);
            _serviceLocator.AddService(_levelDataLoader);
            _serviceLocator.AddService(_dragButton);
            _serviceLocator.AddService(_gameWindow);
            _serviceLocator.AddService(_windowSystem);

            _fireBallFactory.Init(_serviceLocator);
            _staticBallFactory.Init();

            var levelController = new LevelController(_serviceLocator);
            _serviceLocator.AddService(levelController);

            var ballSpawner = new FireBallsController(_serviceLocator, _fireBall);
            _gameSubscriber.AddListener(ballSpawner);
            _serviceLocator.AddService(ballSpawner);

            var staticBallSpawner = new StaticBallsController(_serviceLocator);
            _gameSubscriber.AddListener(staticBallSpawner);
            _serviceLocator.AddService(staticBallSpawner);

            var line = new LineRendererActivator(_dragButton, _fireBall.transform, _lineRenderer);
            _gameSubscriber.AddListener(line);

            levelController.StartLevel();
        }

        private void OnDestroy()
        {
            _gameUpdater.RemoveAll();
            _gameSubscriber.RemoveAll();
            _serviceLocator.RemoveAll();
        }
    }
}
