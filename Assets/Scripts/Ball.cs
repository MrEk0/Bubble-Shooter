using UnityEngine;
using UnityEngine.Pool;

public class Ball : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    private IObjectPool<Ball> _objectPool;

    public IObjectPool<Ball> ObjectPool
    {
        set => _objectPool = value;
    }

    public void Init(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }
}
