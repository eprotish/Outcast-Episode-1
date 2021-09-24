using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class Storge : MonoBehaviour
{

    [SerializeField] private GameObject interactionFuseBox;
    [SerializeField] private AudioSource audio;

    [SerializeField] private GameObject OpenFuse;
    [SerializeField] private GameObject CloseFuse;
    
    [SerializeField] private AudioClip SoundFuseInPlace;
    [SerializeField] [Range(0, 1)] private float VolumeFuseInPlace = 0.5f;
    [SerializeField] private AudioClip SoundFuseButton;
    [SerializeField] [Range(0, 1)] private float VolumeFuseButton = 0.5f;
    [SerializeField] private AudioClip SoundLightOn;
    [SerializeField] [Range(0, 1)] private float VolumeLightOn = 0.5f;
    
    
    [SerializeField] private GameObject FuseBox;
    [SerializeField] private GameObject FusePlace;
    [SerializeField] private GameObject PanelFuseBox;
    [SerializeField] private GameObject [] fuseBoxObjs;
    [SerializeField] private GameObject[] fuseBoxUi;
    [SerializeField] private Sprite greenLight;

    [SerializeField] private Light2D globalLight;
    [SerializeField] private Light2D [] lights;

    [SerializeField] private float [] lightValue;

    private Step _step;

    private bool FuseIn;
    
    void Start()
    {
       _step = GetComponent<Step>();

       if (!_step.Steps[7])
       {
           OpenFuse.SetActive(false);
           CloseFuse.SetActive(true);
       }
       
        if(_step.Steps[7] && !_step.Steps[10])
            LightOff();

        if (_step.Steps[10])
        {
            fuseBoxObjs[0].SetActive(true);
            fuseBoxObjs[1].GetComponent<SpriteRenderer>().sprite = greenLight;
            fuseBoxObjs[2].SetActive(false);
            fuseBoxObjs[3].SetActive(true);
            LightOn();
            interactionFuseBox.SetActive(false);
        }
         
        

    }


    public void FuseCheck()
    {
        FuseIn = true;
        fuseBoxObjs[0].SetActive(true);
        fuseBoxUi[0].SetActive(true);
        
        fuseBoxObjs[1].GetComponent<SpriteRenderer>().sprite = greenLight;
        fuseBoxUi[1].GetComponent<Image>().sprite = greenLight;
        
        SoundPlayer(SoundFuseInPlace,VolumeFuseInPlace);
    }

    public void TryFuse()
    {
        if (FuseIn && !_step.Steps[10])
        {
            fuseBoxObjs[2].SetActive(false);
            fuseBoxObjs[3].SetActive(true);
                        
            fuseBoxUi[2].SetActive(false);
            fuseBoxUi[3].SetActive(true);
            
            _step.DoWork(10);
            interactionFuseBox.SetActive(false);
            
            Invoke("CloseFuseBoxByDelay",1.1f);
        }
        else
        {
            // nemishe
        }
        
        SoundPlayer(SoundFuseButton,VolumeFuseButton);
    }
    
    
    public void CheckTouch(string a)
    {
        if (a == "Interaction FuseBox")
        {
            OpenFuseBox();
        }

    }

    void CloseFuseBoxByDelay()
    {
        SoundPlayer(SoundLightOn,VolumeLightOn);
        LightOn();
        PanelFuseBox.SetActive(false);
        GameObject.Find("Move Holder - New").GetComponent<Animator>().Play("MoveHolderAnimationOff");
    }


    void OpenFuseBox()
    {
        PanelFuseBox.SetActive(true);
        FusePlace.SetActive(true);
    }


    void SoundPlayer(AudioClip _clip , float _volume)
    {
        audio.clip = _clip;
        audio.volume = _volume;
        
        audio.Play();
    }


    void LightOn()
    {
        globalLight.intensity = lightValue[0];
        lights[0].intensity = lightValue[1];
        lights[1].intensity = lightValue[2];
    }

    void LightOff()
    {
        globalLight.intensity = 0.1f;
        lights[0].intensity = 0;
        lights[1].intensity = 0;
    }
}

