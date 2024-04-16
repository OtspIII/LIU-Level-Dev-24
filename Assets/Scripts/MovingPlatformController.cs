using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : TriggerableController
{
    public List<Vector3> Destinations;
    private int CurrentDest;
    public float Speed = 1f;
    public bool StartOn = true;
    private List<Transform> Riders = new List<Transform>();
    bool Active;

    void Start()
    {
        Active = StartOn;
    }
    
    void FixedUpdate()
    {
        if (Destinations.Count == 0 || !Active) return; // || !PlayerController.HasMoved
        Vector3 dest = Destinations[CurrentDest];
        Vector3 old = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, dest, Speed * Time.fixedDeltaTime);
        Vector3 movement = transform.position - old;
        foreach (Transform tra in Riders)
        {
            tra.position += movement;
        }
        if (Vector3.Distance(transform.position, dest) < 0.01f)
        {
            CurrentDest++;
            if (CurrentDest >= Destinations.Count)
                CurrentDest = 0;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!Riders.Contains(other.transform))
            Riders.Add(other.transform);
    }

    private void OnCollisionExit(Collision other)
    {
        Riders.Remove(other.transform);
    }

    public override void Trigger(TriggerMessages type=TriggerMessages.None, GameObject target = null)
    {
        base.Trigger(type, target);
        switch (type)
        {
            case TriggerMessages.Start:
            {
                Active = true;
                break;
            }
            case TriggerMessages.Stop:
            {
                Active = false;
                break;
            }
            case TriggerMessages.Toggle:
            {
                Active = !Active;
                break;
            }
        }
    }
}