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
    [SerializeField] private Walls _walls;
    [SerializeField] private BallPoolCreator _ballPoolCreator;
    [SerializeField] private GameUpdater _gameUpdater;

    void Start()
    {
        var gameObserver = new GameSubscriber();
        var serviceLocator = new ServiceLocator();

        serviceLocator.AddService(_dragButton);
        serviceLocator.AddService(_ballPoolCreator);
        serviceLocator.AddService(_gameUpdater);

        _ballPoolCreator.Init(serviceLocator);

        var line = new LineRendererActivator(serviceLocator, _fireBall, _lineRenderer);
        gameObserver.AddListener(line);
        
        var ballSpawner = new BallSpawner(serviceLocator);
        gameObserver.AddListener(ballSpawner);

        _walls.SetupWalls(_mainCamera);
    }
}
