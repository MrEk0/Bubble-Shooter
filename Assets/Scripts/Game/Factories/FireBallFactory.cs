using System;
using System.Collections.Generic;
using Configs;
using Game.Balls;
using Game.Common;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Factories
{
    public class FireBallFactory : BallFactory
    {
        [Serializable]
        public class FireBallSettings
        {
            public ObjectMovement ObjectMovement;
            public ObjectCollision ObjectCollision;
        }
        
        [SerializeField] private Ball _ballPrefab;

        [CanBeNull] private GameUpdater _gameUpdater;
        [CanBeNull] private GameSubscriber _gameSubscriber;
        [CanBeNull] private ServiceLocator _serviceLocator;
        
        private float _ballVelocity;

        private readonly Dictionary<GameObject, FireBallSettings> _objectSettings = new();
        
        public event Action<Ball, Collision2D> CollisionEvent = delegate { };

        public void Init(ServiceLocator serviceLocator)
        {
            _gameUpdater = serviceLocator.GetService<GameUpdater>();
            _gameSubscriber = serviceLocator.GetService<GameSubscriber>();
            _serviceLocator = serviceLocator;
            
            _ballVelocity = serviceLocator.GetService<GameSettingsData>().BallVelocity;

            ObjectPool = new ObjectPool<Ball>(CreateProjectile, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject);
        }

        protected override Ball CreateProjectile()
        {
            var ball = Instantiate(_ballPrefab, transform);

            if (_gameUpdater == null || _gameSubscriber == null)
                return ball;
            
            var tr = ball.transform;
            var movement = new ObjectMovement(tr, _ballVelocity);
            var collision = new FireBallCollision(ball, tr, _serviceLocator, other =>
            {
                if (!CanRelease(ball))
                    return;
                
                CollisionEvent(ball, other);
                
                ObjectPool.Release(ball);
            });

            _objectSettings.Add(ball.gameObject, new FireBallSettings { ObjectMovement = movement, ObjectCollision = collision });
            _gameUpdater.AddListener(movement);
            _gameSubscriber.AddListener(collision);

            return ball;
        }

        protected override void OnDestroyPooledObject(Ball pooledObject)
        {
            if (_gameUpdater == null || _gameSubscriber == null)
                return;
            
            if (_objectSettings.TryGetValue(pooledObject.gameObject, out var settings))
            {
                _gameUpdater.RemoveListener(settings.ObjectMovement);
                _objectSettings.Remove(pooledObject.gameObject);
                _gameSubscriber.RemoveListener(settings.ObjectCollision);
            }
            
            Destroy(pooledObject.gameObject);
        }
    }
}