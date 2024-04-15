using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public JSONWeapon Data;
    public ActorController Shooter;
    public ParticleSystem PS;
    //public NetworkObject NO;
    public SphereCollider Coll;
    public bool IsSetup = false;
    public string Name;
    
    public void Setup(ActorController pc,JSONWeapon data)
    {
        Data = data;
        Shooter = pc;
        //NO.Spawn();
        
        Name = Data.Text;
        SetColor();
    }
    public void Setup(float size, float dmg, float kb)
    {
        JSONTempWeapon js = new JSONTempWeapon();
        js.ExplodeRadius = size;
        js.ExplodeDamage = dmg;
        js.Knockback = kb;
        Data = new JSONWeapon(js);
        Shooter = null;
        Name = "BOOM";
        SetColor();
    }
    
    public void SetColor()
    {
        IsSetup = true;
        transform.localScale = Vector3.one * Data.ExplodeRadius;
        StartCoroutine(Explode());
    }
    
    void Update()
    {
        if (!IsSetup && Name != "")
        {
            
            Data = God.LM.GetWeapon(Name.ToString());
            SetColor();
        }
    }

    public IEnumerator Explode()
    {
        Coll.enabled = true;
        PS.Emit(Data.ExplodeDamage);
        yield return null;
        Coll.enabled = false;
        yield return new WaitForSeconds(2);
       Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
//        if (!NetworkManager.Singleton.IsServer) return;
        ActorController pc = other.GetComponent<ActorController>();
        if (pc)
        {
            if(Data.Knockback >0)
                pc.TakeKnockback((pc.transform.position - transform.position).normalized * Data.Knockback);
            if (pc == Shooter && !Data.SelfDamage) return;
            pc.TakeDamage(Data.ExplodeDamage,Shooter);
        }
    }
}
