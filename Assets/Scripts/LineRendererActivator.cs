using Game;
using Interfaces;
using UnityEngine;

public class LineRendererActivator : ISubscribable
{
    private LineRenderer _lineRenderer;
    private DragButton _dragButton;
    private Transform _owner;
    
    private float _timer;
    private bool _isActive;
    
    public LineRendererActivator(ServiceLocator serviceLocator, Transform owner, LineRenderer lineRenderer)
    {
        _lineRenderer = lineRenderer;
        _owner = owner;
        _dragButton = serviceLocator.GetService<DragButton>();

        _isActive = false;

        if (_lineRenderer != null)
            _lineRenderer.enabled = _isActive;
    }
    
    public void Subscribe()
    {
        _dragButton.DragEvent += OnDrag;
        _dragButton.EndDragEvent += OnEndDrag;
    }

    public void Unsubscribe()
    {
        _dragButton.DragEvent -= OnDrag;
        _dragButton.EndDragEvent -= OnEndDrag;
    }

    private void OnDrag(Vector2 direction)
    {
        _isActive = true;
        
        if (_lineRenderer == null)
            return;

        _lineRenderer.enabled = _isActive;

        var startPos = _owner.position;
        startPos.z = 0f;

        var hit = Physics2D.Raycast(startPos, -direction);
        var reflectedVector = Vector2.Reflect(-direction, hit.normal);

        _lineRenderer.positionCount = 3;

        _lineRenderer.SetPosition(0, startPos);
        _lineRenderer.SetPosition(1, hit.point);
        _lineRenderer.SetPosition(2, reflectedVector * 10f);
    }

    private void OnEndDrag(Vector2 direction)
    {
        _isActive = false;

        _lineRenderer.enabled = _isActive;
    }
}
