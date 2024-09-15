using System.Linq;
using Configs;
using Game;
using Pools;
using UnityEngine;

public class LevelController
{
    private readonly LevelDataLoader _levelDataLoader;
    private readonly BallPoolCreator _ballPoolCreator;
    private readonly GameSettingsData _data;
    private readonly Walls _walls;

    public LevelController(ServiceLocator serviceLocator)
    {
        _levelDataLoader = serviceLocator.GetService<LevelDataLoader>();
        _walls = serviceLocator.GetService<Walls>();
        _data = serviceLocator.GetService<GameSettingsData>();
        _ballPoolCreator = serviceLocator.GetService<BallPoolCreator>();
    }

    public void Init()
    {
        _levelDataLoader.LoadData();//todo remove

        var startPosition = new Vector3(_walls.Bounds.min.x + _data.StartPositionOffset.x, _walls.Bounds.max.y + _data.StartPositionOffset.y, 0f);
        var availableBalls = _levelDataLoader.LevelRowSettings.Where(o => o.IsAvailable).Select(o => o.Type).ToList();
        var maxBallsInRow = _data.BallSpacing.x <= 0f ? 0 : Mathf.FloorToInt((_walls.Bounds.size.x - _data.StartPositionOffset.x) / _data.BallSpacing.x) + 1; 
        
        for (var i = 0; i < _data.LevelRowCounts; i++)
        {
            var rowBalls = i % 2 != 0 ? maxBallsInRow - 1 : maxBallsInRow;
            var rowStartPosition = i % 2 != 0 ? new Vector3(startPosition.x + _data.BallSpacing.x * 0.5f, startPosition.y, 0f) : startPosition;
            
            for (var j = 0; j < rowBalls; j++)
            {
                var ballType = availableBalls[Random.Range(0, availableBalls.Count)];

                var ball = _ballPoolCreator.ObjectPool.Get();
                ball.transform.position = new Vector3(rowStartPosition.x + _data.BallSpacing.x * j, rowStartPosition.y - _data.BallSpacing.y * i, 0);
                ball.Init(_data.GetBallSprite(ballType));
            }
        }
    }
}
