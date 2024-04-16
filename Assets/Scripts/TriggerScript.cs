using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : TriggerableController
{
    [Header("Ignore Below")]
    public MeshRenderer MR;
    protected List<GameObject> Inside = new List<GameObject>();
    [Header("Customizable")] 
    public bool Vanish = true;
    public LayerMask CollideWith;
    public bool OneTime = false;
    
    void Start()
    {
        if (MR != null && Vanish)
            MR.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject go = other.attachedRigidbody.gameObject;
        if ((CollideWith & (1 << go.layer)) == 0) return;
        if (Inside.Contains(go)) return;
        Inside.Add(go);
        Trigger(go);
    }

    void OnTriggerExit(Collider other)
    {
        GameObject go = other.attachedRigidbody.gameObject;
        if (Inside.Contains(go))
        {
            Inside.Remove(go);
            Untrigger(go);
        }
    }

    public virtual void Trigger(GameObject go)
    {
        if (OneTime)
        {
            gameObject.SetActive(false);
            Inside.Clear();
        }
    }
    
    public virtual void Untrigger (GameObject go)
    {
        if (OneTime)
        {
            gameObject.SetActive(false);
            Inside.Clear();
        }
    }
}