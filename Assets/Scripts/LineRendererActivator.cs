using UnityEngine;

public class LineRendererActivator : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    // [CanBeNull] private readonly ILaserAttackable _attacker;
    //
    // private readonly float _duration;
    //
    // private float _timer;
    // private bool _isActive;
    //
    // public LineRendererActivator(ServiceLocator serviceLocator, ILaserAttackable attacker, LineRenderer lineRenderer)
    // {
    //     _lineRenderer = lineRenderer;
    //     _attacker = attacker;
    //
    //     _isActive = false;
    //     _duration = serviceLocator.GetService<GameSettingsData>().LaserDuration;
    //
    //     if (_lineRenderer != null)
    //         _lineRenderer.enabled = _isActive;
    // }

    public void OnDrag(Vector2 initPosition, Vector2 direction)
    {
        if (_lineRenderer == null)
            return;

        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, (Vector2)transform.position + direction);
    }
}
