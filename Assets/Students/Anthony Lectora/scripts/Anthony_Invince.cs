using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anthony_Invince : GenericPower
{
    private float Cooldown = 3;
    private float Invincible = 1.5f;

    private bool isInvincible;

    public override void Activate()
    {
        StartCoroutine(BecomeTemporarilyInvincible());

    }
    private IEnumerator BecomeTemporarilyInvincible()
    {
        
        Player.GetComponent<PlayerController>().HP = 99999999f;
          
        yield return new WaitForSeconds(Invincible);

        Player.GetComponent<PlayerController>().HP = 0;

        
    }
}
