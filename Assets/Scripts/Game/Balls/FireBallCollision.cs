using System;
using Configs;
using Game.Common;
using JetBrains.Annotations;
using UnityEngine;

namespace Game.Balls
{
    public class FireBallCollision : ObjectCollision
    {
        private readonly int _wallMask;
        private readonly int _ballMask;
        [CanBeNull] private readonly Transform _ownerTransform;

        [CanBeNull] private readonly Action<Collision2D> _collisionCallback;

        public FireBallCollision(ICollisionable owner, Transform ownerTransform, ServiceLocator serviceLocator, Action<Collision2D> collisionCallback) : base(owner)
        {
            var data = serviceLocator.GetService<GameSettingsData>();

            _wallMask = data.WallMask.value;
            _ballMask = data.BallMask.value;
            _collisionCallback = collisionCallback;
            _ownerTransform = ownerTransform;
        }

        protected override void OnCollision(Collision2D collision)
        {
            if (_ownerTransform == null)
                return;
            
            if (_wallMask == (_wallMask | (1 << collision.gameObject.layer)))
            {
                var point = collision.GetContact(0);
                var direction = Vector2.Reflect(_ownerTransform.up, point.normal);

                var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                var rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

                _ownerTransform.rotation = rotation;
            }

            if (_ballMask == (_ballMask | (1 << collision.gameObject.layer)))
            {
                _collisionCallback?.Invoke(collision);
            }
        }
    }
}
