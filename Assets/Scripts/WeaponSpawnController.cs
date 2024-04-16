using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WeaponSpawnController : TriggerableController
{
    [Header("Customizable")]
    public string WeaponToSpawn;
    public bool StartOn = true;
    [Header("Ignore Below")]
    public GameObject Holder;
    public WeaponController Held;
    public float RespawnTime = 15;
    float Countdown = 0;
    bool Active;
    

    void Start()
    {
        God.LM.WSpawns.Add(this);
        Active = StartOn;
        if(Active)
            Spawn();
    }

    void Update()
    {
        if (!Active) return;
        Holder.transform.Rotate(0,5,0);
        if (Held != null) return;
        Countdown -= Time.deltaTime;
        if (Countdown <= 0)
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        Countdown = RespawnTime;
        Held = Instantiate(GetPrefab(), Holder.transform.position, Quaternion.identity);
        Held.Setup(this,God.LM.GetWeapon(WeaponToSpawn));
    }
    
    public WeaponController GetPrefab()
    {
        return God.Library.WeaponSpawn;
    }

    public void TakenFrom(FirstPersonController pc)
    {
        Held = null;
        Countdown = RespawnTime;
    }
    
    
    public override void Trigger(TriggerMessages type=TriggerMessages.None, GameObject target = null)
    {
        base.Trigger(type, target);
        switch (type)
        {
            case TriggerMessages.Spawn:
            {
                Spawn();
                break;
            }
            case TriggerMessages.Start:
            {
                Active = true;
                break;
            }
            case TriggerMessages.Stop:
            {
                Active = false;
                break;
            }
        }
    }
}
