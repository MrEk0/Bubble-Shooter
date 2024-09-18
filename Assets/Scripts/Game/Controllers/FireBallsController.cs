using Configs;
using Game.Balls;
using Game.Factories;
using Game.Level;
using Interfaces;
using UnityEngine;

namespace Game.Controllers
{
    public class FireBallsController : ISubscribable, IServisable
    {
        private readonly FireBallFactory _fireBallFactory;
        private readonly LevelController _levelController;
        private readonly DragButton _dragButton;
        private readonly Vector3 _position;
        private readonly GameSettingsData _data;
        private readonly Ball _fireBall;
        
        private bool _canShoot = true;

        public FireBallsController(ServiceLocator serviceLocator, Ball fireBall)
        {
            _fireBallFactory = serviceLocator.GetService<FireBallFactory>();
            _levelController = serviceLocator.GetService<LevelController>();
            _dragButton = serviceLocator.GetService<DragButton>();
            _data = serviceLocator.GetService<GameSettingsData>();
            _fireBall = fireBall;

            _fireBall.Setup(_data.GetBallSprite(_levelController.CurrentBallType), _levelController.CurrentBallType);
            _position = fireBall.transform.position;
        }
        
        public void Subscribe()
        {
            _dragButton.EndDragEvent += Shot;
            _fireBallFactory.CollisionEvent += OnFireBallCollided;
        }

        public void Unsubscribe()
        {
            _dragButton.EndDragEvent -= Shot;
            _fireBallFactory.CollisionEvent -= OnFireBallCollided;
        }

        private void Shot(Vector2 direction)
        {
            if (_fireBallFactory == null)
                return;

            if (!_levelController.IsEnoughShots)
                return;

            if (!_canShoot)
                return;

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
            if ((angle >= 0f && angle > _data.AimLineAngleRange.x) || (angle < 0f && angle < _data.AimLineAngleRange.y))
                return;

            _canShoot = false;

            var rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

            var ball = _fireBallFactory.ObjectPool.Get();
            ball.Setup(_data.GetBallSprite(_levelController.CurrentBallType), _levelController.CurrentBallType);

            var tr = ball.transform;
            tr.position = _position;
            tr.rotation = rotation;

            _levelController.ChangeBallType();
            _fireBall.Setup(_data.GetBallSprite(_levelController.CurrentBallType), _levelController.CurrentBallType);
        }

        private void OnFireBallCollided(Ball ball, Collision2D collision)
        {
            _canShoot = true;
        }
    }
}