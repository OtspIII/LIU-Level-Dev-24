﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

public class PlayerController : CharController
{
    public static bool Moved = false;
    public float Invincible = 0;
    public List<int> Keys;

    public override void OnAwake()
    {
        base.OnAwake();
        Player = true;
        Moved = false;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        GameManager.Me.HPText.text = "HP: " + HP;
        if (!Alive)
        {
            SR.color = Color.black;
            RB.velocity = Vector2.zero;
            return;
        }
        bool input = false;
        Vector2 vel = RB.velocity;

        if (Input.GetKey(KeyCode.D))
            vel.x = Data.Speed;
        else if (Input.GetKey(KeyCode.A))
            vel.x = -Data.Speed;
        else
            vel.x = 0;
        if (Input.GetKey(KeyCode.W))
            vel.y = Data.Speed;
        else if (Input.GetKey(KeyCode.S))
            vel.y = -Data.Speed;
        else
            vel.y = 0;

        vel += Knock;
        
        RB.velocity = vel;

        Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float aimAt = Mathf.Atan2(transform.position.y - target.y,
                            transform.position.x - target.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, aimAt);

        if (Input.GetMouseButton(0))
        {
            Shoot();
            input = true;
        }

        if (vel != Vector2.zero) input = true;
        
        if (!Moved && input)
        {
            Moved = true;
            if (!GameManager.LevelMode)
                foreach(EnemyController e in GameManager.Me.Enemies)
                {
                    e.Activate();
                }
        }

        if (Invincible > 0)
        {
            Invincible -= Time.deltaTime;
            Color c = SR.color;
            if (Invincible > 0)
                c.a = (int) (Time.time * 6) % 2 == 0 ? 1 : 0.5f;
            else
                c.a = 1;
            SR.color = c;
        }

    }

    public override void Reset()
    {
        base.Reset();
        Moved = false;
        RB.velocity = Vector2.zero;
        Keys.Clear();
        Alive = true;
        Coll.enabled = true;
        if(JSON.Sprite == null)
            SetColor(Data.Color);
    }

    public override void TakeDamage(int amt)
    {
        if (Invincible > 0) return;
        base.TakeDamage(amt);
        GameManager.Me.HPText.text = "HP: " + HP;
        if(amt > 0)
            Invincible = 0.5f;
    }

    public override void Die()
    {
        if(Alive)
            GameManager.Me.GameOver();
        base.Die();
        
    }

    public void GetKey(KeyController k)
    {
        Keys.Add(k.Number);
    }
}
