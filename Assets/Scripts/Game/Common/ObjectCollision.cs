using Interfaces;
using UnityEngine;

namespace Game.Common
{
    public class ObjectCollision : ISubscribable
    {
        private readonly ICollisionable _owner;

        protected ObjectCollision(ICollisionable owner)
        {
            _owner = owner;
        }

        public void Subscribe()
        {
            _owner.CollisionEvent += OnCollision;
        }

        public void Unsubscribe()
        {
            _owner.CollisionEvent -= OnCollision;
        }

        protected virtual void OnCollision(Collision2D collision)
        {
        }
    }
}
