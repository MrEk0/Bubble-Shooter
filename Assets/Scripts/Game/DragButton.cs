using System;
using Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    [RequireComponent(typeof(RectTransform))]
    public class DragButton : MonoBehaviour, IDragHandler, IEndDragHandler, IServisable
    {
        [SerializeField] private float _maxDragDistance;

        public event Action<Vector2> EndDragEvent = delegate { };
        public event Action<Vector2> DragEvent = delegate { };

        private RectTransform _rect;
        private Vector3 _position;
        private Vector3 _direction;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
        }

        private void Start()
        {
            _position = _rect.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!RectTransformUtility.ScreenPointToWorldPointInRectangle(_rect, eventData.position, eventData.pressEventCamera, out var worldPoint))
                return;

            _direction = worldPoint - _position;
            var newVector = Vector3.ClampMagnitude(_direction, _maxDragDistance);
            _rect.position = new Vector3(_position.x + newVector.x, _position.y + newVector.y, 0f);

            DragEvent(_direction.normalized);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _rect.position = _position;

            EndDragEvent(_direction.normalized);
        }
    }
}
