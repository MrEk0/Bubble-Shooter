using Game.Balls;
using UnityEngine;
using Interfaces;
using UnityEngine.Pool;

namespace Game.Factories
{
    public abstract class BallFactory : MonoBehaviour, IServisable
    {
        public IObjectPool<Ball> ObjectPool { get; protected set; }

        protected bool CanRelease(Ball poolObject) => poolObject.gameObject.activeSelf;

        protected abstract Ball CreateProjectile();
        protected abstract void OnDestroyPooledObject(Ball pooledObject);

        protected static void OnReleaseToPool(Ball pooledObject)
        {
            pooledObject.gameObject.SetActive(false);
        }

        protected static void OnGetFromPool(Ball pooledObject)
        {
            pooledObject.gameObject.SetActive(true);
        }
    }
}