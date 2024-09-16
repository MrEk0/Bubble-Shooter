using Configs;
using Enums;
using Game;
using Interfaces;
using Pools;
using UnityEngine;

namespace Spawners
{
    public class FireBallSpawner : ISubscribable
    {
        private readonly FireBallPoolCreator _fireBallPoolCreator;
        private readonly DragButton _dragButton;
        private readonly Vector3 _position;
        private readonly GameSettingsData _data;

        private float _timer;
        private Bounds _bounds;

        public FireBallSpawner(ServiceLocator serviceLocator, Transform startBall)
        {
            _fireBallPoolCreator = serviceLocator.GetService<FireBallPoolCreator>();
            _dragButton = serviceLocator.GetService<DragButton>();
            _data = serviceLocator.GetService<GameSettingsData>();

            _position = startBall.position;
        }
        
        public void Subscribe()
        {
            _dragButton.EndDragEvent += SpawnBall;
        }

        public void Unsubscribe()
        {
            _dragButton.EndDragEvent -= SpawnBall;
        }

        private void SpawnBall(Vector2 direction)
        {
            if (_fireBallPoolCreator == null)
                return;

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
            var rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

            var ballType = BallEnum.Duck;
            
            var ball = _fireBallPoolCreator.ObjectPool.Get();
            ball.Init(_data.GetBallSprite(ballType), ballType);
            var tr = ball.transform;
            tr.position = _position;
            tr.rotation = rotation;
        }
    }
}