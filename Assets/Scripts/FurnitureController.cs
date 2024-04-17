using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureController : ActorController
{
    
    public int MaxHP = 1;
    public float ExplosionSize = 0;
    public float ExplosionDamage = 0;
    public float ExplosionKnockback = 0;
    public GameObject Drop; 

    public override void OnStart()
    {
        base.OnStart();
        HP = MaxHP;
    }

    public override int GetMaxHP()
    {
        return MaxHP;
    }

    public override void Die(ActorController source = null)
    {
        if (Drop != null)
        {
            Drop.transform.parent = null;
            Drop.gameObject.SetActive(true);
        }

        base.Die(source);
        if (ExplosionSize > 0)
        {
            ExplosionController exp = Instantiate(God.Library.Explosion, transform.position, Quaternion.Euler(0,0,0));
            exp.Setup(ExplosionSize,ExplosionDamage,ExplosionKnockback);
            Destroy(gameObject);
        }
    }
}
