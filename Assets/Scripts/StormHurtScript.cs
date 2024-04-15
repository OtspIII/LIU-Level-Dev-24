using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormHurtScript : MonoBehaviour
{
    public ActorController Who;
    public bool InStorm = false;
    public float DamageCountdown = 1;
    public int DamageAmount = 1;
    private float Timer = 0;

    void Start()
    {
        if(Who == null)
            Who = gameObject.GetComponent<ActorController>();
    }
    
    void Update()
    {
        if (InStorm)
        {
            Timer += Time.deltaTime;
            if (Timer >= DamageCountdown)
            {
                Who.TakeDamage(DamageAmount);
                Timer = 0;
            }
        }
    }

    public void OnStormEnter()
    {
        InStorm = true;
    }

    public void OnStormExit()
    {
        InStorm = false;
    }
}
