using Configs;
using Game;
using Pools;
using Spawners;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private DragButton _dragButton;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _fireBall;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private BallPoolCreator _ballPoolCreator;
    [SerializeField] private GameUpdater _gameUpdater;
    [SerializeField] private Walls _walls;
    [SerializeField] private GameSettingsData _gameSettingsData;
    [SerializeField] private LevelDataLoader _levelDataLoader;

    private void Start()
    {
        _walls.SetupWalls(_mainCamera);

        var gameObserver = new GameSubscriber();
        var serviceLocator = new ServiceLocator();

        serviceLocator.AddService(_dragButton);
        serviceLocator.AddService(_ballPoolCreator);
        serviceLocator.AddService(_gameUpdater);
        serviceLocator.AddService(_gameSettingsData);
        serviceLocator.AddService(_walls);
        serviceLocator.AddService(_levelDataLoader);

        _ballPoolCreator.Init(serviceLocator);
        var levelController = new LevelController(serviceLocator);
        levelController.Init();

        var line = new LineRendererActivator(serviceLocator, _fireBall, _lineRenderer);
        gameObserver.AddListener(line);
        
        var ballSpawner = new BallSpawner(serviceLocator, _fireBall);
        gameObserver.AddListener(ballSpawner);
    }
}
