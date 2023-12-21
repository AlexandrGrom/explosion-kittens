using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardTest : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IBeginDragHandler, IDropHandler
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
    private Vector3 targetScale = Vector3.one;

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
        //
        //
        // Debug.Log(transform.localEulerAngles);
        // Debug.Break();
    }
 
    private void Update()
    {
        if (drag)
        {
            if (tolerantDrag)
            {
                transform.position = Vector3.Lerp(transform.position, localTarget.position, Time.deltaTime * speed * 2) ;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * speed);
            }
        }
        else
        {
            localTarget.transform.position = _target.position;

            transform.position = Vector3.Lerp(transform.position, localTarget.position + offset, Time.deltaTime * speed);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotation), Time.deltaTime * speed);
        }

        transform.localScale = Vector3.Lerp(transform.localScale, targetScale,Time.deltaTime * speed);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.SetSiblingIndex(transform.parent.childCount);
        targetScale = Vector3.one * 1.25f;
    }

    private bool tolerantDrag;
    public void OnDrag(PointerEventData eventData)
    {
        localTarget.anchoredPosition += eventData.delta / _canvas.scaleFactor;

        if (Vector3.Distance(transform.position,localTarget.position) < (1 * eventData.delta.magnitude))
        {
            tolerantDrag = true;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, localTarget.position, Time.deltaTime * speed * 2);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * speed);
        }
    }

    

    public void OnPointerUp(PointerEventData eventData)
    {
        drag = false;
        targetScale = Vector3.one;

        transform.SetSiblingIndex(targetIndex);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        tolerantDrag = false;
        drag = true;

        targetScale = Vector3.one;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform,
            Input.mousePosition, _canvas.worldCamera,
            out var movePos);
        
        localTarget.position = _canvas.transform.TransformPoint(movePos);
    }

    public void OnDrop(PointerEventData eventData)
    {
        
    }
}
