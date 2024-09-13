using System;
using Game;
using Interfaces;
using Pools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spawners
{
    public class BallSpawner : ISubscribable
    {
        private BallPoolCreator _ballPoolCreator;
        private DragButton _dragButton;

        private float _timer;
        private Bounds _bounds;

        public BallSpawner(ServiceLocator serviceLocator)
        {
            _ballPoolCreator = serviceLocator.GetService<BallPoolCreator>();
            _dragButton = serviceLocator.GetService<DragButton>();
        }

        private void SpawnNewAsteroid(Vector2 direction)
        {
            if (_ballPoolCreator == null)
                return;

            var position = new Vector3(Random.Range(_bounds.min.x, _bounds.max.x), _bounds.max.y, 0f);
            
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            var rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

            var ball = _ballPoolCreator.ObjectPool.Get();
            var tr = ball.transform;
            tr.position = position;
            tr.rotation = rotation;
        }

        public void Subscribe()
        {
            _dragButton.EndDragEvent += SpawnNewAsteroid;
        }

        public void Unsubscribe()
        {
            _dragButton.EndDragEvent -= SpawnNewAsteroid;
        }
    }
}