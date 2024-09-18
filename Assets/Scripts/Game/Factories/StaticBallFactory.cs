using System.Collections.Generic;
using System.Linq;
using Game.Balls;
using Interfaces;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Factories
{
    public class StaticBallFactory : MonoBehaviour, IServisable
    {
        [SerializeField] private Ball _ballPrefab;
        
        private readonly List<Ball> _createdBalls = new();
        
        public IObjectPool<Ball> ObjectPool { get; private set; }

        public IReadOnlyList<Ball> CreatedBalls => _createdBalls;
        public IReadOnlyList<Ball> GetActiveBalls => _createdBalls.Where(o => o.gameObject.activeSelf).ToList();

        public void Init()
        {
            ObjectPool = new ObjectPool<Ball>(CreateProjectile, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject);
        }

        public void ReleaseBalls(IEnumerable<Ball> balls)
        {
            foreach (var ball in balls)
                ObjectPool.Release(ball);
        }
        
        private Ball CreateProjectile()
        {
            var ball = Instantiate(_ballPrefab, transform);
            _createdBalls.Add(ball);
            
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
            _createdBalls.Remove(pooledObject);
            Destroy(pooledObject.gameObject);
        }
    }
}