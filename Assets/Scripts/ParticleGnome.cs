using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ParticleGnome : NetworkBehaviour
{
    public ParticleSystem PS;
    public bool IsSetup = false;
    public int Amount = 0;

    void Update()
    {
        if (!IsSetup && Amount > 0)
        {
            IsSetup = true;
            PS.Emit(Amount);
        }
    }

    
    public void Setup(int amt)
    {
        PS.Emit(amt);
        Invoke("TimeUp",1);
        Amount = amt;
    }

    public void TimeUp()
    {
        Destroy(gameObject);
    } 
}
