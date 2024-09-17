using Configs;
using Game.Balls;
using Game.Level;
using Game.Pools;
using Interfaces;
using UnityEngine;

namespace Game.Spawners
{
    public class FireBallSpawner : ISubscribable, IServisable
    {
        private readonly FireBallPoolCreator _fireBallPoolCreator;
        private readonly LevelController _levelController;
        private readonly DragButton _dragButton;
        private readonly Vector3 _position;
        private readonly GameSettingsData _data;
        
        private bool _canShoot = true;

        public FireBallSpawner(ServiceLocator serviceLocator, Transform startBall)
        {
            _fireBallPoolCreator = serviceLocator.GetService<FireBallPoolCreator>();
            _levelController = serviceLocator.GetService<LevelController>();
            _dragButton = serviceLocator.GetService<DragButton>();
            _data = serviceLocator.GetService<GameSettingsData>();

            _position = startBall.position;
        }
        
        public void Subscribe()
        {
            _dragButton.EndDragEvent += SpawnBall;
            _fireBallPoolCreator.CollisionEvent += OnFireBallCollided;
        }

        public void Unsubscribe()
        {
            _dragButton.EndDragEvent -= SpawnBall;
            _fireBallPoolCreator.CollisionEvent -= OnFireBallCollided;
        }

        private void SpawnBall(Vector2 direction)
        {
            if (_fireBallPoolCreator == null)
                return;

            if (!_levelController.IsEnoughShots)
                return;

            if (!_canShoot)
                return;

            _canShoot = false;

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
            var rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

            var ball = _fireBallPoolCreator.ObjectPool.Get();
            ball.Init(_data.GetBallSprite(_levelController.CurrentBallType), _levelController.CurrentBallType);
            
            var tr = ball.transform;
            tr.position = _position;
            tr.rotation = rotation;

            _levelController.ChangeBallType();
        }
        
        private void OnFireBallCollided(Ball ball, Collision2D collision)
        {
            _canShoot = true;
        }
    }
}