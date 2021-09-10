using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSliderController : MonoBehaviour
{
    public Button slideRight;
    public Button slideLeft;
    public Text range;
    public int currentCounter = 1;
    public int total = 5;
    // Start is called before the first frame update
    void Start()
    {
        slideLeft.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        range.text = currentCounter + "/" + total;
    }

    public void SlideRight()
    {
        if(currentCounter + 1 == total)
        {
            currentCounter = total;
            slideRight.interactable = false;

            if (!slideLeft.interactable)
                slideLeft.interactable = true;
        }
        else
        {
            currentCounter += 1;
            if (!slideLeft.interactable)
                slideLeft.interactable = true;
        }
    }

    public void SlideLeft()
    {
        if (currentCounter - 1 == 0)
        {
            currentCounter = 1;
            slideLeft.interactable = false;

            if (!slideRight.interactable)
                slideRight.interactable = true;
        }
        else
        {
            currentCounter -= 1;
            if (!slideRight.interactable)
                slideRight.interactable = true;
        }
    }
}
