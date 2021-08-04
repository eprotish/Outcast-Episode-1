using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManger : MonoBehaviour
{
    public GameObject QuickInventory;
    public GameObject InfoButton;

    [SerializeField] private AudioClip DocRotSound;
    [SerializeField] [Range(0, 1)] private float DocRotVolume;
    [SerializeField] private AudioClip PickNewItemSound;
    [SerializeField] [Range(0, 1)] private float PickNewItemVolume;


    private int numberItemInInvenory = 0;
    private int numberDocInInventory = 0;
    private string [] inventory = new string[6];
    [SerializeField] private Image [] slots;
    [SerializeField] public string item_drag_name;
    [SerializeField] public string item_drop_name;
    [SerializeField] private Color empty_color;
    [SerializeField] private Color normal_color;
    [SerializeField] private CombinItems[] _combinItems;
    [SerializeField] private GameObject info;

    // document
    private Document [] documents = new Document[100];
    private int PageInDoc = 0;
    [SerializeField] private Text NameDocShower;
    [SerializeField] private Text NameDocShower2;
    [SerializeField] private Text ShortInfoShower;
    [SerializeField] private Image ImageShower;
    [SerializeField] private Image ImageShower2;
    [SerializeField] private Text MainInfoShower;
    [SerializeField] private Text NumberShower;
    [SerializeField] private GameObject ReadButton;

    [SerializeField] private GameObject QI_Read;

    [SerializeField] private Documents[] AllDocument;
    [SerializeField] private AudioSource audioSource;

    private GameDataController gamedata;

    private bool isFront = true;
    private bool isChangeImage = true;
    [SerializeField] private float Speed;
    [SerializeField] private GameObject NewItemPanel;
    [SerializeField] private Text NameItemNewShower;
    [SerializeField] private Image SpriteItemNewShower;
    [SerializeField] private Text ShortInfoNewShower;

    //[SerializeField] private GameObject BackGround;

    GameObject DestroyItem;
    Step _step;

    [HideInInspector] public bool OnQuickInventory;

    private GameObject inventoryBtnObj;

    private Animator moveHolder;

    private void Start()
    {
        _step = GameObject.FindObjectOfType<Step>();
        gamedata = GameObject.FindObjectOfType<GameDataController>();

        inventoryBtnObj = GameObject.Find("Inventorybtn");

        moveHolder = GameObject.Find("Move Holder - New").GetComponent<Animator>();
    }

    public void AddItem (string itemName,Sprite itemImage)
    {

        if (numberItemInInvenory >= 6)
        {
            // inventory is full
            return;
        }

        audioSource.clip = PickNewItemSound;
        audioSource.volume = PickNewItemVolume;
        audioSource.Play();

        // add to inventory


        Invoke("CloseInventoryByDelay", 0.5f);


        #region Step

        if (itemName == "Fuse")
        {
            _step.DoWork(8);
        }

        if (itemName == "Tape")
        {
            _step.DoWork(21);
        }

        if (itemName == "BookR")
        {
            _step.DoWork(22);
        }

        if(itemName == "Battery")
        {
            _step.DoWork(24);
        }

        if (itemName == "Zero Key")
        {
            _step.DoWork(33);
        }

        #endregion

        inventory[numberItemInInvenory] = itemName;
        slots[numberItemInInvenory].name = itemName;
        slots[numberItemInInvenory].sprite = itemImage;
        slots[numberItemInInvenory].transform.parent.name = itemName;
        slots[numberItemInInvenory].color = normal_color;

        numberItemInInvenory++;

        GameDataController.instance.gameData.AddItem(itemName);
    }

    public void AddItemFromLoad(string itemName, Sprite itemImage)
    {

        if (numberItemInInvenory >= 6)
        {
            // inventory is full
            return;
        }

        // add to inventory

        inventory[numberItemInInvenory] = itemName;
        slots[numberItemInInvenory].name = itemName;
        slots[numberItemInInvenory].sprite = itemImage;
        slots[numberItemInInvenory].transform.parent.name = itemName;
        slots[numberItemInInvenory].color = normal_color;

        numberItemInInvenory++;
    }

    public void RemoveItem (string itemName)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == itemName)
            {
                inventory[i] = null;
                slots[i].sprite = null;
                slots[i].color = empty_color;
                slots[i].transform.parent.name = null;
                slots[i].name = null;
            }
        }
        GameDataController.instance.gameData.RemoveItem(itemName);
    }

    public void AddDocument(Sprite docImage, Sprite Backdoc , string docTitle , string docInfo,string MainInfo, GameObject a)
    {
        if(a != null)
           a.SetActive(false);


        numberDocInInventory++;
        documents[numberDocInInventory].ShortInfo = docInfo;
        documents[numberDocInInventory].nameDocument = docTitle;
        documents[numberDocInInventory].ImageDocument = docImage;
        documents[numberDocInInventory].infoDocument = MainInfo;
        GameDataController.instance.gameData.AddItem(docTitle);
        documents[numberDocInInventory].BackDoc = Backdoc;


        Invoke("CloseInventoryByDelay", 0.5f);

        if (numberDocInInventory == 1)
            PageInDoc = 1;

        DocumentShow();

        if (docTitle == "NewsPaper")
        {
            _step.DoWork(29);
        }
    }

    public void AddDocumentFromLoad(Sprite docImage, Sprite Backdoc , string docTitle, string docInfo, string MainInfo)
    {
        numberDocInInventory++;
        documents[numberDocInInventory].infoDocument = docInfo;
        documents[numberDocInInventory].nameDocument = docTitle;
        documents[numberDocInInventory].ImageDocument = docImage;
        documents[numberDocInInventory].infoDocument = MainInfo;
        documents[numberDocInInventory].BackDoc = Backdoc;

        DocumentShow();
    }

    public void TryToCombin(string dropName)
    {

        item_drop_name = dropName;

        for (int i = 0; i < _combinItems.Length; i++)
        {
            if (item_drag_name == _combinItems[i].item1.name && item_drop_name == _combinItems[i].item2.name)
            {
                RemoveItem(_combinItems[i].item1.name);
                RemoveItem(_combinItems[i].item2.name);
                AddItem(_combinItems[i].result.name,_combinItems[i].result);
                GameDataController.instance.gameData.Combine(_combinItems[i].item1.name, _combinItems[i].item2.name, _combinItems[i].result.name);
                Invoke("CloseInventoryByDelay", 0.5f);
            }
            
            if (item_drag_name == _combinItems[i].item2.name && item_drop_name == _combinItems[i].item1.name)
            {
                RemoveItem(_combinItems[i].item1.name);
                RemoveItem(_combinItems[i].item2.name);
                AddItem(_combinItems[i].result.name,_combinItems[i].result);
                GameDataController.instance.gameData.Combine(_combinItems[i].item1.name, _combinItems[i].item2.name, _combinItems[i].result.name);
                Invoke("CloseInventoryByDelay", 0.5f);
            }
        }

        #region Special

        if (item_drag_name == "Fuse" && item_drop_name == "FusePlace")
        {
            RemoveItem("Fuse");
            GameObject.FindObjectOfType<Step>().DoWork(9);
            GameObject.FindObjectOfType<Scene2>().FuseCheck();

            Invoke("CloseInventoryByDelay", 0.5f);
        }

        else if (item_drag_name == "Tape" && item_drop_name == "BookR")
        {


            RemoveItem("Tape");
            RemoveItem("BookR");
            AddDocument(AllDocument[0].ImageDocument, AllDocument[0].BackDoc, AllDocument[0].nameDocument, AllDocument[0].ShortInfo, AllDocument[0].infoDocument, null);

            Invoke("CloseInventoryByDelay", 0.5f);

        }

        if (item_drag_name == "BookR" && item_drop_name == "Tape")
        {

            RemoveItem("Tape");
            RemoveItem("BookR");
            AddDocument(AllDocument[0].ImageDocument, AllDocument[0].BackDoc, AllDocument[0].nameDocument, AllDocument[0].ShortInfo, AllDocument[0].infoDocument, null);

            Invoke("CloseInventoryByDelay", 0.5f);
        }




        #endregion
    }

    public void SpecialCombin (int a)
    {

        if (a == 1)
        {
            RemoveItem("KeyArtanRoom");
            GameObject.FindObjectOfType<Step>().DoWork(12);
            GameObject.FindObjectOfType<Scene3SF>().KeyUsed();
            Invoke("CloseInventoryByDelay", 0.5f);
        }

        if (a == 2)
        {
            RemoveItem("BookR");
            RemoveItem("Tape");
            Invoke("CloseInventoryByDelay", 0.5f);
        }

        if (a == 3)
        {
            RemoveItem("Battery");
            GameObject.FindObjectOfType<VIPDream>().ControlFully();
            Invoke("CloseInventoryByDelay", 0.5f);
        }

        if(a == 4)
        {
            RemoveItem("Zero Key");
            GameObject.FindObjectOfType<SFLong>().Door0OPen();
            Invoke("CloseInventoryByDelay", 0.5f);
        }
    }
    
    public void inventoryBtn()
    {
        if(QuickInventory.activeInHierarchy)
        {
            info.SetActive(false);
            QuickInventory.SetActive(false);
            OnQuickInventory = false;
            InfoButton.SetActive(false);

            gamedata.gameData.isOnCanvas = false;

            moveHolder.Play("MoveHolderAnimationOff");
        }
        else
        {
            QuickInventory.SetActive(true);
            OnQuickInventory = true;
            InfoButton.SetActive(true);
            info.SetActive(false);

            gamedata.gameData.isOnCanvas = true;

            moveHolder.Play("MoveHolderAnimation");
        }
    }
    
    public void infoBtn()
    {
        if(info.activeInHierarchy)
        {
            info.SetActive(false);
        }
        else
        {
            info.SetActive(true);
            DocumentShow();
        }
    }

    public void Read ()
    {
        QI_Read.SetActive(true);
        inventoryBtnObj.SetActive(false);
        
    }

    public void Flip ()
    {
        float Angle = ImageShower.transform.eulerAngles.y;

        if( (Angle <= 0.1f && Angle >= -0.1f ) || (Angle <= 180.1f && Angle >= 179.9f))
        {
            audioSource.clip = DocRotSound;
            audioSource.volume = DocRotVolume;
            audioSource.Play();
            isFront = !isFront;
            isChangeImage = false;
        }
    }

    public void CloseRead ()
    {
        QI_Read.SetActive(false);
        inventoryBtnObj.SetActive(true);
    }
    public void nextBtn()
    {
        if (PageInDoc < numberDocInInventory)
        {
            PageInDoc++;
        }
        
        DocumentShow();
    }

    public void backBtn()
    {
        if (PageInDoc > 1)
        {
            PageInDoc--;
        }
        
        DocumentShow();
    }

    private void DocumentShow()
    {
        NumberShower.text = PageInDoc.ToString() + "/" + numberDocInInventory.ToString(); 

        if (numberDocInInventory == 0)
        {
            ImageShower2.color = empty_color;
            PageInDoc = 0;
            ImageShower.color = empty_color;
            ShortInfoShower.text = "هیچ مدرکی وجود ندارد";
            NameDocShower.text = "";
            NameDocShower2.text = "";
            ReadButton.SetActive(false);
        }
        else
        {
            ImageShower2.color = normal_color;
            ImageShower2.sprite = documents[PageInDoc].ImageDocument; 
            ReadButton.SetActive(true);
            ImageShower.color = normal_color;
            NameDocShower.text = documents[PageInDoc].nameDocument;
            NameDocShower2.text = documents[PageInDoc].nameDocument;
            ShortInfoShower.text = documents[PageInDoc].ShortInfo;
            MainInfoShower.text = documents[PageInDoc].infoDocument;
            ImageShower.sprite = documents[PageInDoc].ImageDocument;
        }
        
        
    }

    private void Update()
    {
        if(!isFront && ImageShower.transform.eulerAngles.y < 180)
        {
            ImageShower.transform.Rotate(0, Speed, 0);
        }


        if (isFront && ImageShower.transform.eulerAngles.y > 0)
        {
            ImageShower.transform.Rotate(0, Speed * -1, 0);
        }


        if (ImageShower.transform.eulerAngles.y >= 90 && !isFront && !isChangeImage)
        {
            isChangeImage = true;
            ImageShower.sprite = documents[PageInDoc].BackDoc;
        }

        if (ImageShower.transform.eulerAngles.y <= 90 && isFront && !isChangeImage)
        {
            isChangeImage = true;
            ImageShower.sprite = documents[PageInDoc].ImageDocument;
        }

    }

    public void PanelNew (string nameItem , Sprite spriteItem , string Shortinfo , bool isDoc , GameObject DestroyMeOrNo)
    {
        DestroyItem = DestroyMeOrNo;
        NewItemPanel.SetActive(true);

        NameItemNewShower.text = nameItem;
        SpriteItemNewShower.sprite = spriteItem;


         /*
        if (isDoc)
            ShortInfoNewShower.text = Shortinfo;
        else
            ShortInfoNewShower.text = "";


        */
        if(!QuickInventory.activeInHierarchy)
        {
            QuickInventory.SetActive(true);
            InfoButton.SetActive(true);
            info.SetActive(false);
        }
    }

    public void Pick ()
    {
        if(NewItemPanel.activeInHierarchy)
        {
            AddItem(NameItemNewShower.text, SpriteItemNewShower.sprite);

            if (DestroyItem != null)
                DestroyItem.SetActive(false);
        }
    }

    public void CloseInventoryByDelay ()
    {
        info.SetActive(false);
        QuickInventory.SetActive(false);
        OnQuickInventory = false;
        InfoButton.SetActive(false);

        gamedata.gameData.isOnCanvas = false;     
    }
}

[Serializable] public struct Documents
{
    public string nameDocument;
    public Sprite ImageDocument;
    public Sprite BackDoc;
    public string ShortInfo;
    [TextArea] public string infoDocument;
}

[Serializable] public struct CombinItems
{
    public Sprite item1;
    public Sprite item2;
    public Sprite result;
}

[Serializable] public  struct Document
{
    public string nameDocument;
    public Sprite ImageDocument;
    public Sprite BackDoc;
    public string ShortInfo;
    [TextArea] public string infoDocument;
}

