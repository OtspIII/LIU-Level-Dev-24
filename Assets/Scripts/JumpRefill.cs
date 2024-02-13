using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpRefill : MonoBehaviour
{
    public SpriteRenderer SR;
    public float ResetTime = 2;
    private float OffTimer = 0;
    private Color StartColor;
    public Color OffColor = new Color(0,1,1,0);
    bool JustTriggered = false;

    private void Start()
    {
        StartColor = SR.color;
        if (OffColor == new Color(0,1,1,0))
        {
            OffColor = Color.Lerp(StartColor, Color.clear, 0.5f);
        }
    }

    void Update()
    {
        if (OffTimer > 0)
        {
            if (!JustTriggered)
            {
                OffTimer -= Time.deltaTime;
                if (OffTimer <= 0)
                {
                    SR.color = StartColor;
                }
            }
        }
    }

    public void Trigger(PlayerController pc)
    {
        if (OffTimer > 0) return;
        if (pc.AirJumps == pc.MaxAirJumps) return;
        pc.AirJumps = pc.MaxAirJumps;
        Cooldown();
    }

    public void Cooldown(float time=-123)
    {
        if (time == -123) time = ResetTime;
        OffTimer = time;
        if (OffTimer < 0) OffTimer = 99999;
        else if (OffTimer == 0) OffTimer = 0.1f;
        SR.color = OffColor;
        JustTriggered = true;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController pc = other.gameObject.GetComponent<PlayerController>();
        if (pc == null) return;
        if(pc.MaxAirJumps > 0)
            Trigger(pc);
        else
            pc.AirJumps = 1;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        
        PlayerController pc = other.gameObject.GetComponent<PlayerController>();
        if (pc == null) return;
        if (pc.MaxAirJumps > 0)
            Trigger(pc);
        else
        {
            if (pc.AirJumps != 1)
                Cooldown();
            pc.AirJumps = 0;
        }
        if (JustTriggered)
        {
            JustTriggered = false;
            return;
        }
    }
}
