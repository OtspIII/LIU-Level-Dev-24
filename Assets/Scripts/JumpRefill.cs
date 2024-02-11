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
            OffTimer -= Time.deltaTime;
            if (OffTimer <= 0)
            {
                SR.color = StartColor;
            }
        }
    }

    public void Trigger(PlayerController pc)
    {
        if (pc == null || pc.AirJumps == pc.MaxAirJumps) return;
            pc.AirJumps = pc.MaxAirJumps;
        OffTimer = ResetTime;
        SR.color = OffColor;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (OffTimer > 0) return;
        Trigger(other.gameObject.GetComponent<PlayerController>());
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (OffTimer > 0) return;
        Trigger(other.gameObject.GetComponent<PlayerController>());
    }
}
