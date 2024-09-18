using System;
using System.Linq;
using Enums;
using Interfaces;
using JetBrains.Annotations;
using UnityEngine;

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

        [SerializeField] private string _linkedInURL;
        [SerializeField] private int _scorePerBall;
        [SerializeField] private int _levelShotsCount;
        [SerializeField] private int _minBallsCountToRelease;
        [SerializeField] private float _ballVelocity;
        [SerializeField] private int _levelRowCounts;
        [SerializeField] private Vector2 _ballSpacing;
        [SerializeField] private Vector2 _startPositionOffset;
        [SerializeField] private LayerMask _wallMask;
        [SerializeField] private LayerMask _ballMask;
        [SerializeField] private BallSpriteSettings[] _ballSpriteSettings = Array.Empty<BallSpriteSettings>();

        public string LinkedInURL => _linkedInURL;
        public int ScorePerBall => _scorePerBall;
        public int LevelShotsCount => _levelShotsCount;
        public int MinBallsCountToRelease => _minBallsCountToRelease;
        public float BallVelocity => _ballVelocity;
        public int LevelRowCounts => _levelRowCounts;
        public Vector2 BallSpacing => _ballSpacing;
        public Vector2 StartPositionOffset => _startPositionOffset;
        public LayerMask WallMask => _wallMask;
        public LayerMask BallMask => _ballMask;

        [CanBeNull]
        public Sprite GetBallSprite(BallEnum type)
        {
            var ballSettings = _ballSpriteSettings.FirstOrDefault(o => o.Type == type);
            return ballSettings?.Sprite;
        }
    }
}
