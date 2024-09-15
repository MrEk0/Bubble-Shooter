using System;
using System.Linq;
using Enums;
using Interfaces;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Configs
{
    [CreateAssetMenu(menuName = "Configs/GameSettingsData")]
    public class GameSettingsData : ScriptableObject, IServisable
    {
        [Serializable]
        public class BallSpriteSettings
        {
            public BallEnum Type;
            public Sprite Sprite;
        }
        
        [SerializeField] private int _scorePerBall;
        [FormerlySerializedAs("_ballRadius")] [SerializeField] private float ballSize;
        [SerializeField] private int _levelRowCounts;
        [SerializeField] private int _levelColumnCounts;
        [SerializeField] private Vector2 _ballSpacing;
        [SerializeField] private Vector2 _startPositionOffset;
        [SerializeField] private BallSpriteSettings[] _ballSpriteSettings = Array.Empty<BallSpriteSettings>();

        public int ScorePerBall => _scorePerBall;
        public float BallSize => ballSize;
        public int LevelRowCounts => _levelRowCounts;
        public int LevelColumnCounts => _levelColumnCounts;
        public Vector2 BallSpacing => _ballSpacing;
        public Vector2 StartPositionOffset => _startPositionOffset;

        [CanBeNull]
        public Sprite GetBallSprite(BallEnum type)
        {
            var ballSettings = _ballSpriteSettings.FirstOrDefault(o => o.Type == type);
            return ballSettings?.Sprite;
        }
    }
}
