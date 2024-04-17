using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class DashPowerup : MonoBehaviour
{
    public FirstPersonController PC;
    public KeyCode Key = KeyCode.LeftShift;
    public float Duration = 0.5f;
    public float Speed = 30f;
    public bool AirDash = true;
    public bool HoldDown = false;
    public bool Slide = false;
    public bool Aim = false;
    public bool OnGround = false;
    public bool Momentum = true;

    private void Awake()
    {
        if (PC == null) PC = GetComponent<FirstPersonController>();
    }

    void Update()
    {
        if (!PC.InControl) return;
        if (Input.GetKeyDown(Key))
        {
            if (OnGround && !PC.OnGround()) return;
            PC.InControl = false;
            StartCoroutine(Run());
        }
    }

    public IEnumerator Run()
    {
        float timer = Duration;
        if (Slide) PC.Eyes.transform.localPosition -= new Vector3(0, 0.5f, 0);
        Transform src = AirDash ? PC.Eyes.transform : PC.transform;
        
        Vector3 dir = src.forward * Speed;
        while (timer > 0)
        {
            if (HoldDown) timer = Input.GetKey(Key) ? 1 : 0;
            else
                timer -= Time.fixedDeltaTime;
            if (Aim)
            {
                dir = src.forward * Speed;
            }

            if (!AirDash) dir.y = PC.RB.velocity.y;
            PC.RB.velocity = dir;
            yield return new WaitForFixedUpdate();
        }
        if (Slide) PC.Eyes.transform.localPosition += new Vector3(0, 0.5f, 0);
        PC.Fling = Vector3.zero;
        if(!Momentum) PC.RB.velocity = Vector3.zero;
        PC.InControl = true;
    }
}
