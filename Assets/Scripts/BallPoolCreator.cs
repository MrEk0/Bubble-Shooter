using System.Collections.Generic;
using Game;
using Interfaces;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Pool;

namespace Pools
{
    public class BallPoolCreator : MonoBehaviour, IServisable
    {
        [SerializeField] private Ball _ballPrefab;
        
        [CanBeNull] private GameUpdater _gameUpdater;

        public IObjectPool<Ball> ObjectPool { get; private set; }
        
        private readonly Dictionary<GameObject, ObjectMovement> _objectMovements = new();

        public void Init(ServiceLocator serviceLocator)
        {
            _gameUpdater = serviceLocator.GetService<GameUpdater>();

            ObjectPool = new ObjectPool<Ball>(CreateProjectile, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject);
        }

        private Ball CreateProjectile()
        {
            var ball = Instantiate(_ballPrefab, transform);
            ball.ObjectPool = ObjectPool;

            var movement = new ObjectMovement(ball.transform, 0f);
            _objectMovements.Add(ball.gameObject, movement);
            _gameUpdater.AddListener(movement);
            
            return ball;
        }
  
        private void OnReleaseToPool(Ball pooledObject)
        {
            pooledObject.gameObject.SetActive(false);
        }
    
        private void OnGetFromPool(Ball pooledObject)
        {
            pooledObject.gameObject.SetActive(true);
        }
   
        private void OnDestroyPooledObject(Ball pooledObject)
        {
            if (_objectMovements.TryGetValue(pooledObject.gameObject, out var movement))
            {
                _gameUpdater.RemoveListener(movement);
                _objectMovements.Remove(pooledObject.gameObject);
            }
            
            Destroy(pooledObject.gameObject);
        }
    }
}