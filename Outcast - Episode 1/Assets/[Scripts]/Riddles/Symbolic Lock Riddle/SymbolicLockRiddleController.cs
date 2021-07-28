using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SymbolicLockRiddleController : MonoBehaviour
{
    public Sprite[] Symbols;

    public int[] LockSymbols = { 3, 3, 3, 3, 3, 3, 3, 3 };

    public int[] CorrectOrder = { 3, 5, 2, 4, 1, 4, 0, 7 };

    public bool isCorrect = false;
    public bool finished = false;

    public Text order;

    public RollButton[] btns;
    public Animator[] anim;

    public Animator LockHandleAnim;
    // Start is called before the first frame update

    public bool canClick = true;

    [SerializeField] private GameObject TriggerPedram;
    [SerializeField] private GameObject ThisInteraction;
    [SerializeField] private GameObject SafeInteraction;

    void Start()
    {
        if(GameObject.FindObjectOfType<Step>().Steps[27])
        {
            SafeInteraction.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isCorrect && !finished)
        {
            GameObject.FindObjectOfType<Step>().DoWork(27);
            finished = true;
            LockHandleAnim.SetTrigger("Open");
            TriggerPedram.SetActive(true);
            ThisInteraction.SetActive(false);
            StartCoroutine(CloseThisPanel(1.5f));
            SafeInteraction.SetActive(true);
        }

        //
        if (Input.GetKeyDown(KeyCode.Space))
            Test();
    }

    public  void Test ()
    {
        isCorrect = true;
    }


    IEnumerator UpwardRoll(int slotIndex)
    {
        canClick = false;
        anim[slotIndex].SetTrigger("RollUp");
        yield return new WaitForSeconds(1.01f);
        btns[slotIndex].Up.sprite = Symbols[LockSymbols[slotIndex]];
        LockSymbols[slotIndex] = (LockSymbols[slotIndex] + 1) % LockSymbols.Length;

        btns[slotIndex].Middle.sprite = Symbols[LockSymbols[slotIndex]];
        int up1Index = (LockSymbols[slotIndex] - 2) < 0 ? (LockSymbols[slotIndex] - 2) + LockSymbols.Length : (LockSymbols[slotIndex] - 2);
        int down1Index = (LockSymbols[slotIndex] + 2) % LockSymbols.Length;

        int index2 = (LockSymbols[slotIndex] + 1) % LockSymbols.Length;

        btns[slotIndex].Down.sprite = Symbols[index2];

        btns[slotIndex].Up1.sprite = Symbols[up1Index];
        btns[slotIndex].Down1.sprite = Symbols[down1Index];

        isCorrect = CheckOrder();
        canClick = true;
        //PrintOrder();
    }

    IEnumerator DownwardRoll(int slotIndex)
    {
        canClick = false;
        anim[slotIndex].SetTrigger("RollDown");
        yield return new WaitForSeconds(1.01f);
        btns[slotIndex].Down.sprite = Symbols[LockSymbols[slotIndex]];
        LockSymbols[slotIndex] = (LockSymbols[slotIndex] - 1) < 0 ? (LockSymbols[slotIndex] - 1) + LockSymbols.Length : (LockSymbols[slotIndex] - 1);

        btns[slotIndex].Middle.sprite = Symbols[LockSymbols[slotIndex]];
        //int up1Index = (LockSymbols[slotIndex] - 2) < 0 ? (LockSymbols[slotIndex] - 2) + LockSymbols.Length : (LockSymbols[slotIndex] - 2);
        //int down1Index = (LockSymbols[slotIndex] + 2) % LockSymbols.Length;

        int index2 = (LockSymbols[slotIndex] - 1) < 0 ? (LockSymbols[slotIndex] - 1) + LockSymbols.Length : (LockSymbols[slotIndex] - 1);

        btns[slotIndex].Up.sprite = Symbols[index2];

        //btns[slotIndex].Up1.sprite = Symbols[up1Index];
        //btns[slotIndex].Down1.sprite = Symbols[down1Index];

        isCorrect = CheckOrder();
        canClick = true;
        //PrintOrder();
    }

    public void UpwardSlot(int slotIndex)
    {
        if(canClick)
            StartCoroutine(UpwardRoll(slotIndex));
    }

    public void DownwardSlot(int slotIndex)
    {
        if(canClick)
            StartCoroutine(DownwardRoll(slotIndex));
    }

    public void PrintOrder()
    {
        string s = "{ ";
        for (int i = 0; i < LockSymbols.Length; i++)
        {
            s += LockSymbols[i].ToString() + ", ";
        }

        s += "}";

        order.text = s;
    }

    public bool CheckOrder()
    {
        for(int i = 0; i < LockSymbols.Length; i++)
        {
            if (LockSymbols[i] != CorrectOrder[i])
                return false;
        }

        return true;
    }

    [Serializable]
    public struct RollButton
    {
        public Image Up1;
        public Image Up;
        public Image Middle;
        public Image Down;
        public Image Down1;
    }

    IEnumerator CloseThisPanel (float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        LockHandleAnim.transform.parent.gameObject.SetActive(false);
    }
}
