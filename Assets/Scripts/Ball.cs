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

    private void OnCollisionEnter2D(Collision2D col)
    {
        var point = col.GetContact(0);
        var direction = Vector2.Reflect(transform.up, point.normal);
        
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        var rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        
        transform.rotation = rotation;
    }
}
