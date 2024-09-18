using Configs;
using Interfaces;
using UnityEngine;

namespace Game
{
    public class LineRendererActivator : ISubscribable
    {
        private readonly LineRenderer _lineRenderer;
        private readonly DragButton _dragButton;
        private readonly Transform _owner;

        private Vector2Int _aimLineAngleRange;
        private float _timer;
        private bool _isActive;

        public LineRendererActivator(ServiceLocator serviceLocator, DragButton dragButton, Transform owner, LineRenderer lineRenderer)
        {
            _lineRenderer = lineRenderer;
            _owner = owner;
            _dragButton = dragButton;
            _aimLineAngleRange = serviceLocator.GetService<GameSettingsData>().AimLineAngleRange;

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

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
            if ((angle >= 0f && angle > _aimLineAngleRange.x) || (angle < 0f && angle < _aimLineAngleRange.y))
                return;

            _lineRenderer.enabled = _isActive;

            var startPos = _owner.position;
            startPos.z = 0f;
            
            _lineRenderer.positionCount = 2;
            
            var hit = Physics2D.Raycast(startPos, -direction);
            _lineRenderer.SetPosition(0, startPos);
            _lineRenderer.SetPosition(1, hit.point);
        }

        private void OnEndDrag(Vector2 direction)
        {
            _isActive = false;

            _lineRenderer.enabled = _isActive;
        }
    }
}
