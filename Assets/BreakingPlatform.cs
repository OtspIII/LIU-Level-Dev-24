using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BreakingPlatform : MonoBehaviour
{
    public float Timer;
    public float TimeBeforeBreak = 3;
    public float TimeBeforeReform = 1;
    public BreakState State = BreakState.Idle;

    public Collider2D Collider;
    public SpriteRenderer Body;
    Color StartColor;
    Color FadeColor;

    void Start(){
        StartColor = Body.color;
        FadeColor = Body.color;
        FadeColor.a = 0.1f;
        Timer = TimeBeforeBreak;
    }
    
    void Update()
    {
        if (State == BreakState.Breaking || State == BreakState.TempBroke)
        {
            Timer -= Time.deltaTime;
            if (State == BreakState.Breaking)
            {
                Body.transform.localPosition = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0);
            }

            if (Timer <= 0)
            {
                Body.transform.localPosition = Vector3.zero;
                if (State == BreakState.TempBroke)
                {
                    Collider.enabled = true;
                    Body.color = StartColor;
                    Timer = TimeBeforeBreak;
                    State = BreakState.Idle;
                }
                else
                {
                    if(TimeBeforeReform > 0)
                        State = BreakState.TempBroke;
                    else
                        State = BreakState.Broke;
                    Collider.enabled = false;
                    Body.color = FadeColor;
                    Timer = TimeBeforeReform;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(State == BreakState.Idle)
            State = BreakState.Breaking;
    }

    public enum BreakState
    {
        None,
        Idle,
        Breaking,
        Broke,
        TempBroke
    }
}
