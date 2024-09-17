using Configs;
using Game.Pools;
using Game.Spawners;
using Game.Windows;
using UnityEngine;

namespace Game.Level
{
    public class LevelInitializer : MonoBehaviour
    {
        [SerializeField] private DragButton _dragButton;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Transform _fireBall;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private FireBallPoolCreator _fireBallPoolCreator;
        [SerializeField] private StaticBallPoolCreator _staticBallPoolCreator;
        [SerializeField] private GameUpdater _gameUpdater;
        [SerializeField] private Walls _walls;
        [SerializeField] private GameSettingsData _gameSettingsData;
        [SerializeField] private LevelDataLoader _levelDataLoader;
        [SerializeField] private GameWindow _gameWindow;
        [SerializeField] private WindowSystem _windowSystem;

        private GameSubscriber _gameSubscriber;
        private ServiceLocator _serviceLocator;

        private void Start()
        {
            _walls.SetupWalls(_mainCamera);

            _gameSubscriber = new GameSubscriber();
            _serviceLocator = new ServiceLocator();

            _serviceLocator.AddService(_fireBallPoolCreator);
            _serviceLocator.AddService(_staticBallPoolCreator);
            _serviceLocator.AddService(_gameUpdater);
            _serviceLocator.AddService(_gameSettingsData);
            _serviceLocator.AddService(_walls);
            _serviceLocator.AddService(_levelDataLoader);
            _serviceLocator.AddService(_gameSubscriber);
            _serviceLocator.AddService(_dragButton);
            _serviceLocator.AddService(_gameWindow);
            _serviceLocator.AddService(_windowSystem);

            _fireBallPoolCreator.Init(_serviceLocator);
            _staticBallPoolCreator.Init();

            var levelController = new LevelController(_serviceLocator, _spriteRenderer);
            _serviceLocator.AddService(levelController);

            var ballSpawner = new FireBallSpawner(_serviceLocator, _fireBall);
            _gameSubscriber.AddListener(ballSpawner);
            _serviceLocator.AddService(ballSpawner);

            var staticBallSpawner = new StaticBallSpawner(_serviceLocator);
            _gameSubscriber.AddListener(staticBallSpawner);
            _serviceLocator.AddService(staticBallSpawner);

            var line = new LineRendererActivator(_dragButton, _fireBall, _lineRenderer);
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
