using UnityEngine;
using UnityEngine.EventSystems;


public class drop : MonoBehaviour, IPointerEnterHandler , IPointerExitHandler 
{

    private InventoryManger manger;
    private bool In;

    private void Start()
    {
        manger = GameObject.FindObjectOfType<InventoryManger>();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
            In = true;
            manger.item_drop_name = this.name;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (In)
            In = false;

        manger.item_drop_name = "";
    }


    private void Update()
    {
        if(Input.GetMouseButtonUp(0) && In)
        {
            manger.transform.GetChild(0).GetChild(5).SetAsFirstSibling();

            manger.TryToCombin(this.name);

            manger.item_drag_name = "";
            manger.item_drop_name = "";
        }
    }
}
