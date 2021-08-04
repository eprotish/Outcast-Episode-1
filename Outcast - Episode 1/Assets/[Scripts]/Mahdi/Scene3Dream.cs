using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Scene3Dream : MonoBehaviour
{
    [SerializeField] private AudioClip Sound1;
    [Range(0, 1)] [SerializeField] private float volume1 = 0.5f;
    [SerializeField] private AudioClip Sound_t;
    [Range(0, 1)] [SerializeField] private float volume2 = 0.5f;
    [SerializeField] private AudioClip SoundLoster;
    [SerializeField] [Range(0, 1)] private float VolumeLoster = 0.5f;
    [SerializeField] private AudioClip SoundSafeOpen;
    [SerializeField] [Range(0, 1)] private float volumeSafeOpen = 0.5f;
    [SerializeField] private AudioClip SoundPedram;
    [SerializeField] [Range(0, 1)] private float VolumePedram = 0.5f;

    [SerializeField] private Animator Margin;
    [SerializeField] private GameObject MissPeople1;
    [SerializeField] private GameObject InteractionMissPanel;
    [SerializeField] private GameObject Trigger2;

    [SerializeField] private AudioSource SoundPlayer;

    [SerializeField] private GameObject moveHolder;
    [SerializeField] private GameObject MissPanel;
    [SerializeField] private GameObject Circle;
    [SerializeField] private GameObject Battery;
    [SerializeField] private GameObject CoffeeShopBanner1;
    [SerializeField] private GameObject CoffeeShopBanner2;

    [SerializeField] private GameObject TriggerCoffeeShop;
    [SerializeField] private Light2D [] lightsCoffeeShop1;
    [SerializeField] private Light2D[] lightsCoffeeShop2;
    [SerializeField] private float SpeedLightCoffeeShopOn;
    private bool LightCoffeeShopOn = false;

    [SerializeField] private GameObject KeyPanel;
    [SerializeField] private GameObject LockPanel;
    [SerializeField] private GameObject KeyInteraction;
    [SerializeField] private GameObject LockInteraction;

    [SerializeField] private GameObject Artan;
    [SerializeField] private ConversationObject dialog;
    [SerializeField] private LoadLevelInteraction _load;

    [SerializeField] private GameObject DarPedram;
    [SerializeField] private GameObject PanelCantWork;

    [SerializeField] private GameObject Loster;
    [SerializeField] private GameObject TriggerLoster;


    [SerializeField] private float shakeInt;
    [SerializeField] private float shakeDis;
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject KeyHolder;
    [SerializeField] private Transform KeyHolderOutSide;
    [SerializeField] private float SpeedKeyHolder;
    private bool KeyholderInTarget = true;

    private Vector3 orginPos;
    private Vector3 shakePos;

    private bool is_shake;
    private float shake_time = 1f;

    [SerializeField] private GameObject CloseSafe;
    [SerializeField] private GameObject OpenSafe;
    [SerializeField] private GameObject ZeroKey;


    [SerializeField] private Light2D globalLight;
    [SerializeField] private Light2D [] anotherLight;

    private float globalLightValue;
    private float [] anotherValue; 

    private Step _step; 
      
    void Start()
    {      
        _step = GameObject.FindObjectOfType<Step>();

        if(_step.Steps[18])
        {
            MissPeople1.SetActive(true);
            InteractionMissPanel.SetActive(true);
        }

        if(_step.Steps[19])
        {
            Circle.SetActive(true);
        }

        if(_step.Steps[22] && !_step.Steps[24])
        {
            Battery.SetActive(true);
            TriggerCoffeeShop.SetActive(true);           
        }

        if(_step.Steps[22] && !_step.Steps[25])
        {
            KeyInteraction.SetActive(true);
        }

        if(!_step.Steps[23])
        {
            LightCoffeeShopOff();
        }

        if(_step.Steps[20])
        {
            _load.nextSceneName = "Scene 3-2 SF - LongDream";
        }

        if(_step.Steps[25] && !_step.Steps[27])
        {
            LockInteraction.SetActive(true);
        }

        if(_step.Steps[26] && !_step.Steps[31])
        {
            TriggerLoster.SetActive(true);
        }

        if (_step.Steps[31])
        {
            Loster.transform.localPosition = new Vector3(5.06f, -3.98f, 0);
            Loster.transform.eulerAngles = new Vector3(0, 0, -90);
            Loster.transform.GetChild(0).gameObject.SetActive(false);
        }

        if(_step.Steps[32])
        {
            CloseSafe.SetActive(false);
            OpenSafe.SetActive(true);
        }

        if(_step.Steps[33])
        {
            ZeroKey.SetActive(false);
        }

        if(_step.Steps[25])
        {
            KeyHolder.transform.position = KeyHolderOutSide.position;
        }

    }

    void FixedUpdate()
    {
        if(LightCoffeeShopOn)
        {
            LerpLightCoffeeShopOn();
        }

        camera_shake();

        if(!KeyholderInTarget)
             KeyHolderMove();
    }


    public void CheckTrigger (string a)
    {
        if(a == "Trigger1" && !_step.Steps[15])
        {
            Trigger2.SetActive(true);
            _step.DoWork(15);
        }

        if(a == "Trigger2" && !_step.Steps[16])
        {
            PlaySound(Sound1, volume1);
            _step.DoWork(16);
        }

        if (a == "TriggerCoffeeShop" && !_step.Steps[23])
        {
           StartCoroutine(BannerOn(4f));
            MarginOpen();
            LightCoffeeShopOn = true;
            _step.DoWork(23);
        }

        if(a == "TriggerPedram" && !_step.Steps[30])
        {
            GameObject.FindObjectOfType<PlayerMovement>().RunStop();
            GameObject.FindObjectOfType<PlayerMovement>().Stop();
            PanelCantWork.SetActive(true);
            StartCoroutine(PanelCantWorkClose(1.5f));
            _step.DoWork(30);
            DarPedram.SetActive(true);
            shake_time = 1f;
            shakeDis = 0.12f;
            shakeInt = 19f;
            cam.SetActive(true);
            cam.transform.position = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
            cam.GetComponent<Camera>().orthographicSize = 3.43f;
            is_shake = true;
            orginPos = cam.transform.position;
            PlaySound(SoundPedram, VolumePedram);

        }

        if(a == "TriggerLamp5" && !_step.Steps[31])
        {
            FallLoster();
            _step.DoWork(31);
        }
    }

    public void CheckTouch (string a)
    {
        if(a == "Interaction Note")
        {
            MissPanel.SetActive(true);
        }

        if(a == "Interaction Safe" && !_step.Steps[32])
        {
            _step.DoWork(32);
            PlaySound(SoundSafeOpen, volumeSafeOpen);
            CloseSafe.SetActive(false);
            OpenSafe.SetActive(true);
        }
    }

    public void CheckTriggerExit (string a)
    {
        if(a == "TriggerExitReception")
        {
            LockPanel.SetActive(false);
            KeyPanel.SetActive(false);
        }
    }

    public void PlaySound (AudioClip clip , float volume)
    {
        SoundPlayer.clip = clip;
        SoundPlayer.volume = volume;
        SoundPlayer.Play();
    }

    public void T_Touch ()
    {
        if(!_step.Steps[19]) {

        Circle.SetActive(true);
        Circle.GetComponent<Animator>().Play("T_MissPanel_anim");
        PlaySound(Sound_t, volume2);
        _step.DoWork(19);
        FindObjectOfType<GameDataController>().gameData.SetGameEventAsFinished("LightningIfTTouched");

        }

    }

    private void LightCoffeeShopOff ()
    {
        for (int i = 0; i < lightsCoffeeShop1.Length; i++)
        {
            lightsCoffeeShop1[i].intensity = 0;
        }

        for (int i = 0; i < lightsCoffeeShop2.Length; i++)
        {
            lightsCoffeeShop2[i].intensity = 0;
        }

        CoffeeShopBanner1.SetActive(false);
        CoffeeShopBanner2.SetActive(false);
    }

    private void LerpLightCoffeeShopOn ()
    {
        if(lightsCoffeeShop1[0].intensity < 0.96f)
        {
            for (int i = 0; i < lightsCoffeeShop1.Length; i++)
            {
                lightsCoffeeShop1[i].intensity =
                    Mathf.Lerp(lightsCoffeeShop1[i].intensity, 1, SpeedLightCoffeeShopOn * Time.fixedDeltaTime);
            }
        }

        if(lightsCoffeeShop2[0].intensity < 1.15f)
        {
            for (int i = 0; i < lightsCoffeeShop2.Length; i++)
            {
                lightsCoffeeShop2[i].intensity =
                    Mathf.Lerp(lightsCoffeeShop2[i].intensity, 1.19f, SpeedLightCoffeeShopOn * Time.fixedDeltaTime);
            }
        }

        if(lightsCoffeeShop1[0].intensity >= 0.96f && lightsCoffeeShop2[0].intensity >= 1.15f)
        {
            LightCoffeeShopOn = false;
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
    public void  MarginClose()
    {
        GameObject.FindObjectOfType<DialogueInteraction>().OnDialogueEnded();
        Margin.SetBool("Show", false);
        moveHolder.SetActive(true);
    }

    public void FallLoster ()
    {
        Loster.transform.GetChild(0).gameObject.SetActive(false);
        Loster.GetComponent<Rigidbody2D>().gravityScale = 1f;
        Loster.transform.eulerAngles = new Vector3(0, 0, -6);
        StartCoroutine(LosterDown(0.85f));
    }

    IEnumerator BannerOn (float waitTime)
    {
        yield return new WaitForSeconds(waitTime); 
        CoffeeShopBanner1.SetActive(true);
        CoffeeShopBanner2.SetActive(true);
    }

    IEnumerator PanelCantWorkClose (float wait)
    {
        yield return new WaitForSeconds(wait);
        PanelCantWork.SetActive(false);

        LightOff();

        StartCoroutine(LightBackByDelay());
    }

    IEnumerator LightBackByDelay () {
        yield return new WaitForSeconds(1.5f);

        DarPedram.SetActive(false);
        LightOn();
    }

    IEnumerator LosterDown (float wait)
    {
        yield return new WaitForSeconds(wait);

        GameObject.FindObjectOfType<PlayerMovement>().RunStop();
        GameObject.FindObjectOfType<PlayerMovement>().Stop();

        cam.SetActive(true);
        is_shake = true;
        orginPos = cam.transform.position;
        PlaySound(SoundLoster, VolumeLoster);
        moveHolder.SetActive(false);
    }

    IEnumerator CameraBack (float wait)
    {
        yield return new WaitForSeconds(wait);
        cam.SetActive(false);
        moveHolder.SetActive(true);
        GameObject.FindObjectOfType<PlayerMovement>().RunStop();
        GameObject.FindObjectOfType<PlayerMovement>().Stop();
    }

    void camera_shake()
    {
        if (!is_shake)
        {
            return;
        }

        var xpos = Time.time * shakeInt + 10;
        var ypos = Time.time * shakeInt + 100;


        shake_time -= Time.deltaTime;

        if (shake_time > 0)
        {
            shakePos = new Vector3((Mathf.PerlinNoise(xpos, 1) - 0.5f) * shakeDis,
            (Mathf.PerlinNoise(ypos, 1) - 0.5f) * shakeDis, -10);

            cam.transform.position = orginPos + shakePos;
        }

        if (shake_time <= 0)
        {
            is_shake = false;
            cam.transform.position = orginPos;
            StartCoroutine(CameraBack(1.5f));
        }

    }

    public void KeyHolderOut ()
    {
        KeyholderInTarget = false;
        _step.DoWork(25);
        KeyInteraction.SetActive(false);
        LockInteraction.SetActive(true);
    }

    private void KeyHolderMove ()
    {

            KeyHolder.transform.position =
                Vector3.MoveTowards(KeyHolder.transform.position, KeyHolderOutSide.position, SpeedKeyHolder * Time.fixedDeltaTime);

        if(KeyHolder.transform.position == KeyHolderOutSide.position)
        {
            KeyholderInTarget = true;
        }
    }


    private void LightOff () {
      
       globalLightValue = globalLight.intensity;
       globalLight.intensity = 0;
       anotherValue = new float [anotherLight.Length];

       for (var i = 0; i < anotherLight.Length; i++)
       {
           anotherValue[i] = anotherLight[i].intensity;
           anotherLight[i].intensity = 0;
       }
    }

    private void LightOn () {

       globalLight.intensity = globalLightValue;

       for (var i = 0; i < anotherLight.Length; i++)
       {
           anotherLight[i].intensity = anotherValue[i];
       }
    }
}
