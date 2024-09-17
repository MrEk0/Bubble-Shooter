using Interfaces;
using JetBrains.Annotations;
using UnityEngine;

namespace Game.Common
{
    public class ObjectMovement : IGameUpdatable
    {
        private readonly float _velocity;

        [CanBeNull] private readonly Transform _owner;

        public ObjectMovement(Transform owner, float velocity)
        {
            _owner = owner;
            _velocity = velocity;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_owner == null)
                return;

            var newPos = _owner.up * (_velocity * Time.deltaTime);
            _owner.position += newPos;
        }
    }
}
