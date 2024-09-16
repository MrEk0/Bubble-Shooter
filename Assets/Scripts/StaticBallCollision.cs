using System;
using JetBrains.Annotations;
using UnityEngine;

public class StaticBallCollision : ObjectCollision
{
    private readonly int _ballMask;

    [CanBeNull] private readonly Action<Collision2D> _collisionCallback;

    public StaticBallCollision(ICollisionable owner, int ballMask, Action<Collision2D> collisionCallback) : base(owner)
    {
        _ballMask = ballMask;
        _collisionCallback = collisionCallback;
    }
    
    protected override void OnCollision(Collision2D collision)
    {
        if (_ballMask != (_ballMask | (1 << collision.gameObject.layer)))
            return;
        
        _collisionCallback?.Invoke(collision);
    }
}
