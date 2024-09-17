using System.Collections.Generic;
using System.Linq;
using Configs;
using Game.Balls;
using Game.Level;
using Game.Pools;
using Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Spawners
{
    public class StaticBallSpawner : ISubscribable, IServisable
    {
        private readonly LevelDataLoader _levelDataLoader;
        private readonly LevelController _levelController;
        private readonly StaticBallPoolCreator _staticBallPoolCreator;
        private readonly FireBallPoolCreator _fireBallPoolCreator;
        private readonly GameSettingsData _data;
        private readonly Walls _walls;
        private readonly List<Ball> _connectedBalls = new();

        public StaticBallSpawner(ServiceLocator serviceLocator)
        {
            _levelDataLoader = serviceLocator.GetService<LevelDataLoader>();
            _levelController = serviceLocator.GetService<LevelController>();
            _walls = serviceLocator.GetService<Walls>();
            _data = serviceLocator.GetService<GameSettingsData>();
            _staticBallPoolCreator = serviceLocator.GetService<StaticBallPoolCreator>();
            _fireBallPoolCreator = serviceLocator.GetService<FireBallPoolCreator>();
        }

        public void StartLevel()
        {
            var startPosition = new Vector3(_walls.Bounds.min.x + _data.StartPositionOffset.x, _walls.Bounds.max.y + _data.StartPositionOffset.y, 0f);
            var maxBallsInRow = _data.BallSpacing.x <= 0f ? 0 : Mathf.FloorToInt((_walls.Bounds.size.x - _data.StartPositionOffset.x) / _data.BallSpacing.x) + 1;

            var availableBalls = _levelDataLoader.LevelRowSettings.Where(o => o.IsAvailable).Select(o => o.Type).ToList();

            for (var i = 0; i < _data.LevelRowCounts; i++)
            {
                var rowBalls = i % 2 != 0 ? maxBallsInRow - 1 : maxBallsInRow;
                var rowStartPosition = i % 2 != 0 ? new Vector3(startPosition.x + _data.BallSpacing.x * 0.5f, startPosition.y, 0f) : startPosition;

                for (var j = 0; j < rowBalls; j++)
                {
                    var ballType = availableBalls[Random.Range(0, availableBalls.Count)];

                    var ball = _staticBallPoolCreator.ObjectPool.Get();
                    ball.transform.position = new Vector3(rowStartPosition.x + _data.BallSpacing.x * j, rowStartPosition.y - _data.BallSpacing.y * i, 0);
                    ball.Init(_data.GetBallSprite(ballType), ballType);
                }
            }
        }

        public void Subscribe()
        {
            if (_fireBallPoolCreator == null)
                return;

            _fireBallPoolCreator.CollisionEvent += OnBallCollided;
        }

        public void Unsubscribe()
        {
            if (_fireBallPoolCreator == null)
                return;

            _fireBallPoolCreator.CollisionEvent -= OnBallCollided;
        }

        private void OnBallCollided(Ball collidedBall, Collision2D collision)
        {
            _connectedBalls.Clear();

            var collidedPos = collidedBall.transform.position;
            var collisionBallPos = collision.transform.position;
            var possiblePositions = new Vector3[]
            {
                new(collisionBallPos.x + _data.BallSpacing.x * 0.5f, collisionBallPos.y + _data.BallSpacing.y, 0f),
                new(collisionBallPos.x + _data.BallSpacing.x * 0.5f, collisionBallPos.y - _data.BallSpacing.y, 0f),
                new(collisionBallPos.x - _data.BallSpacing.x * 0.5f, collisionBallPos.y + _data.BallSpacing.y, 0f),
                new(collisionBallPos.x - _data.BallSpacing.x * 0.5f, collisionBallPos.y - _data.BallSpacing.y, 0f)
            };

            var ball = _staticBallPoolCreator.ObjectPool.Get();
            ball.transform.position = possiblePositions.OrderBy(o => Vector3.Distance(o, collidedPos)).FirstOrDefault();
            ball.Init(_data.GetBallSprite(collidedBall.Type), collidedBall.Type);

            var typedBalls = _staticBallPoolCreator.CreatedBalls.Where(o => o.Type == collidedBall.Type).ToList();
            var maxDistance = new Vector3(_data.BallSpacing.x * 0.5f, _data.BallSpacing.y).magnitude;

            GetNeighbors(typedBalls, ball.transform, maxDistance);

            if (_data.MinBallsCountToRelease > _connectedBalls.Count)
                return;

            _staticBallPoolCreator.ReleaseBalls(_connectedBalls);

            _levelController.AddScore(_connectedBalls.Count);
        }

        private void GetNeighbors(IReadOnlyList<Ball> list, Transform ball, float maxDistance)
        {
            var neighbors = list.Where(typedBall => Vector3.Distance(typedBall.transform.position, ball.position) <= maxDistance).ToList();
            var newNeighbors = neighbors.Where(o => _connectedBalls.All(oo => o != oo && o.transform != ball.transform)).ToList();
            _connectedBalls.AddRange(newNeighbors);

            foreach (var neighbor in newNeighbors)
                GetNeighbors(list, neighbor.transform, maxDistance);
        }
    }
}
