using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class Tutorail : MonoBehaviour
{
    [SerializeField] private GameObject Controller;
    [SerializeField] [TextArea] string [] infoTutorail;
    [SerializeField] private GameObject TutorailPanel;
    [SerializeField] private Text TutorailText;
    [SerializeField] private Image [] MoveButtons;
    [SerializeField] private Image RunButton;

    private PlayerMovement playerMovment;
    private Rigidbody2D rb;

    private void Start()
    {
        playerMovment = GameObject.FindObjectOfType<PlayerMovement>();
        rb = playerMovment.gameObject.GetComponent<Rigidbody2D>();
    }

    private void PauseGame ()
    {
        if(TutorailPanel.activeInHierarchy)
        {
            rb.bodyType = RigidbodyType2D.Static;
            Time.timeScale = 0;
        }
    }

    private void UnPauseGame()
    {
        Time.timeScale = 1;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public void TutorailShow (int a)
    {

        StartCoroutine(PauseChange(true));

        playerMovment.RunStop();
        playerMovment.Stop();
        playerMovment.StateChecker();

        TutorailPanel.SetActive(true);
        if (a == 1)
        {
            MoveButtons[0].color = new Color(255, 255, 255, 255);
            MoveButtons[1].color = new Color(255, 255, 255, 255);

            TutorailText.text = infoTutorail[0];
        }

        if(a == 2)
        {
            RunButton.color = new Color(255, 255, 255, 255);
            TutorailText.text = infoTutorail[1];
            RunButton.transform.parent.gameObject.SetActive(true);
        }

        if(a == 3)
        {
            TutorailText.text = infoTutorail[2];
        }

        if (a == 4)
        {
            TutorailText.text = infoTutorail[3];

        }

    }

    public void TutorailShowOff (int a)
    {
        UnPauseGame();
        CancelInvoke("PauseChange");

        TutorailPanel.SetActive(false);

        if (TutorailPanel.activeInHierarchy && a == 1)
        {
            MoveButtons[0].color = new Color(255, 255, 255, 0.823f);
            MoveButtons[1].color = new Color(255, 255, 255, 0.823f);
        }

        if(TutorailPanel.activeInHierarchy && a == 2)
        {
            RunButton.color = new Color(255, 255, 255, 0.823f);
        }

    }

    IEnumerator PauseChange (bool ispause)
    {
        if(ispause)
        {
            yield return new WaitForSeconds(1);
            PauseGame();
        }
        else
        {
            UnPauseGame();
        }
    }  
}
