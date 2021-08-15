using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MoveHolderController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int buttonCode;
    public GameObject run;
    PlayerMovement move;
    GameObject inventorybtn;

    private InventoryManger inventoryManger;
    void Start()
    {
        if (run)
            run.SetActive(false);
        move = FindObjectOfType<PlayerMovement>();

        inventorybtn = GameObject.Find("Inventorybtn");
        inventoryManger = GameObject.FindObjectOfType<InventoryManger>();
        run = GameObject.Find("Move Holder - New").transform.GetChild(2).gameObject;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
          if(inventoryManger.OnQuickInventory)
                return;
          
        Color holdColor = GetComponent<Image>().color;
        holdColor.a = 0.5f;
        GetComponent<Image>().color = holdColor;

        if (buttonCode == 0 && !move.InInteratcion)  //moveLeft    // moghee ke to interaction nartone rah bere
        {
            if(move.CantMoveLeft && inventoryManger.OnQuickInventory) 
                return;
               

            move.MoveLeft();
            run.SetActive(true);
        }
        if (buttonCode == 1 && !move.InInteratcion) //moveRight    // moghee ke to interaction nartone rah bere
        {
            if(move.CantMoveRight && inventoryManger.OnQuickInventory)
                return;
               

            move.MoveRight();
            run.SetActive(true);
        }
        if (buttonCode == 2) //run
        {
            if(inventoryManger.OnQuickInventory)
            return;

            
            move.Run();
            inventorybtn.SetActive(false);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Color holdColor = GetComponent<Image>().color;
        holdColor.a = 1f;
        GetComponent<Image>().color = holdColor;

        if(buttonCode == 0 || buttonCode == 1)
        {
            run.SetActive(false);
            move.Stop();
        }

        if(buttonCode == 2)
        {
            move.RunStop();
            inventorybtn.SetActive(true);
        }

    }

    public void HideRunButton () {

        run.SetActive(false);
    }
}
