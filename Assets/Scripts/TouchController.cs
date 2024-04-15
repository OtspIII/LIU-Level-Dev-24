using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class TouchController : MonoBehaviour
{
    public TouchThings Type = TouchThings.Lava;
    public float Amount;
    public float KB = 10;
    public Vector3 KBDir;

    private void OnCollisionEnter(Collision other)
    {
//        if (!NetworkManager.Singleton.IsServer) return;
        ActorController pc = other.gameObject.GetComponent<ActorController>();
        if (pc == null) return;
        Apply(pc);
    }

    private void OnTriggerEnter(Collider other)
    {
        ActorController pc = other.gameObject.GetComponent<ActorController>();
        if (pc == null) return;
        Apply(pc);
    }

    public void Apply(ActorController pc)
    {
        switch (Type)
        {
            case TouchThings.Lava:
            {
                pc.TakeDamage((int)Amount);
                Vector3 dir = KBDir.magnitude == 0 ? (pc.transform.position - transform.position) : KBDir;
                Vector3 kb = dir.normalized * KB;
                if (kb.y <= 5) kb.y = 5;
                pc.TakeKnockback(kb);
                break;
            }
            case TouchThings.Dizzy:
            {
                if (!(pc is FirstPersonController)) break;
                ((FirstPersonController)pc).Dizzy = Amount;
                break;
            }
        }
    }
}

public enum TouchThings
{
    None,
    Lava,
    Dizzy,
    
}