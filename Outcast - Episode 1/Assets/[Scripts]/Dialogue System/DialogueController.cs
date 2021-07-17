using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueController : MonoBehaviour
{
    

    public ConversationObject introConversation;
    public ConversationObject mainConversation;
    public ConversationObject exitConversation;

    public ConversationObject conversation;

    public GameObject ChoicePrefab;


    public Text DialogueText;

    public int lineIndex = 0;

    public Button next;

    public List<int> uncheckedLines = new List<int>();


    public string currentLineName;

    public Sprite BlackSprite;
    public Sprite RedSprite;

    DialogueInteraction dialogueInteraction;

    int conversationIndex = 0;

    //--------------
    [SerializeField] private Camera cam;
    [SerializeField] private float SpeedCamera;
    [SerializeField] private float MaxCameraZoom = 1.75f;
    [SerializeField] private float SpeedZoomCamera;
       

    private string CameraGoTo;
    private string CameraIn;
    [SerializeField] private GameObject [] characters;
    private Animator [] charactersAnimator = new Animator[2];

    // artan

    [SerializeField] private GameObject _Manger;
    private Step _step;

    //--------------
    [SerializeField] private ConversationObject jamshid1;
    [SerializeField] private ConversationObject jamshid2;

    private CharacterController2D characterController2d;




    private void Awake()
    {
        _step = GameObject.FindObjectOfType<Step>();
        characterController2d = GameObject.FindObjectOfType<CharacterController2D>();
    }
    void Start()
    {


        charactersAnimator[0] = characters[0]. transform.parent.GetComponent<Animator>();
        charactersAnimator[1] = characters[1].transform.parent.GetComponent<Animator>();

        #region dialog

        if (SceneManager.GetActiveScene().name == "Scene 3 FF")
        {

            if (!_step.Steps[7])
            {
                introConversation = jamshid1;
                mainConversation = jamshid1;
                exitConversation = jamshid1;
            }

            if (_step.Steps[10])
            {
                introConversation = jamshid2;
                mainConversation = jamshid2;
                exitConversation = jamshid2;
            } 
        }
       

        #endregion

        if (cam == null)
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
        cam.gameObject.SetActive(true);

        cam.transform.position = new Vector3
            (characters[0].transform.position.x, characters[0].transform.position.y, -10);

        cam.orthographicSize = 2.94f;


       
        conversation = introConversation;
        lineIndex = -1;
        UpdateName();
        NextLine();
       
    }

    // Update is called once per frame
    void Update()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, MaxCameraZoom, SpeedZoomCamera * Time.fixedDeltaTime);
        if(CameraGoTo != CameraIn)
        {
            if (cam.transform.position == characters[0].transform.position)
            {
                CameraIn = "Artan";
                return;
            }

            if (cam.transform.position == characters[1].transform.position)
            {
                CameraIn = "Jamshid";
                return;
            }


            if (CameraGoTo == "Artan")
            {
                cam.transform.position = Vector3.MoveTowards(cam.transform.position,
                    new Vector3(characters[0].transform.position.x , characters[0].transform.position.y,-10),
                    SpeedCamera * Time.fixedDeltaTime);
            }

            if (CameraGoTo == "Jamshid")
            {
                cam.transform.position = Vector3.MoveTowards(cam.transform.position,
                    new Vector3(characters[1].transform.position.x, characters[1].transform.position.y, -10),
                    SpeedCamera * Time.fixedDeltaTime);
            }

            if(CameraGoTo == "Lida")
            {
                cam.transform.position = Vector3.MoveTowards(cam.transform.position,
                   new Vector3(characters[1].transform.position.x, characters[1].transform.position.y, -10),
                  SpeedCamera * Time.fixedDeltaTime);
            }
        }
    }

    public void SetDialog (ConversationObject dialog)
    {
        introConversation = dialog;
        mainConversation = dialog;
        exitConversation = dialog;
    }

    public void ResetDialogue()
    {
        lineIndex = -1;
        currentLineName = "";
        uncheckedLines.Clear();
        UpdateName();
        NextLine();
    }

    public void NextLine()
    {

        if ( (lineIndex +1 ) >= conversation.lines.Length)
        {
            if (SceneManager.GetActiveScene().name == "Scene 3 FF")
            {
                _Manger.GetComponent<Scene3>().MarginClose();
                _Manger.GetComponent<Step>().DoWork(7);
                CloseDialogue();
                FindObjectOfType<GameDataController>().gameData.SetGameEventAsFinished("CantUpFloor");
            }
            else if (SceneManager.GetActiveScene().name == "Scene 3-1 FF - Dream")
            {
                _Manger.GetComponent<Scene3Dream>().MarginClose();
                CloseDialogue();
            }
            else if (SceneManager.GetActiveScene().name == "Scene 4 VIP Room")
            {
                _Manger.GetComponent<Scene4VIP>().MarginClose();
                CloseDialogue();
            }
            else if (SceneManager.GetActiveScene().name == "Scene 4-1 VIP Room - Dream")
            {
                _Manger.GetComponent<VIPDream>().MarginClose();
                CloseDialogue();
            }

            else if (SceneManager.GetActiveScene().name == "Scene 3 SF")
            {
                CloseDialogue();
                _Manger.GetComponent<Scene3SF>().MarginClose();
            }
            else if (SceneManager.GetActiveScene().name == "Scene 2")
            {
                CloseDialogue();
                _Manger.GetComponent<Scene2>().MarginClose();
            }

            return;
        }

        lineIndex++;

        if(mainConversation.lines[lineIndex].character.fullName == "آرتان")
        {
            CameraGoTo = "Artan";
        }

        if(mainConversation.lines[lineIndex].character.fullName == "متلدار")
        {
            CameraGoTo = "Jamshid";
        }

        if(mainConversation.lines[lineIndex].character.fullName == "ليدا")
        {
            CameraGoTo = "Lida";
        }


        if (conversation.lines.Length > lineIndex && !uncheckedLines.Contains(lineIndex))
        {
            DialogueText.text = conversation.lines[lineIndex].text;
            UpdateName();

            if (conversation.lines[lineIndex].choices != null && conversation.lines[lineIndex].choices.Length > 0)
            {
                for (int i = 0; i < conversation.lines[lineIndex].choices.Length; i++)
                {
                    if (CheckConditions(conversation.lines[lineIndex].choices[i]))
                    {
                        Choice _choice = conversation.lines[lineIndex].choices[i];
                        if (!_choice.hasViewed)
                        {
                            
                        }
                    }
                }
                //next.gameObject.SetActive(false);
            }
            else
            {
                if (conversation.lines[lineIndex].hasRecursion)
                {
                    print("recursiove");
                    lineIndex = conversation.lines[lineIndex].recursiveIndex - 1;
                    uncheckedLines.Clear();
                }
            }
        }
        else if(conversation.lines.Length <= lineIndex)
        {
            conversationIndex = (conversationIndex + 1) % 3;
            if(conversationIndex == 1)
            {
                lineIndex = -1;
                conversation = mainConversation;
                uncheckedLines.Clear();
                NextLine();
            }
            else if(conversationIndex == 2)
            {
                lineIndex = -1;
                conversation = exitConversation;
                uncheckedLines.Clear();
                NextLine();
            }
            else
                next.gameObject.SetActive(false);
        }


        if(lineIndex == 0)
        {
            if(characters[0].transform.position.x > characters[1].transform.position.x)
            {
                if (characterController2d.IsFacingRight())
                    characterController2d.Flip();
            }
            else
            {
                if (!characterController2d.IsFacingRight())
                    characterController2d.Flip();
            }
        }


        #region Special




        if (introConversation.name == "jamshid1")
        {
            if(lineIndex == 8)
            {
                _Manger.GetComponent<Scene3>().AllLightOff();
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
                StartCoroutine(WaitForNextDialog(2f));
                return;
            }

            if (lineIndex == 1)
            {
                charactersAnimator[1].Play("Artan_Idle_Anim_Usef(Edit)");
            }

            if(lineIndex == 9)
            {
                charactersAnimator[0].Play("Artan_LookUp");
                charactersAnimator[1].Play("Artan_HeadMovement");
            }

            if(lineIndex == 10)
            {
                charactersAnimator[0].Play("Idle");
            }

        }

        #endregion
    }

    bool CheckConditions(Choice choice)
    {
        if (choice.conditions != null)
        {
            bool ok = true;
            for(int i = 0; i < choice.conditions.conditions.Length; i++)
            {
                if (choice.conditions.conditions[i].OK)
                {
                    ok = ok && true;
                }
                else
                {
                    ok = ok && false;
                }
            }
            return ok;
        }
        return true;
    }

    void UpdateName()
    {
        /*
        if (!currentLineName.Equals(conversation.lines[lineIndex].character.fullName))
        {
            CharacterImage.sprite = conversation.lines[lineIndex].character.characterIcon;
            currentLineName = conversation.lines[lineIndex].character.fullName;
        }
        characterName.text = currentLineName;
        */

        //LeftCharacterImage.texture = conversation.leftCharacter.characterRenderTexture;
        //RightCharacterImage.texture = conversation.rightCharacter.characterRenderTexture;
    }

    public void SelectChoice(int lineIndex)
    {
        this.lineIndex = lineIndex - 1;
        NextLine();
        next.gameObject.SetActive(true);
    }

    public void SetDialogueInteraction(DialogueInteraction interaction)
    {
        dialogueInteraction = interaction;
    }

    public void CloseDialogue()
    {
        dialogueInteraction.OnDialogueEnded();
        gameObject.SetActive(false);
        cam.gameObject.SetActive(false);
    }

    IEnumerator WaitForNextDialog (float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
