using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene4VIP : MonoBehaviour
{
    [SerializeField] private AudioSource KnockKnockPlayer;
    [SerializeField] private float [] TimeKnockKnock;

    [SerializeField] private GameObject Interactio_Sleep;

    [SerializeField] private GameObject PanelChoice;
    [SerializeField] private GameObject PanelFade;
    [SerializeField] private Animator Margin;
    [SerializeField] private GameObject moveHolder;
    [SerializeField] private ConversationObject dialog;
    [SerializeField] private GameObject Artan;
    [SerializeField] private GameObject PanelFade2;

    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource music2;
    [SerializeField] private float SpeedVolumeDown;
    [SerializeField] private float SpeedVolumeUp;
    private bool AfterSleep;
    private bool isSleep;

    private Step _step;

    void Start()
    {
        _step = GameObject.FindObjectOfType<Step>();

        if(_step.Steps[36] && !_step.Steps[37])
        {
            music.volume = 0f;
            music2.volume = 0f;
            AfterSleep = true;
            PanelFade2.SetActive(true);
            PanelFade2.GetComponent<Animator>().SetBool("Show", false);
            StartCoroutine(ArtanTalkHimself(6.5f));
        }

        if(_step.Steps[36])
        {
            Interactio_Sleep.SetActive(false);
        }

    }


    public void CheckTouch (string a)
    {

    }

    public void EndInteraction (string a)
    {
        if(a == "Interaction Bed" && !isSleep)
        {
            PanelChoice.SetActive(true);
        }
    }

    public void ArtanGoToSleep ()
    {
        PanelFade.SetActive(true);
        PanelFade.GetComponent<Animator>().SetBool("Show", true);
        isSleep = true;

        FindObjectOfType<GameDataController>().gameData.SetGameEventAsFinished("Sleep");
        StartCoroutine(GoToDream(9));
    }

    IEnumerator GoToDream (float wait)
    {
        yield return new WaitForSeconds(wait);
        GameObject.FindObjectOfType<Step>().DoWork(13);
        SceneManager.LoadScene("Scene 4-1 VIP Room - Dream");
    }

    IEnumerator KnockKnockLoop (float wait)
    {
        yield return new WaitForSeconds(wait);
        KnockKnockPlayer.Play();


        float ran = UnityEngine.Random.Range(TimeKnockKnock[0],TimeKnockKnock[1]);
        StartCoroutine(KnockKnockLoop(ran));
    }

    IEnumerator ArtanTalkHimself (float wait)
    {
        yield return new WaitForSeconds(wait);
        MarginOpen();
    }

    public void MarginOpen()
    {
        PanelFade2.SetActive(false);
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
        GameObject.FindObjectOfType<DialogueInteraction>().OnDialogueEnded();
        Margin.SetBool("Show", false);
        moveHolder.SetActive(true);

        StartCoroutine(KnockKnockLoop(0f));
    }

    private void Update()
    {
        VolumeDown();

        if (AfterSleep)
            VolumeUp();
    }

    void VolumeDown ()
    {
        if(isSleep && music.volume > 0)
        {
            music.volume -= SpeedVolumeDown * Time.deltaTime;
            music2.volume -= SpeedVolumeDown * Time.deltaTime;
        }
    }

    void VolumeUp()
    {
        if (music.volume <= 0.3f)
        {
            music.volume += SpeedVolumeUp * Time.deltaTime;
        }

        if (music2.volume <= 0.4f)
        {
            music2.volume += SpeedVolumeUp * Time.deltaTime;
        }
    }
}
