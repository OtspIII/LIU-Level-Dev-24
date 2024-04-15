using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Unity.Netcode;

public class ItemSpawnController : TriggerableController
{
    [Header("Customizable")]
    public string ItemToSpawn;
    public float RespawnTime = 15;
    public Vector3 Destination;
    public Transform DestObj;
    public bool StartOn = true;
    
    [Header("Ignore Below")]
    public GameObject Holder;
    public SpawnableController Held;
    float Countdown = 0;
    bool Active;
    
    void Start()
    {
        Active = StartOn;
        if (DestObj != null) Destination = DestObj.position - God.LM.transform.position;
        God.LM.ISpawns.Add(this);
        if(Active)
            Spawn();
    }

    void Update()
    {
        if (!Active) return;
        if(Holder != null)
            Holder?.transform.Rotate(0,5,0);
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
        Vector3 where = Holder != null ? Holder.transform.position : transform.position;
        Held = Instantiate(GetPrefab(), where, Quaternion.identity);
        Held.Setup(this,God.LM.GetItem(ItemToSpawn));
    }
    
    public SpawnableController GetPrefab()
    {
        return God.Library.TestSpawn;
    }

    public void TakenFrom(FirstPersonController pc)
    {
        Held = null;
        Countdown = RespawnTime;
    }

    public override void Trigger(string type = "", GameObject target = null)
    {
        base.Trigger(type, target);
        switch (type)
        {
            case "Spawn":
            {
                Spawn();
                break;
            }
            case "Start":
            {
                Active = true;
                break;
            }
            case "Stop":
            {
                Active = false;
                break;
            }
        }
    }
}
