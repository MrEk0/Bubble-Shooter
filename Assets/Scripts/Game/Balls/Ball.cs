using System;
using Enums;
using UnityEngine;

namespace Game.Balls
{
    public class Ball : MonoBehaviour, ICollisionable
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public event Action<Collision2D> CollisionEvent = delegate { };

        public BallEnum Type { get; private set; }

        public void Init(Sprite sprite, BallEnum type)
        {
            _spriteRenderer.sprite = sprite;
            Type = type;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            CollisionEvent(col);
        }
    }
}
