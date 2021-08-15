using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;
using System.Collections;


public class Scene2 : MonoBehaviour
{

    private int timeEnter;

    [SerializeField] private GameObject _Tutorail;

    private Step _step;

    [SerializeField] private GameObject bird;
    [SerializeField] private Transform targetBird;
    [SerializeField] private AudioClip birdSound;
    [Range(0,1)] [SerializeField] private float VolumeBird = 0.5f;
    [SerializeField] private float birdSpeed;
    private bool CanbirdRun;


    [SerializeField] private AudioClip SoundFuseInPlace;
    [SerializeField] [Range(0, 1)] private float VolumeFuseInPlace = 0.5f;
    [SerializeField] private AudioClip SoundFuseButton;
    [SerializeField] [Range(0, 1)] private float VolumeFuseButton = 0.5f;


    [SerializeField] private AudioClip SoundCarEngine;
    [SerializeField] [Range(0, 1)] private float VolumeCarEngine = 0.5f;

    [SerializeField] private AudioClip SoundLightOff;
    [SerializeField] [Range(0,1)] private float VolumeLightOff;

    private bool DoTouch;

    [SerializeField] private GameObject MainThunder;

    [SerializeField] private AudioClip Soundthunder1;
    [Range(0,1)] [SerializeField] private float Volume1 = 0.5f;
    [SerializeField] private AudioClip Soundthunder2;
    [Range(0, 1)] [SerializeField] private float Volume2 = 0.5f;
    private AudioSource Soundplayer;


    //
    [SerializeField] private Light2D [] _lights;
    [SerializeField] private float [] IntensityValues;
    [SerializeField] private Light2D MainLight;

    // fuse box
    [SerializeField] private GameObject FuseBox;
    [SerializeField] private GameObject FusePlace;
    [SerializeField] private Sprite CloseFuseBox;
    [SerializeField] private Sprite OpenFuseBox;
    [SerializeField] private GameObject PanelFuseBox;
    [SerializeField] private GameObject [] fuseBoxObjs;
    [SerializeField] private Sprite greenLight;
    [SerializeField] private GameObject fuse2Item;
    [SerializeField] private GameObject WindowLight;
    [SerializeField] private GameObject LightZargMotel;
    [SerializeField] private GameObject LightMotel2;


    [SerializeField] private GameObject Lida;
    [SerializeField] private GameObject TriggerLida;
    [SerializeField] private Animator Margin;
    [SerializeField] private ConversationObject dialog;
    [SerializeField] private GameObject Artan;

    [SerializeField] private Animator PanelFade;
    [SerializeField] private GameObject Camera3;

    [SerializeField] private AudioSource music;
    private float FirstVolumemusic;
    [SerializeField] private AudioSource music2;
    private float FirstVolumemusic2;
    [SerializeField] private AudioSource music3;
    private float FirstVolumemusic3;
    [SerializeField] private float SpeedVolumeDown;
    [SerializeField] private float SpeedVolumeUp;
    private bool canup;
    private bool candown;

    [SerializeField] private GameObject FrontLightCar;
    [SerializeField] private GameObject Car;
    [SerializeField] private float CarSpeed;
    private bool CanCarMove;
 

    [SerializeField] private Animator BlackPanel;
    [SerializeField] private AudioSource CarArriveSound;

    void Awake()
    {    
        FirstVolumemusic = music.volume;
        FirstVolumemusic2 = music2.volume;
        FirstVolumemusic3 = music3.volume;   

        
        MainThunder.GetComponent<Animator>().Play("Thunder_Light_Long");
        Soundplayer = GetComponent<AudioSource>();       
    }
    void Start()
    {
        #region Steps
        GameDataController gameData = FindObjectOfType<GameDataController>();
        Step._steps = gameData.gameData.steps;
        _step = GetComponent<Step>();


        //BlackPanel

        if(!_step.Steps[0]) {
           BlackPanel.gameObject.SetActive(true);
            CarArriveSound.Play();

            StartCoroutine(WaitToBlackPanel());
        }
        else {
             PlaySound(Soundthunder1,false, Volume1);
        }

        //dialog light
        if(_step.Steps[7])
        {
            AllLightOff();

            if(!_step.Steps[8])
            {
                fuse2Item.SetActive(true);
            }
        }

        //fuse in place
        if(_step.Steps[9])
        {
            fuseBoxObjs[0].SetActive(true);
            fuseBoxObjs[1].GetComponent<Image>().sprite = greenLight;
            FuseBox.transform.GetChild(0).transform.GetChild(3).gameObject.SetActive(true);
            FuseBox.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = greenLight;
        }

        //button_fuse_on
        if(_step.Steps[10])
        {
            AllLightOn();
            fuseBoxObjs[2].SetActive(true);
            fuseBoxObjs[3].SetActive(false);
            FuseBox.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = greenLight;
            FuseBox.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
            FuseBox.transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(true);
            FuseBox.transform.GetChild(0).transform.GetChild(3).gameObject.SetActive(true);
        }


        if(_step.Steps[39])
        {
            Lida.SetActive(true);
            TriggerLida.SetActive(true);
        }
        #endregion
       
    }


