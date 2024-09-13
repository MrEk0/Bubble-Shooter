using Interfaces;
using JetBrains.Annotations;
using UnityEngine;

public class BallMovement : IGameUpdatable
{
    private readonly float _velocity;

    [CanBeNull] private readonly Transform _owner;

    public BallMovement(Transform owner, float velocity)
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
