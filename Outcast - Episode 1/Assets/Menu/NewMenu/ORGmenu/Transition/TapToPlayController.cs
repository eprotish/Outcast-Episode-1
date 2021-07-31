using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapToPlayController : MonoBehaviour
{
    public GameObject ttp;
    public Button ttpWhole;
    public float ttpDelayTime;
    // Start is called before the first frame update
    void Start()
    {
       // ttp.SetActive(false);
      //  ttpWhole.enabled = false;
       // StartCoroutine(ShowTTP());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ShowTTP()
    {
        yield return new WaitForSeconds(ttpDelayTime);
       // ttp.SetActive(true);
       // ttpWhole.enabled = true;
    }
}
