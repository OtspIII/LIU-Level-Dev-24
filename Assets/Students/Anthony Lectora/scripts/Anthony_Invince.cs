using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anthony_Invince : GenericPower
{
    // private float Cooldown = 3;
    private float Invincible = 1.5f;
    //
    // private bool isInvincible;

    public override void Activate()
    {
        StartCoroutine(BecomeTemporarilyInvincible());

    }
    private IEnumerator BecomeTemporarilyInvincible()
    {
        
        Player.HP = 9999f;
          
        yield return new WaitForSeconds(Invincible);

        Player.HP = 0;
        
    }
}
