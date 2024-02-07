using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AMLAbility : GenericPower
{

    public float Timer = 0;
    public float Speed = 30;
    public float Duration = 0.2f;
    public bool OnWall = false;

    private void Awake()
    {
        //Player = GetComponent<PlayerController>();
    }

    //This gets called whenever the player hits 'X'
    public override void Activate()
    {

        if (OnWall == false) return;

        //Player.MaxAirJumps = 1;
        
        
        
        Timer = 0.3f;
        
        Player.SetInControl(false);
        Player.RB.gravityScale = 0;
        Player.KBVel = Vector2.zero;
        Player.KBDesired = Vector2.zero;
        Vector2 dir = new Vector2(0,0);
        
        dir.y = 5;
        Player.RB.velocity = dir.normalized * Speed;
        
    }

    public void Update()
    {
        
        if (Timer > 0)  
        {
            Timer -= Time.deltaTime / Duration;
            //Player.Body.transform.rotation = Quaternion.Euler(0, 0, Timer * 360);
            if (Timer <= 0)
            {
                Player.RB.gravityScale = Player.Gravity;
                Player.SetInControl(true);
                //Player.Body.transform.rotation = Quaternion.Euler(0,0,0);
            }
        }
        
    }


    //This gets called when you die. If it returns true, the player doesn't run their normal death code
    //You can use this to make something alternate happen when the player dies
    public virtual bool DeathOverride(GameObject source){
        return false;
    }
	
    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.tag == "AlexWalls")
            OnWall = true;

    }

    private void OnCollisionExit2D(Collision2D other)
    {

        if (other.gameObject.tag == "AlexWalls")
        {
            
            OnWall = false;
            Player.MaxAirJumps = 0;

        }
        
    }
	
}