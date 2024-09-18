using System.Collections.Generic;
using System.Linq;
using Game.Balls;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Factories
{
    public class StaticBallFactory : BallFactory
    {
        [SerializeField] private Ball _ballPrefab;
        
        private readonly List<Ball> _createdBalls = new();

        public IReadOnlyList<Ball> CreatedBalls => _createdBalls;

        public void Init()
        {
            ObjectPool = new ObjectPool<Ball>(CreateProjectile, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject);
        }

        public void ReleaseBalls(IEnumerable<Ball> balls)
        {
            foreach (var ball in balls.Where(CanRelease))
                ObjectPool.Release(ball);
        }
        
        protected override Ball CreateProjectile()
        {
            var ball = Instantiate(_ballPrefab, transform);
            _createdBalls.Add(ball);
            
            return ball;
        }

        protected override void OnDestroyPooledObject(Ball pooledObject)
        {
            _createdBalls.Remove(pooledObject);
            Destroy(pooledObject.gameObject);
        }
    }
}