using UnityEngine;

public class UiController : MonoBehaviour
{

    private Animator inventoryAnimator;
    private Animator btnInventoryAnimator;
    private Animator moveholderAnimator;

    private void Awake()
    {
       // DontDestroyOnLoad(gameObject);

        inventoryAnimator = GameObject.Find("Inventory").GetComponent<Animator>();
        btnInventoryAnimator = GameObject.Find("Inventorybtn").GetComponent<Animator>();
        moveholderAnimator = GameObject.Find("Move Holder - New").GetComponent<Animator>();

    }

    private void OnLevelWasLoaded()
    {
        inventoryAnimator = GameObject.Find("Inventory").GetComponent<Animator>();
        btnInventoryAnimator = GameObject.Find("Inventorybtn").GetComponent<Animator>();
        moveholderAnimator = GameObject.Find("Move Holder - New").GetComponent<Animator>();
    }

    public void ShowOff ()
    {
        inventoryAnimator.Play("InventoryMangerAnimation");
        btnInventoryAnimator.Play("BtnInventory");
        moveholderAnimator.Play("MoveHolderAnimation");
    }

    public void Show ()
    {
        inventoryAnimator.Play("InventoryMangerAnimationOff");
        btnInventoryAnimator.Play("BtnInventoryOff");
        moveholderAnimator.Play("MoveHolderAnimationOff");
    }

}
