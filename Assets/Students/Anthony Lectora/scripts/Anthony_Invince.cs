using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anthony_Invince : GenericPower
{
    // private float Cooldown = 3;
    private float Invincible = 1.5f;
    //
    // private bool isInvincible;
    //Dash Script Vars
    public float Timer = 0;
    public float Speed = 30;
    public float Duration = 0.2f;
    public int MaxDashes = 1;
    int Dashes;
    
    //PlayerController Script Vars
    public KeyCode AbilityButton2 = KeyCode.X;
    public KeyCode AbilityButton3 = KeyCode.V;
    private GenericPower Power;
    public static bool HasMoved = false;
    
    //Christian Ability Copy
    private bool isActive;
    private bool hasLanded = true;
    public GameObject dottedLine;
    private float coolDown;
    
    //private Vector3 topPos = new Vector3(3.53f, 0.72f, 0f);
    //private Vector3 topPos = new Vector3(3.53f, 0.9f, 0f);
    private Vector3 topPos = new Vector3(2.91f, 1.22f, 0f);
    private Vector3 startPos = new Vector3(2.91f, 0f, 0f);

    //private Vector3 bottomPos = new Vector3(3.53f, -0.56f, 0f);
    //private Vector3 bottomPos = new Vector3(3.53f, -0.56f, 0f);
    private Vector3 bottomPos = new Vector3(2.91f, -0.56f, 0f);
    //private Vector3 topRot = new Vector3(0f, 0f, 18.36f);
    private Vector3 topRot = new Vector3(0f, 0f, 31.22f);
    private Vector3 bottomRot = new Vector3(0f, 0f, -11.36f);
    private float loopTimer;
    private bool movingDown;
    private float gravity;
    private float timerMax;


    public void Start()
    {
        Dashes = MaxDashes;
        Power = GetComponent<GenericPower>();
        HasMoved = false;
        // gravity = 1.25f;
        // timerMax = .5f;


    }



    public override void Activate()
    {
    }
    // if (coolDown > 0 || isActive) return;
        // dottedLine.transform.position = topPos;
        // dottedLine.transform.localEulerAngles = topRot;
        // dottedLine.SetActive(true);
        // isActive = true;
        // movingDown = true;
        // loopTimer = 0f;
        // coolDown = 6f;
        // GetComponent<PlayerController>().enabled = false;
        // GetComponent<Rigidbody2D>().velocity = new Vector3();
    
    private IEnumerator BecomeTemporarilyInvincible()
    {
        
        Player.HP = 9999f;
          
        yield return new WaitForSeconds(Invincible);

        Player.HP = 0;
        
    }
    void Update()
    {
        if (Input.GetKeyDown(AbilityButton3))
        {
            StartCoroutine(BecomeTemporarilyInvincible());
        }

        if (  Input.GetKeyDown(AbilityButton2)) //Input.GetKey(KeyCode.LeftShift) ||
        {
            HasMoved = true;
            
            if (Dashes <= 0 && MaxDashes >= 0) return;
            Dashes--;
            Timer = 1;
            Player.SetInControl(false);
            Player.RB.gravityScale = 0;
            Player.KBVel = Vector2.zero;
            Player.KBDesired = Vector2.zero;
            Vector2 dir = new Vector2(0,0);
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                dir.x = 1;
            else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                dir.x = -1;
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
                dir.y = 1;
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
                dir.y = -1;
            if (dir == Vector2.zero) dir.x = Player.FaceLeft ? -1 : 1;
            Player.RB.velocity = dir.normalized * Speed;
        }

        if (Timer > 0)
        {
            Timer -= Time.deltaTime / Duration;
            Player.Body.transform.rotation = Quaternion.Euler(0, 0, Timer * 360);
            if (Timer <= 0)
            {
                Player.RB.gravityScale = Player.Gravity;
                Player.Body.transform.rotation = Quaternion.Euler(0, 0, 0);
                Player.SetInControl(true);
            }
           
          
            
        }
        if (Player.OnGround()) Dashes = MaxDashes;
        //Christians code came from void override
        
        //   if (coolDown > 0) coolDown -= Time.deltaTime;
        // if (isActive)
        // {
        //     if (Input.GetKeyDown(KeyCode.LeftShift) && hasLanded)
        //     {
        //         isActive = false;
        //         coolDown = 1f;
        //         dottedLine.SetActive(false);
        //         GetComponent<PlayerController>().enabled = true;
        //         return;
        //     }
        //     if (movingDown)
        //     {
        //         dottedLine.transform.localPosition = Vector3.Lerp(topPos, bottomPos, loopTimer / timerMax);
        //         dottedLine.transform.localEulerAngles = Vector3.Lerp(topRot, bottomRot, loopTimer/ timerMax);
        //         if (dottedLine.transform.localPosition.y < bottomPos.y)
        //         {
        //             dottedLine.transform.localPosition = bottomPos;
        //         }
        //     }
        //
        //     if (!movingDown)
        //     {
        //         dottedLine.transform.localPosition = Vector3.Lerp(bottomPos, topPos, loopTimer / timerMax);
        //         dottedLine.transform.localEulerAngles = Vector3.Lerp(bottomRot, topRot, loopTimer/ timerMax);
        //         if (dottedLine.transform.localPosition.y > topPos.y)
        //         {
        //             dottedLine.transform.localPosition = topPos;
        //         }
        //     }
        //     loopTimer += Time.deltaTime;
        //     if (loopTimer > timerMax)
        //     {
        //         movingDown = !movingDown;
        //         loopTimer = 0f;
        //     }
        //     if (Input.GetKeyUp(KeyCode.X))
        //     {
        //         isActive = false;
        //         coolDown = 1f;
        //         dottedLine.SetActive(false);
        //
        //         Vector2 direction = new Vector2();
        //         if (GetComponent<PlayerController>().FaceLeft) direction = new Vector2(-dottedLine.transform.right.x, -dottedLine.transform.right.y*1.2f);
        //         else direction = new Vector2(dottedLine.transform.right.x, dottedLine.transform.right.y*1.2f);
        //         GetComponent<Rigidbody2D>().velocity = new Vector3(direction.x * 11f, direction.y * 14f, 0f);
        //         GetComponent<Rigidbody2D>().gravityScale = gravity;
        //         isActive = false;
        //         hasLanded = false;
        //     }
            
        //}
    }
}