    IEnumerator WaitToBlackPanel () {

       yield return new WaitForSeconds(9);
       BlackPanel.Play("FadeOutFirst");

       StartCoroutine(TutorailCome());
    }
    
    IEnumerator TutorailCome () {

        yield return new WaitForSeconds(4.2f);
        BlackPanel.gameObject.SetActive(false);
         if (!_step.Steps[0])
        {
            _Tutorail.GetComponent<Tutorail>().TutorailShow(1);
            _step.DoWork(0);

        }
    }



    void FixedUpdate()
    {
        BirdRun();

        if (candown)
            VolumeDown();

        if (canup)
            VolumeUp();

        if(CanCarMove)
        {
            Car.transform.Translate(Vector2.left * CarSpeed * Time.fixedDeltaTime);
        }
    }
    
    
    public void CheckTouch(string name)
    {
        if (name == "Interaction Telephone" && !DoTouch)
        {
            DoTouch = true;
        }

        if (name == "Interaction FuseBox" && FuseBox.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            // Another Time
            PanelFuseBox.SetActive(true);
            FusePlace.SetActive(true);
        }

        if (name == "Interaction FuseBox" && !FuseBox.transform.GetChild(0).gameObject.activeInHierarchy && _step.Steps[7] && !_step.Steps[10])
        {
            // First Time
            FuseBox.transform.GetChild(0).gameObject.SetActive(true);
            FuseBox.transform.GetChild(1).gameObject.SetActive(false);
            PanelFuseBox.SetActive(true);
            FusePlace.SetActive(true);
        }

    }

    public void CheckTrigger(string name)
    {
        if (name == "tutorail_Run" && !_step.Steps[1])
        {
            _step.DoWork(1);
            _Tutorail.GetComponent<Tutorail>().TutorailShow(2);
        }

        if(name == "tutorail_Interaction" && !_step.Steps[2])
        {

            _step.DoWork(2);
            _Tutorail.GetComponent<Tutorail>().TutorailShow(3);
        }

        if(name == "Thunder1" && !_step.Steps[4])
        {
            MainThunder.GetComponent<Animator>().Play("Thunder_Light_Long");
            PlaySound(Soundthunder1, false,Volume1);

            _step.DoWork(4);
        }

        if (name == "Thunder2" && !_step.Steps[5])
        {
            if(timeEnter >= 5)
                     return;
           
            timeEnter++;

            if (timeEnter == 1 || timeEnter == 5)
            {
                MainThunder.GetComponent<Animator>().Play("Thunder_Light_Long");
                PlaySound(Soundthunder2, false, Volume2);
                if(timeEnter == 5)
                {
                    _step.DoWork(5);
                }
            }
        }

        if(name == "Exit_Telephone" && DoTouch && !_step.Steps[6])
        {
            CanbirdRun = true;
            PlaySound(birdSound, false,VolumeBird);

            _step.DoWork(6);
        }

        if(name == "SideLida")
        {
            MarginOpen();
        }
    }

    public void CheckTriggerExit (string name)
    {
        if(name == "FuseBoxExit")
        {
            PanelFuseBox.SetActive(false);
        }
    }


    private void BirdRun ()
    {
        if(!CanbirdRun)
        {
            return;
        }

        if(bird.transform.position != targetBird.position)
        {
            bird.transform.position = Vector3.MoveTowards(bird.transform.position, targetBird.position,
            birdSpeed * Time.fixedDeltaTime);
        }
    }

    public void CheckEvent(int a)
    {
        if(a == 1 && !_step.Steps[3])
        {
            _step.DoWork(3);
            _Tutorail.GetComponent<Tutorail>().TutorailShow(4);
        }

    }

    private void PlaySound (AudioClip sound , bool isLoop , float volume)
    {
        Soundplayer.clip = sound;
        Soundplayer.loop = isLoop;
        Soundplayer.volume = volume;

        Soundplayer.Play();
    }

    public void FuseCheck ()
    {
        if(_step.Steps[9])
        {
            fuseBoxObjs[0].SetActive(true);
            fuseBoxObjs[1].GetComponent<Image>().sprite = greenLight;

            FuseBox.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = greenLight;
            FuseBox.transform.GetChild(0).transform.GetChild(3).gameObject.SetActive(true);


            PlaySound(SoundFuseInPlace, false, VolumeFuseInPlace);

            _step.DoWork(9);
        }
    }

