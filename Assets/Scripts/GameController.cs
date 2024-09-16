using Configs;
using Game;
using Pools;
using Spawners;
using UnityEngine;
using UnityEngine.Serialization;

public class GameController : MonoBehaviour
{
    [SerializeField] private DragButton _dragButton;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _fireBall;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private FireBallPoolCreator _fireBallPoolCreator;
    [SerializeField] private StaticBallPoolCreator _staticBallPoolCreator;
    [SerializeField] private GameUpdater _gameUpdater;
    [SerializeField] private Walls _walls;
    [SerializeField] private GameSettingsData _gameSettingsData;
    [SerializeField] private LevelDataLoader _levelDataLoader;

    private void Start()
    {
        _walls.SetupWalls(_mainCamera);

        var gameSubscriber = new GameSubscriber();
        var serviceLocator = new ServiceLocator();

        serviceLocator.AddService(_dragButton);
        serviceLocator.AddService(_fireBallPoolCreator);
        serviceLocator.AddService(_staticBallPoolCreator);
        serviceLocator.AddService(_gameUpdater);
        serviceLocator.AddService(_gameSettingsData);
        serviceLocator.AddService(_walls);
        serviceLocator.AddService(_levelDataLoader);
        serviceLocator.AddService(gameSubscriber);

        _fireBallPoolCreator.Init(serviceLocator);
        _staticBallPoolCreator.Init();
        
        var levelController = new LevelController(serviceLocator);
        levelController.Init();
        gameSubscriber.AddListener(levelController);

        var line = new LineRendererActivator(serviceLocator, _fireBall, _lineRenderer);
        gameSubscriber.AddListener(line);
        
        var ballSpawner = new FireBallSpawner(serviceLocator, _fireBall);
        gameSubscriber.AddListener(ballSpawner);
    }
}
