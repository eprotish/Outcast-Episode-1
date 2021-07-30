using UnityEngine;
using UnityEngine.EventSystems;


public class drop : MonoBehaviour, IDropHandler
{

    private InventoryManger manger;

    private void Start()
    {
        manger = GameObject.FindObjectOfType<InventoryManger>();
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        print("Ok");

        manger.transform.GetChild(0).GetChild(5).SetAsFirstSibling();

        manger.TryToCombin(this.name);

        manger.item_drag_name = "";
        manger.item_drop_name = "";
    }

    
}
