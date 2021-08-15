using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider2D))]
public class InteractionController : MonoBehaviour
{
    public LayerMask whatIsInteractable;
    public Collider2D m_collider2D;
    public AudioClip audioClip;
    public string[] textInfos;
    public GameObject interactableIcon;

    public bool isAudio = true;
    public bool isText = true;

    public float textFadeTime = 2f;

    public bool isInTrigger;

    AudioSource audioSource;

    int textIndex = 0;

    public TextBubble textBubble;

    bool canClick = true;

    [SerializeField] [Header("Mahdi- item - or - Document")]
    private GameObject Controller;
    [SerializeField] private Sprite itemImage;

    [SerializeField] private bool IsDocument;
    [SerializeField] private Sprite BackDoc;
    [SerializeField] private string NameDoc;
    [SerializeField] private string ShortInfo;
    [SerializeField] [TextArea] private string MainInfo;

    

    private string SceneName;

    GameDataController gameData;
    PlayerMovement playermovment;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if(audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.loop = false;
            audioSource.playOnAwake = false;
        }
        textBubble = FindObjectOfType<TextBubble>();
        
        //mahdi
        Controller = GameObject.Find("GameController");
        SceneName = SceneManager.GetActiveScene().name;
        
         gameData = FindObjectOfType<GameDataController>();
        if (!gameData)
        {
            GameObject gameDataController = new GameObject();
            gameDataController.AddComponent<GameDataController>();
        }

        playermovment = GameObject.FindObjectOfType<PlayerMovement>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playermovment.moveMode == MoveMode.run)
              return;


        if (!gameData.gameData.isOnCanvas)
        {
            if (isInTrigger && canClick)
            {
                CheckForClick();
                CheckForTouch();
            }
            if (this.tag != "Item")
                interactableIcon.SetActive(isInTrigger);
        }
    }

    void CheckForClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero, 0f, whatIsInteractable);

            if (hit)
            { 
                if (hit.collider.gameObject.name.Equals(gameObject.name))
                {
                    
                    Interact();

                    if (hit.transform.gameObject.tag == "Item")
                    {

                        if (!IsDocument)
                        {
                            if (itemImage == null)
                            {
                                GameObject.FindObjectOfType<InventoryManger>().PanelNew
                                    (this.name,GetComponent<SpriteRenderer>().sprite, "", false , gameObject);

                            }
                            else
                            {
                                GameObject.FindObjectOfType<InventoryManger>().PanelNew
                                    (this.name,itemImage, "", false , gameObject);
                            }       

                        }
                        else
                        {
                            GameObject.FindObjectOfType<InventoryManger>().
                                AddDocument(itemImage ,BackDoc, NameDoc, ShortInfo, MainInfo , gameObject);  
                           
                        }
                    }
                    
                    #region mahdi

                    if(SceneName == "Scene 2")
                    {
                        Controller.GetComponent<Scene2>().CheckTouch(this.name);
                    }
                       

                    if(SceneManager.GetActiveScene().name.Equals("Scene 3 FF"))
                        Controller.GetComponent<Scene3>().CheckTouch(this.name);

                    if(SceneName == "Scene 4-1 VIP Room - Dream")
                        Controller.GetComponent<VIPDream>().CheckTouch(this.name);

                    if (SceneName == "Scene 3-1 FF - Dream")
                        Controller.GetComponent<Scene3Dream>().CheckTouch(this.name);

                    if (SceneName == "Scene 3-2 SF - LongDream")
                        Controller.GetComponent<SFLong>().CheckTouch(this.name);

                    if (SceneName == "Scene 3 WC")
                        Controller.GetComponent<WC>().CheckTouch(this.name);

                    if (SceneName == "Scene 3 SF")
                        Controller.GetComponent<Scene3SF>().CheckTouch(this.name);

                    #endregion
                }

            }
        }
    }

    void CheckForTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Handle finger movements based on TouchPhase
            switch (touch.phase)
            {
                //When a touch has first been detected, change the message and record the starting position
                case TouchPhase.Began:
                    Vector2 origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero, 0f, whatIsInteractable);
                    if (hit)
                    {
                        if (hit.collider.gameObject.name.Equals(gameObject.name))
                        {
                            Interact();
                        }
                            
                    }
                    break;

                //Determine if the touch is a moving touch
                case TouchPhase.Moved:
                    break;

                case TouchPhase.Ended:
                    break;
            }
        }
    }

    public void Interact()
    {

       // if (!GameDataController.instance.gameData.isOnCanvas)
        {
            if (isAudio)
            {
                audioSource.Play();
            }

            if (isText)
            {
                StartCoroutine(TextInfoCoroutine());
                //StartCoroutine(TextInfoCoroutine());
            }
        }
    }

    IEnumerator TextInfoCoroutine()
    {
/*
        canClick = false;
        textBubble.transform.GetChild(0).gameObject.SetActive(true);
        textBubble.TypeText(textInfos[textIndex]);
        textIndex = (textIndex + 1) % textInfos.Length;
        yield return new WaitForSeconds(textFadeTime);
        textBubble.ClearText();
        textBubble.transform.GetChild(0).gameObject.SetActive(false);
        canClick = true;
*/

        canClick = false;
        textBubble.transform.GetChild(0).gameObject.SetActive(true);
        textBubble.SetDialog(textInfos[textIndex]);
        textIndex = (textIndex + 1) % textInfos.Length;


        yield return new WaitForSeconds(textFadeTime);
        textBubble.ClearText();
        textBubble.transform.GetChild(0).gameObject.SetActive(false);
        canClick = true;


        if (SceneName == "Scene 4 VIP Room")
            Controller.GetComponent<Scene4VIP>().EndInteraction(this.name);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            isInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            isInTrigger = false;
        }
    }
}