    public void TryHandleFuse ()
    {
        if(_step.Steps[9])
        {
            fuseBoxObjs[2].gameObject.SetActive(true);
            fuseBoxObjs[3].gameObject.SetActive(false);

            FuseBox.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = greenLight;
            FuseBox.transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
            FuseBox.transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(true);
            FuseBox.transform.GetChild(0).transform.GetChild(3).gameObject.SetActive(true);

            PlaySound(SoundFuseButton, false, VolumeFuseButton);

            _step.DoWork(10);

            PlaySound(SoundLightOff,false,VolumeLightOff);
            FindObjectOfType<GameDataController>().gameData.SetGameEventAsFinished("TurnOnElectricity");
            StartCoroutine(waitToClose(0.5f));
           StartCoroutine(waitToLightCome(1f));
        }
    }

    IEnumerator waitToClose (float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        PanelFuseBox.SetActive(false);

        GameObject.Find("Move Holder - New").GetComponent<Animator>().Play("MoveHolderAnimationOff");
    }

    IEnumerator waitToLightCome (float Waittime)
    {
        yield return new WaitForSeconds(Waittime);
        AllLightOn();
    }


    public void MarginOpen()
    {
        Margin.gameObject.SetActive(true);
        Margin.SetBool("Show", true);


        GameObject.FindObjectOfType<DialogueInteraction>().OnDialogueStarted
            (Artan);
        GameObject.FindObjectOfType<DialogueController>().SetDialog(dialog);

        GameObject.FindObjectOfType<PlayerMovement>().RunStop();
        GameObject.FindObjectOfType<PlayerMovement>().Stop();

    }
    public void MarginClose()
    {
        Margin.SetBool("Show", false);

        PanelFade.gameObject.SetActive(true);
        PanelFade.SetBool("Show", true);

        GameObject.Find("Inventory").SetActive(false);
        GameObject.Find("Inventorybtn").SetActive(false);
        GameObject.Find("Move Holder - New").SetActive(false);

        candown = true;
        StartCoroutine(FadeOutNow(12f));
    }

    IEnumerator FadeOutNow (float wait)
    {
        yield return new WaitForSeconds(wait);

        candown = false;
        canup = true;
        ArtanLidaGoToCar();
        PanelFade.SetBool("Show", false);

        StartCoroutine(LightCarOn(9f));
    }

    IEnumerator LightCarOn (float wait)
    {
        yield return new WaitForSeconds(wait);
        canup = false;

        FrontLightCar.SetActive(true);

        PlaySound(SoundCarEngine,false, VolumeCarEngine);
        StartCoroutine(CarMove(3f));
    }

    IEnumerator CarMove (float wait)
    {
        yield return new WaitForSeconds(wait);
        CanCarMove = true;
        StartCoroutine(FinishFadeOut(1f));

        Car.GetComponent<Animator>().Play("Car2Animation");
    }

    IEnumerator FinishFadeOut (float wait)
    {
        yield return new WaitForSeconds(wait);
        PanelFade.SetBool("Show", true);
        candown = true;
    }

    void ArtanLidaGoToCar ()
    {
        Artan.transform.position = new Vector3(100, 100, 0);
        Lida.transform.position = new Vector3(100, 100, 0);
        Camera3.SetActive(true);
    }

    void VolumeDown()
    {
        if(music2.volume == 0 && music.volume == 0 && music3.volume == 0)
             return;


        if (music.volume > 0)
        {
            music.volume -= SpeedVolumeDown * Time.deltaTime;
        }

        if(music2.volume > 0) {
            music2.volume -= SpeedVolumeDown * Time.deltaTime;
        }

        if(music3.volume > 0) {
            music3.volume -= SpeedVolumeDown * Time.deltaTime;
        }
    }

    void VolumeUp()
    {

        if(music2.volume == FirstVolumemusic2 && music.volume == FirstVolumemusic && music3.volume == FirstVolumemusic3)
             return;

        if (music.volume <= FirstVolumemusic)
        {
            music.volume += SpeedVolumeUp * Time.deltaTime;
        }

        if (music2.volume <= FirstVolumemusic2)
        {
            music2.volume += SpeedVolumeUp * Time.deltaTime;
        }

        if(music3.volume <= FirstVolumemusic3) {
            music3.volume += SpeedVolumeUp * Time.deltaTime;
        }


    }

    public void AllLightOff ()
    {
        for (int i = 0; i < _lights.Length; i++)
        {
            if(_lights[i] != null)
               _lights[i].intensity = 0f;
        }

        WindowLight.SetActive(false);
        LightZargMotel.SetActive(false);
        LightMotel2.SetActive(false);
        MainLight.intensity = 0.05f;
    }

    public void AllLightOn ()
    {
        for (int i = 0; i < _lights.Length; i++)
        {
            if (_lights[i] != null)
                _lights[i].intensity = IntensityValues[i];
        }

        WindowLight.SetActive(true);
        LightZargMotel.SetActive(true);
        LightMotel2.SetActive(true);

        MainLight.intensity = 0.4f;
    } 

}
