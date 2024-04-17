using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBox : MonoBehaviour
{
    public ActorController Shooter;
    public Animator Anim;
    public BoxCollider Coll;
    private JSONWeapon Data;
    public ActorController Hit;
    public GameObject Sword;

    void Awake()
    {
        Shooter = gameObject.GetComponentInParent<ActorController>();
        if (Shooter != null && Shooter is NPCController)
        {
            Coll.gameObject.layer = 11;
            gameObject.layer = 11;
        }
            
    }

    public void Swing(JSONWeapon wpn)
    {
        Hit = null;
        Data = wpn;
        Anim.speed = 1/wpn.RateOfFire;
        Anim.Play("MeleeSwing");
        //Debug.Log("SWING: " + wpn.RateOfFire + " / " + Anim.speed);
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("OTE: " + other.name);
        ActorController pc = other.GetComponentInParent<ActorController>();
        if (pc == Shooter || (pc != null && pc == Hit)) return;
        if ((pc is NPCController && Shooter is NPCController)) return;
        if (pc != null)
        {
            pc.TakeDamage(Data.Damage,Shooter);
            if(Data.Knockback >0 && Data.ExplodeRadius <= 0)
                pc.TakeKnockback(transform.forward * Data.Knockback);
            Hit = pc;
        }
        ParticleGnome partic = pc != null ? God.Library.Blood : God.Library.Dust;
        ParticleGnome pg = Instantiate(partic, transform.position + (transform.forward * 2), Quaternion.identity);
        pg.Setup(Data.Damage / 5);

    }
}
