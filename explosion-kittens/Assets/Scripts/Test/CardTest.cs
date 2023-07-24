using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardTest : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IBeginDragHandler
{
    [SerializeField] private Transform _target;
    [SerializeField] private RectTransform localTarget;
    [SerializeField] private int speed = 10;
    [SerializeField] private LayoutElement _element ;
    private Vector3 targetPos;
    public Vector3 offset = Vector3.zero;
    public Vector3 rotation;
    private Canvas _canvas;
    private bool drag;
    private int targetIndex;
    private Transform _world;

    

    

    public void SetTarget(Transform target, Canvas canvas, Transform world)
    {
        targetIndex = transform.GetSiblingIndex();
        
        var localTargetGo = new GameObject();
        localTarget = localTargetGo.AddComponent<RectTransform>();
        localTargetGo.transform.SetParent(world);
        Canvas.ForceUpdateCanvases();
        localTargetGo.transform.position = target.position;
        localTargetGo.transform.localScale = Vector3.zero;

        _target = target;
        _canvas = canvas;
    }

    private void Update()
    {
        //if (_target == null) return;

        if (drag)
        {
            transform.position = Vector3.Lerp(transform.position, localTarget.position, Time.deltaTime * speed * 2) ;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * speed);

        }
        else
        {
            localTarget.transform.position = _target.position;

            transform.position = Vector3.Lerp(transform.position, localTarget.position + offset, Time.deltaTime * speed);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotation), Time.deltaTime * speed);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.SetSiblingIndex(transform.parent.childCount);
    }

    public void OnDrag(PointerEventData eventData)
    {
        localTarget.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * speed);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        drag = false;
        transform.SetSiblingIndex(targetIndex);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        drag = true;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform,
            Input.mousePosition, _canvas.worldCamera,
            out var movePos);

        
        localTarget.position = _canvas.transform.TransformPoint(movePos);

        //localTarget.anchoredPosition = eventData.position;
    }
}
