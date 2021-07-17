using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class drag_drop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas _can;
    private Vector2 _orgin_pos;
    private RectTransform _rec;
    private InventoryManger inventorymanger;

    private void Awake()
    {
        _rec = GetComponent<RectTransform>();
        _orgin_pos = _rec.anchoredPosition;
        inventorymanger = GameObject.FindObjectOfType<InventoryManger>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        inventorymanger.item_drag_name = this.name;
        transform.parent.transform.SetAsLastSibling();

        GetComponent<Image>().raycastTarget = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _rec.anchoredPosition = _orgin_pos;
        GetComponent<Image>().raycastTarget = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (this.GetComponent<Image>().sprite != null)
        {
            _rec.anchoredPosition += eventData.delta / _can.scaleFactor;
        }
    }
}
