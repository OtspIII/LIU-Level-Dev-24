using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DashPower : GenericPower
{
    public float Timer = 0;
    public float Speed = 30;
    public float Duration = 0.2f;
    public int MaxDashes = 1;
    int Dashes;

    private void Start()
    {
        Dashes = MaxDashes;
    }

    public override void Activate()
    {
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
    
    void Update()
    {
        if (Timer > 0)  
        {
            Timer -= Time.deltaTime / Duration;
            Player.Body.transform.rotation = Quaternion.Euler(0, 0, Timer * 360);
            if (Timer <= 0)
            {
                Player.RB.gravityScale = Player.Gravity; 
                Player.Body.transform.rotation = Quaternion.Euler(0,0,0);
                Player.SetInControl(true);
            }
        }

        if (Player.OnGround()) Dashes = MaxDashes;
    }
}
