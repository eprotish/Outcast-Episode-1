using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBubble : MonoBehaviour
{

    public Text textBubble;
    public float timeBetweenSubstrings = 0.125f;

    CharacterController2D controller2D;

    public bool facingRight = true;
    public float localX = -23f;

    public Transform ChangePos;
    private Vector3 orginPos;
    private Transform Holder;

    private void Start()
    {
        Holder = transform.GetChild(0);

        orginPos = Holder.position;

        controller2D = FindObjectOfType<CharacterController2D>();
        facingRight = controller2D.IsFacingRight();
    }

    private void Update()
    {

        if (!controller2D.IsFacingRight() && facingRight)
        {
            facingRight = controller2D.IsFacingRight();
            Flip(true);
        }
        if (controller2D.IsFacingRight() && !facingRight)
        {
            facingRight = controller2D.IsFacingRight();
            Flip(false);
        }

    }
    public void TypeText(string str)
    {
       // StopCoroutine(TypeWriter(str));
       // StartCoroutine(TypeWriter(str));
        //textBubble.text = str;
    }

    public void SetDialog (string content) {

      textBubble.text = content;
    }

    public void ClearText()
    {
        textBubble.text = "";
    }

    IEnumerator TypeWriter(string str)
    {
        string[] strs = str.Split(' ');
        List<string> substrings = new List<string>();
        for (int i = 0;  i < strs.Length; i++)
        {
            int end = 0;
            for (int j = 0; j <= i; j++)
            {
                end += strs[j].Length + 1;
            }
            if (end >= str.Length)
                end = str.Length - 1;
            substrings.Add(str.Substring(0, end+1));
        }

        foreach(string substring in substrings)
        {
            textBubble.text = substring;
            yield return new WaitForSeconds(timeBetweenSubstrings);
        }

    }

    void Flip(bool rtl)
    {
        Vector3 theScale = transform.GetChild(0).localScale;
        Vector3 localPos = transform.GetChild(0).localPosition;

        if (rtl)
        {
            theScale.x = -1;
            transform.localScale = theScale;
            localPos.x = localX;
        }
        else
        {
            theScale.x = 1;
            transform.localScale = theScale;
            localPos.x = 0;
        }
        transform.GetChild(0).localPosition = localPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            Holder.position = ChangePos.position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Wall")
        {
            Holder.position = orginPos;
        }
           
    }
}
