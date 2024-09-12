using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class DragButton : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] private float _maxDragDistance;
    [SerializeField] private LineRendererActivator _lineRendererActivator;
    
    private RectTransform _rect;
    private Vector3 _position;

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

        var direction = worldPoint - _position;
        var newVector = Vector3.ClampMagnitude(direction, _maxDragDistance);
        var newPosition = new Vector3(_position.x + newVector.x, _position.y + newVector.y, 0f);
        _rect.position = newPosition;

        _lineRendererActivator.OnDrag(_position,  Camera.main.ScreenToWorldPoint(direction * 10f));//todo remove
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        _rect.position = _position;
        //todo shot event
    }
}
