using UnityEngine;
using UnityEngine.Serialization;

namespace Configs
{
    [CreateAssetMenu(menuName = "Configs/GameSettingsData")]
    public class GameSettingsData : ScriptableObject
    {
        [SerializeField] private int _scorePerBall;
        [SerializeField] private float _playerVelocityTimeRate;
        [SerializeField] private int _playerAngleVelocity;
        [SerializeField] private int _maxLaserShotCount;
        [SerializeField] private float _laserDuration;
        [SerializeField] private float _laserRecoveryTime;
        [SerializeField] private float _asteroidSpawnPause;
        [SerializeField] private float _ufoSpawnPause;
        [SerializeField] private int _asteroidOnBulletHitCount;
        [SerializeField] private int _coinsForAsteroid;
        [SerializeField] private int _coinsForSmallAsteroid;
        [SerializeField] private int _coinsForUFO;
        [SerializeField] private float _enemyExtraBoundsMultiplayer;
        [SerializeField] private Vector2 _asteroidAttackAngleRange;
        [SerializeField] private Vector2 _smallAsteroidAttackAngleRange;
        [SerializeField] private LayerMask _playerAttackMask;

        public int ScorePerBall => _scorePerBall;
        public float PlayerVelocityTimeRate => _playerVelocityTimeRate;
        public int PlayerAngleVelocity => _playerAngleVelocity;
        public int MaxLaserShotCount => _maxLaserShotCount;
        public float LaserDuration => _laserDuration;
        public float LaserRecoveryTime => _laserRecoveryTime;
        public float AsteroidSpawnPause => _asteroidSpawnPause;
        public float UFOSpawnPause => _ufoSpawnPause;
        public int AsteroidOnBulletHitCount => _asteroidOnBulletHitCount;
        public int CoinsForAsteroid => _coinsForAsteroid;
        public int CoinsForSmallAsteroid => _coinsForSmallAsteroid;
        public int CoinsForUFO => _coinsForUFO;
        public float EnemyExtraBoundsMultiplayer => _enemyExtraBoundsMultiplayer;
        public Vector2 AsteroidAttackAngleRange => _asteroidAttackAngleRange;
        public Vector2 SmallAsteroidAttackAngleRange => _smallAsteroidAttackAngleRange;
        public LayerMask PlayerAttackMask => _playerAttackMask;
    }
}
