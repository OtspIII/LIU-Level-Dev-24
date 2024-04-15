using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class DoorController : TriggerableController
{
    [Header("Customizable")]
    public Vector3 Movement = new Vector3(0, 10, 0);
    public float Speed = 2;
    public bool AutoClose = true;
    public bool StartUp = false;
    
    [Header("Ignore Below")]
    public GameObject Body;
    private Vector3 DesiredPos;
    private bool Open = false;
    private Vector3 StartPos;
    public TriggerZoneScript Detector;

    void Start()
    {
        StartPos = Body.transform.position;
        DesiredPos = StartPos;
        if (StartUp)
        {
            DesiredPos = StartPos + Movement;
            Body.transform.position = DesiredPos;
        }

        if (AutoClose && Detector != null)
        {
            Detector.ExitMessage = "Close";
        }
    }

    private void Update()
    {
        if (DesiredPos != Body.transform.position)
        {
            Body.transform.position = Vector3.Lerp(Body.transform.position, DesiredPos, Time.deltaTime * Speed);
            Body.transform.position = Vector3.MoveTowards(Body.transform.position, DesiredPos, 0.01f);
        }
    }

    public override void Trigger(string type = "", GameObject target = null)
    {
        //if (Body.transform.position != DesiredPos) return;
        switch (type)
        {
            case "Open":case "Up":
            {
                DesiredPos = StartPos + Movement;
                Open = true;
                break;
            }
            case "Close":case "Down":
            {
                DesiredPos = StartPos;
                Open = true;
                break;
            }
            case "Toggle":
            {
                Open = !Open;
                if (Open)
                {
                    DesiredPos = StartPos + Movement;
                }
                else
                {
                    DesiredPos = StartPos;
                }
                break;
            }
        }
    }
}
