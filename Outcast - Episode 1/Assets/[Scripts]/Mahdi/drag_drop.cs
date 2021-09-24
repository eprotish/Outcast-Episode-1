using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class drag_drop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas _can;
    private Vector2 _orgin_pos;
    private RectTransform _rec;
    private InventoryManger inventorymanger;

    public bool DontDarg;

    private void Awake()
    {
         if(_can == null)
         {
             _can = transform.parent.parent.parent.parent.GetComponent<Canvas>();
         }
     
        _rec = GetComponent<RectTransform>();
        _orgin_pos = _rec.anchoredPosition;
        inventorymanger = GameObject.FindObjectOfType<InventoryManger>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        inventorymanger.item_drag_name = this.name;
        transform.parent.transform.SetAsLastSibling();

        GetComponent<Image>().raycastTarget = false;

        if (DontDarg)
        {
            if (transform.name == "book")
            {
                inventorymanger.ShowBookDoc();
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _rec.anchoredPosition = _orgin_pos;
            GetComponent<Image>().raycastTarget = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (this.GetComponent<Image>().sprite != null && !DontDarg)
        {
            _rec.anchoredPosition += eventData.delta / _can.scaleFactor;
        }
    }
}
