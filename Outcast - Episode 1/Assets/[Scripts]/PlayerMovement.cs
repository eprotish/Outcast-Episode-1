//using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.U2D.Animation;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    [SerializeField] private GameObject _Tutorail;
    [SerializeField] private GameObject _controller;
    public Animator animator;

    float horizontalMove = 0f;

    [SerializeField] private float WalkSpeed = 30f;
    [SerializeField] private float RunSpeed = 50f;
    [SerializeField] private float TimeCanRun = 10f;
    [SerializeField] private float EnergyBackSpeed = 0.5f;
    [SerializeField] private float TimeRest = 5;
    
    private float timeRest;
    private float timeEnergy; 
    public MoveMode moveMode;

    private Step _step;

    private bool CantMoveRight = false;
    private bool CantMoveLeft = false;

    public AudioSource audioSource;
    public AudioClip[] MoveSounds;

    public float BetweenTimeWalk;
    public float BetweenTimeRun;

    private int SoundIn;

    [HideInInspector] public bool InInteratcion; 

    void Start()
    {
        _step = GameObject.FindObjectOfType<Step>();

        if(_step.Steps[13] && !_step.Steps[39])
        {
            ChangeClothes(1);
        }

        timeRest = TimeRest;
        timeEnergy = TimeCanRun;
        moveMode = MoveMode.idle;

        animator.Play("Idle");

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Earth")
            return;

        if(horizontalMove > 0.1)
        {
            CantMoveRight = true;
        }
        else if (horizontalMove < -0.1)
        {
            CantMoveLeft = true;
        }

        if (moveMode == MoveMode.run)
            RunStop();

           Stop();

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        CantMoveRight = false;
        CantMoveLeft = false;
    }

    void Update()
    {
       // animator.SetFloat("Speed", Mathf.Abs(horizontalMove));       
    }

    void FixedUpdate ()
    {
        // for pc
        if (Input.GetKeyDown(KeyCode.LeftShift) && moveMode != MoveMode.noEnergy)
        {
            if(SceneManager.GetActiveScene().name == "Scene2")
            {
                _Tutorail.GetComponent<Tutorail>().TutorailShowOff(2);
            }
            moveMode = MoveMode.run;
            //animator.SetBool("Run",true);
            animator.Play("Artan_Runing_Anim_Usef(Edit_3)");
            
        }
 
        if (Input.GetKeyUp(KeyCode.LeftShift) && moveMode != MoveMode.noEnergy)
        {
            moveMode = MoveMode.walk;
            //animator.SetBool("Run",false);
            if(horizontalMove != 0)
                animator.Play("Artan_Walking");
        }

        StateChecker();    
    }

    public void StateChecker ()
    {

        if (moveMode == MoveMode.idle)
        {
            if (timeEnergy < TimeCanRun)
            {
                timeEnergy += (EnergyBackSpeed * 2 * Time.fixedDeltaTime);
            }
        }

        else if (moveMode == MoveMode.walk)
        {
            controller.Move(horizontalMove * WalkSpeed * Time.fixedDeltaTime, false, false);
            if (timeEnergy < TimeCanRun)
            {
                timeEnergy += EnergyBackSpeed * Time.fixedDeltaTime;
            }
        }

        else if (moveMode == MoveMode.run)
        {
            controller.Move(horizontalMove * RunSpeed * Time.fixedDeltaTime, false, false);
            if (timeEnergy > 0)
            {
                timeEnergy -= Time.fixedDeltaTime;
            }
            else
            {

                //animator.SetBool("NoEnergy", true);
                moveMode = MoveMode.noEnergy;

                animator.Play("Artan_Breathing&Tiring_Anim_Usef(Edit)");

                timeRest = TimeRest;
                timeEnergy = 0.5f;

                if (SceneManager.GetActiveScene().name == "Scene 2")
                    _controller.GetComponent<Scene2>().CheckEvent(1);
            }
        }

        else if (moveMode == MoveMode.noEnergy)
        {
            if (timeRest > 0)
            {
                timeRest -= Time.fixedDeltaTime;
            }
            else
            {
                horizontalMove = 0;
                // animator.SetBool("Run", false);
                // animator.SetBool("NoEnergy", false);
                animator.Play("Idle");

                moveMode = MoveMode.idle;
            }
        }

    }

    public void MoveRight()
    {

        if (CantMoveRight || InInteratcion)
            return;

        if (moveMode != MoveMode.noEnergy)
        {
            moveMode = MoveMode.walk;
            horizontalMove = 1;

            animator.Play("Artan_Walking");

            SoundPlayer();
        }

    }
    
    public void MoveLeft()
    {
        if (CantMoveLeft || InInteratcion)
            return;

        if (moveMode != MoveMode.noEnergy)
        {
            moveMode = MoveMode.walk;
            horizontalMove = -1;

            animator.Play("Artan_Walking");

            SoundPlayer();
        }
    }

    public void Stop()
    {
        // Stop All
        RunStop();

        if (moveMode != MoveMode.noEnergy)
        {     
            moveMode = MoveMode.idle;

            animator.Play("Idle");

            horizontalMove = 0;
        }
    }

    public void Run()
    {
        if (moveMode == MoveMode.walk)
        {
            moveMode = MoveMode.run;
            // animator.SetBool("Run",true);
            animator.Play("Artan_runing_Anim_Usef(Edit_3)");
            SoundPlayer();
        }
    }

    public void RunStop()
    {
        if (moveMode != MoveMode.noEnergy)
        {
            moveMode = MoveMode.walk;
            //  animator.SetBool("Run",false);
            if (horizontalMove != 0)
                animator.Play("Artan_Walking");
        }


    }

    public void ChangeClothes (int a)
    {
        if (a == 0)
        {
            // orginal

            transform.GetChild(2)
                .GetComponent<SpriteResolver>().SetCategoryAndLabel("BaseBody", "New");

            transform.GetChild(10)
               .GetComponent<SpriteResolver>().SetCategoryAndLabel("LeftArm", "New");

            transform.GetChild(13)
              .GetComponent<SpriteResolver>().SetCategoryAndLabel("LeftUpArm", "New");

            transform.GetChild(18)
              .GetComponent<SpriteResolver>().SetCategoryAndLabel("RightArm", "New");

            transform.GetChild(21)
              .GetComponent<SpriteResolver>().SetCategoryAndLabel("RightUpArm", "New");
        }

        if (a == 1)
        {
            // Lebas Zir

            transform.GetChild(2)
                .GetComponent<SpriteResolver>().SetCategoryAndLabel("BaseBody", "BlackRekabi");


            transform.GetChild(10)
               .GetComponent<SpriteResolver>().SetCategoryAndLabel("LeftArm", "BlackRekabi");

            transform.GetChild(13)
              .GetComponent<SpriteResolver>().SetCategoryAndLabel("LeftUpArm", "BlackRekabi");

            transform.GetChild(18)
              .GetComponent<SpriteResolver>().SetCategoryAndLabel("RightArm", "BlackRekabi");

            transform.GetChild(21)
              .GetComponent<SpriteResolver>().SetCategoryAndLabel("RightUpArm", "BlackRekabi");
        }
    }

    private void SoundPlayer ()
    {
        if(moveMode == MoveMode.walk)
        {
            Invoke("SoundPlayer", BetweenTimeWalk);
        }
        else if (moveMode == MoveMode.run)
        {
            Invoke("SoundPlayer", BetweenTimeRun);
        }
        else if( moveMode == MoveMode.idle)
        {
            CancelInvoke("SoundPlayer");
            return;
        }


        // audioSource.clip = MoveSounds[Random.Range(0, MoveSounds.Length)];
        audioSource.clip = MoveSounds[SoundIn];
        audioSource.Play();


        SoundIn++;

        if (SoundIn == MoveSounds.Length)
            SoundIn = 0;

    }
}


[System.Serializable]
public enum MoveMode
{
    idle , walk , run , noEnergy
}
