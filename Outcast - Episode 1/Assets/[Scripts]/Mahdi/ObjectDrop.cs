using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectDrop : MonoBehaviour
{

  
    public string ItemNeed;

    [Header("Sound")]
    private AudioSource audiosource;
    public AudioClip SoundCorrect;
    [Range(0, 1)] public float VolumeCorrect;
    
    public AudioClip SoundUncorrect;
    [Range(0, 1)] public float VolumeUncorrect;

    private InventoryManger _inventoryManger;

    private Animator animator;
    public GameObject GreenCircle;
    public GameObject RedCircle;


    private int In;  // zero = empty   ,  1 = uncorrect   2 = correct
    

    void Start()
    {
        audiosource = GetComponent<AudioSource>();
        _inventoryManger = GameObject.FindObjectOfType<InventoryManger>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        if(Input.GetMouseButtonUp(0))
        {
            if(In == 1)
            {
                Soundplayer(SoundUncorrect, VolumeUncorrect);
            }

            if(In == 2)
            {

                Soundplayer(SoundCorrect, VolumeCorrect);

                if (ItemNeed == "KeyArtanRoom")
                   _inventoryManger.SpecialCombin(1);


                if(ItemNeed == "Battery")
                    _inventoryManger.SpecialCombin(3);


                if (ItemNeed == "ZeroKey")
                    _inventoryManger.SpecialCombin(4);



                _inventoryManger.item_drag_name = "";
                _inventoryManger.item_drop_name = "";

                gameObject.SetActive(false);
            }
        }


        /*
        if(Used)
        {
            this.GetComponent<ObjectDrop>().enabled = false;
            return;
        }


        Vector2 origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero, 0f);

        if (hit)
        {

            if (hit.collider.gameObject.name == (gameObject.name) && Input.GetMouseButtonUp(0))
            {
                if(GameObject.FindObjectOfType<InventoryManger>().item_drag_name == "KeyArtanRoom" &&
                    this.name == "Door4VIP")
                {

                    _inventoryManger.SpecialCombin(1);
                    _inventoryManger.item_drag_name = "";
                    _inventoryManger.item_drop_name = "";
                    Used = true;
                }

                if (GameObject.FindObjectOfType<InventoryManger>().item_drag_name == "Battery" &&
                      this.name == "Control TV")
                {
                    _inventoryManger.SpecialCombin(3);
                    _inventoryManger.item_drag_name = "";
                    _inventoryManger.item_drop_name = "";
                    Used = true;
                }

                if (GameObject.FindObjectOfType<InventoryManger>().item_drag_name == "ZeroKey" &&
                           this.name == "Door0")
                {
                    _inventoryManger.SpecialCombin(4);
                    _inventoryManger.item_drag_name = "";
                    _inventoryManger.item_drop_name = "";
                    Used = true;
                }


            }

        }




        */
    }

    private void Soundplayer (AudioClip _clip , float _volume)
    {
        audiosource.clip = _clip;
        audiosource.volume = _volume;
        audiosource.Play();
    }

    private void OnMouseEnter()
    {
        animator.Play("Empty");

        if(_inventoryManger.item_drag_name == ItemNeed)
        {
            // Green
            GreenCircle.SetActive(true);
            In = 2;
        }
         else if (_inventoryManger.item_drag_name.Equals("") || _inventoryManger.item_drag_name.Equals("null"))
         {
             // Null
         }
        else
        {
            // Red
            In = 1;
            RedCircle.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        // Normal
        In = 0;

        animator.Play("WantAnim");

        GreenCircle.SetActive(false);
        RedCircle.SetActive(false);
    }
}
