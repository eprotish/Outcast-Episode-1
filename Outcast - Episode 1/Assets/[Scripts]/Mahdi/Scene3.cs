using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Scene3 : MonoBehaviour
{

    [SerializeField] private AudioSource SoundLightOff;
    [SerializeField] private AudioSource ringBellPlayer;

    private int RingTheBellTime;
    [SerializeField] private Animator Margin;

    [SerializeField] private GameObject _Player;
    [SerializeField] private GameObject _Jamshid;
    [SerializeField] private Transform JamshidPosTarget;
    [SerializeField] private float JamshidSpeed;


    private Animator JamshidAnimator;
    private bool JamshidWork = false;
    private bool JamshidArrive;

    [SerializeField] private GameObject Converstion;


    [SerializeField] private Light2D [] _lights;
    [SerializeField] private Light2D _mainLight;
    [SerializeField] private float [] IntensityValues;

    private Step _step;

    [SerializeField] private Sprite _keyArtanRoom;
    [SerializeField] private GameObject Panel_NewItem;


    public GameObject LoadSceneSF;
    public GameObject Interaction_KasiNist;
    public GameObject Bell;

    private bool CanRingBell = true;

    void Start()
    {
        #region Steps

        LoadSceneSF.SetActive(false);

        JamshidAnimator = _Jamshid.GetComponent<Animator>();
        _step = GetComponent<Step>();

        if(_step.Steps[7] && !_step.Steps[10])
        {
            JamshidAnimator.Play("Jamshid_Idle");
            _Jamshid.transform.position = JamshidPosTarget.position;
            AllLightOff();
        }

        if(_step.Steps[10])
        {
            JamshidAnimator.Play("Jamshid_Idle");
            _Jamshid.transform.position = JamshidPosTarget.position;
        }

        if(_step.Steps[39])
        {
            _Jamshid.SetActive(false);
        }

        if(_step.Steps[11])
        {
            LoadSceneSF.SetActive(true);
        }

        if(_step.Steps[7])
        {
            Interaction_KasiNist.SetActive(false);
            Bell.SetActive(false);
        }

        #endregion
    }

    void FixedUpdate()
    {
        if(!_step.Steps[7])
        {
            if (JamshidWork && _Jamshid.transform.position != JamshidPosTarget.position)
            {
                _Jamshid.transform.position = Vector3.MoveTowards(_Jamshid.transform.position,
                    JamshidPosTarget.position, JamshidSpeed * Time.fixedDeltaTime);
            }


            if (_Jamshid.transform.position == JamshidPosTarget.position &&
                !JamshidArrive)
            {

                JamshidAnimator.Play("Jamshid_Idle");
                Converstion.GetComponent<DialogueInteraction>().OnDialogueStarted(_Player);
                JamshidArrive = true;

            }
        }
    }

    public void RingTheBell ()
    {
        if(!_step.Steps[7])
        {
            if (CanRingBell)
            {
                CanRingBell = false;
                Invoke("CanRingAgain", 1f);
                ringBellPlayer.Play();
                RingTheBellTime++;
                if (RingTheBellTime == 2)
                {
                    MarginOpen();
                    JamshidWork = true;
                    Interaction_KasiNist.SetActive(false);
                    Bell.SetActive(false);
                }
            }
        }
    }

    private void CanRingAgain ()
    {
        CanRingBell = true;
    }

    public void CheckTouch (string name)
    {
        if (name == "Interaction Bell")
        {
            RingTheBell();
        }

        if(name == "JamshidMH" && _step.Steps[10])
        {
            MarginOpen();
            JamshidWork = true;
            Converstion.GetComponent<DialogueInteraction>().OnDialogueStarted(_Player);
        }
    }

    public void LightOffSound () {

        SoundLightOff.Play();
    }

    public void MarginOpen ()
    {
        Margin.gameObject.SetActive(true);
        Margin.SetBool("Show", true);
    }
    public void MarginClose ()
    {
        Margin.SetBool("Show", false);


        if (_step.Steps[10])
        {
            LoadSceneSF.SetActive(true);
            GameObject.FindObjectOfType<InventoryManger>().AddItem("KeyArtanRoom", _keyArtanRoom);
            Panel_NewItem.SetActive(true);
            _step.DoWork(11);
        }

    }

    public void AllLightOff ()
    {
        for (int i = 0; i < _lights.Length; i++)
        {
            _lights[i].intensity = 0f;
        }
        _mainLight.intensity = 0.1f;
    }
    public void AllLightOn ()
    {
        for (int i = 0; i < _lights.Length; i++)
        {
            _lights[i].intensity = IntensityValues[i];
        }
        _mainLight.intensity = 0.2f;
    }
}
