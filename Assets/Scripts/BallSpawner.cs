using Game;
using Interfaces;
using Pools;
using UnityEngine;

namespace Spawners
{
    public class BallSpawner : ISubscribable
    {
        private readonly BallPoolCreator _ballPoolCreator;
        private readonly DragButton _dragButton;
        private readonly Vector3 _position;

        private float _timer;
        private Bounds _bounds;

        public BallSpawner(ServiceLocator serviceLocator, Transform startBall)
        {
            _ballPoolCreator = serviceLocator.GetService<BallPoolCreator>();
            _dragButton = serviceLocator.GetService<DragButton>();

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
            if (_ballPoolCreator == null)
                return;

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
            var rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

            var ball = _ballPoolCreator.ObjectPool.Get();
            var tr = ball.transform;
            tr.position = _position;
            tr.rotation = rotation;
        }
    }
}