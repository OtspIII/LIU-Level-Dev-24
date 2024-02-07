using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danny_Ability : GenericPower
{
    public BoxCollider2D BS;
    public BoxCollider2D BSF;
    public SpriteRenderer SR;

    public override void Activate()
    {
        StartCoroutine(Ability());
    }

    public IEnumerator Ability()
    {
        BS.enabled = false; 
        BSF.enabled = false;
        SR.color=Color.black;
        yield return new WaitForSeconds(0.6f);
        BS.enabled = true; 
        BSF.enabled = true;
        SR.color=Color.white;
    }
}
