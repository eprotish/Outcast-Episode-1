using UnityEngine;
using System.Collections;


public class Scene3SF : MonoBehaviour
{
    [SerializeField] private AudioClip SoundWear;
    [SerializeField] [Range(0, 1)] private float VolumeWear = 0.5f;
    [SerializeField] private AudioSource SoundPlayer;


    [SerializeField] private float TimeWear = 5f;
    private Step _step;
    [SerializeField] private GameObject Lida;
    [SerializeField] private GameObject DoorRoom4VIP;
    [SerializeField] private Animator Margin;
    [SerializeField] private GameObject moveHolder;
    [SerializeField] private GameObject Artan;
    [SerializeField] private ConversationObject dialog;


    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource music2;
    [SerializeField] private float SpeedVolumeDown;
    [SerializeField] private float SpeedVolumeUp;

    private bool LidaCanGo;
    [SerializeField] private float LidaMovementSpeed;

    [SerializeField] private GameObject Door4Interaction;
    [SerializeField] private GameObject WearInteraction;
    [SerializeField] private GameObject ColliderBeforeWear;
    [SerializeField] private GameObject TriggerDialog;

    [SerializeField] private Animator PanelFade;

    private bool BeforeWear = false;
    private bool AfterWear = false;
    
    void Start()
    {

        #region Steps
        _step = GameObject.FindObjectOfType<Step>();

        if(!_step.Steps[12])
        {
            Door4Interaction.SetActive(false);
            DoorRoom4VIP.SetActive(true);
        }
        else
        {
            Door4Interaction.SetActive(true);
            DoorRoom4VIP.SetActive(false);
        }           

        if(_step.Steps[36] && !_step.Steps[37])
        {
            _step.DoWork(37);
        }

        if(_step.Steps[37] && !_step.Steps[38])
        {
            Lida.SetActive(true);
            TriggerDialog.SetActive(true);
        }

        if(_step.Steps[38])
        {
            Door4Interaction.SetActive(false);
            WearInteraction.SetActive(true);
        }

        if(_step.Steps[36] && !_step.Steps[39])
        {
            ColliderBeforeWear.SetActive(true);
        }

        #endregion
    }

    public void CheckTrigger (string a)
    {
        if(a == "TriggerDialog")
        {
            MarginOpen();
        }
    }

    public void CheckTouch(string a)
    {
        if(a == "InteractionLida")
        {
            MarginOpen();
        }

        if(a == "InteractionWear")
        {
            // 
            ColliderBeforeWear.SetActive(false);
            PanelFade.gameObject.SetActive(true);
            PanelFade.SetBool("Show", true);
            BeforeWear = true;
            StartCoroutine(WaitForWear(7f));
        }
    }

    public void MarginOpen()
    {
        Margin.gameObject.SetActive(true);
        Margin.SetBool("Show", true);
        moveHolder.SetActive(false);

        GameObject.FindObjectOfType<DialogueInteraction>().OnDialogueStarted
            (Artan);
        GameObject.FindObjectOfType<DialogueController>().SetDialog(dialog);

        GameObject.FindObjectOfType<PlayerMovement>().RunStop();
        GameObject.FindObjectOfType<PlayerMovement>().Stop();

    }
    public void MarginClose()
    {
        Lida.transform.eulerAngles = new Vector3(0, 180, 0);
        LidaCanGo = true;
        StartCoroutine(WaitThisFuncation(5f));

        _step.DoWork(38);
        Door4Interaction.SetActive(false);
        WearInteraction.SetActive(true);
        moveHolder.SetActive(false);
    }

    IEnumerator WaitThisFuncation (float wait)
    {
        yield return new WaitForSeconds(wait);
        Margin.SetBool("Show", false);
        moveHolder.SetActive(true);
    }


    IEnumerator WaitForWear (float wait)
    {
        yield return new WaitForSeconds(wait);
        BeforeWear = false;
        AfterWear = false;
        ArtanWear();
        ColliderBeforeWear.SetActive(false);
        StartCoroutine(EndWear(TimeWear));
        PlaySound(SoundWear, VolumeWear);
    }

    IEnumerator EndWear (float wait)
    {
        yield return new WaitForSeconds(wait);
        AfterWear = false;
        BeforeWear = true;
        PanelFade.SetBool("Show", false);

        StartCoroutine(endWear2(6.8f));
    }

    IEnumerator endWear2 (float wait)
    {
        yield return new WaitForSeconds(wait);
        PanelFade.gameObject.SetActive(false);
        AfterWear = false;
        BeforeWear = false;
    }

    void FixedUpdate()
    {
        LidaMovement();

        if (BeforeWear)
            VolumeDown();

        if (AfterWear)
            VolumeUp();
    }


    public void PlaySound(AudioClip clip, float volume)
    {
        SoundPlayer.clip = clip;
        SoundPlayer.volume = volume;
        SoundPlayer.Play();
    }

    void LidaMovement ()
    {
        if(LidaCanGo)
        {
            Lida.transform.Translate(Vector2.left * LidaMovementSpeed * Time.fixedDeltaTime);

            if(Lida.transform.position.x > 24)
            {
                Lida.SetActive(false);
            }
        }
    }

    public void KeyUsed ()
    {
        Door4Interaction.SetActive(true);
        DoorRoom4VIP.SetActive(false);
    }

    void ArtanWear ()
    {
        GameObject.FindObjectOfType<PlayerMovement>().ChangeClothes(0);
        _step.DoWork(39);
    }

    void VolumeDown()
    {
        if (music.volume > 0)
        {
            music.volume -= SpeedVolumeDown * Time.deltaTime;
            music2.volume -= SpeedVolumeDown * Time.deltaTime;
        }
    }

    void VolumeUp()
    {
        if (music.volume <= 0.25f)
        {
            music.volume += SpeedVolumeUp * Time.deltaTime;
        }

        if (music2.volume <= 1f)
        {
            music2.volume += SpeedVolumeUp * Time.deltaTime;
        }
    }
}
