using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormController : TriggerableController
{
	[Header("Customizable")]
	public float ShrinkSpeed = 1;
	public float MinSize = 10;
	public bool StartOn = true;
	[Header("Ignore Below")]
	public SphereCollider Coll;
	private float StartSize;
	private float StartScale;
	private float Size;
	bool Active;

    void Start()
    {
	    Active = StartOn;
	    StartSize = Coll.radius;
	    Size = StartSize;
	    StartScale = transform.localScale.x;
    }

    void Update()
    {
	    if (Active && Size > MinSize)
	    {
		    Size -= ShrinkSpeed * Time.deltaTime;
		    float scl = (Size / StartSize) * StartScale;
		    transform.localScale = new Vector3(scl, scl, scl);
	    }
    }

    private void OnTriggerExit(Collider other)
    {
	    //Debug.Log("THEY LEFT ME: " + other.gameObject);
	    other.gameObject.SendMessage("OnStormEnter",SendMessageOptions.DontRequireReceiver);
    }

    private void OnTriggerEnter(Collider other)
    {
	    other.gameObject.SendMessage("OnStormExit",SendMessageOptions.DontRequireReceiver);
    }

    public override void Trigger(string type = "", GameObject target = null)
    {
	    base.Trigger(type,target);
	    switch (type)
	    {
		    case "Start":
		    {
			    Active = true;
			    break;
		    }
		    case "Stop":case "Pause":
		    {
			    Active = false;
			    break;
		    }
		    case "Toggle":
		    {
			    Active = !Active;
			    break;
		    }
	    }
    }
}
