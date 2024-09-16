using System;
using UnityEngine;

public interface ICollisionable
{
    public event Action<Collision2D> CollisionEvent;
}
