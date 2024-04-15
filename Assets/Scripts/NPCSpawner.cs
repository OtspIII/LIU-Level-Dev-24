using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : TriggerableController
{
    [Header("Customizable")]
    public List<NPCController> Prefabs;
    public float RespawnTime = 15;
    public bool SpawnEndless = false;
    public bool StartOn = true;
    
    [Header("Ignore Below")]
    public GameObject Holder;
    public List<NPCController> Children;
    float Countdown = 0;
    bool Waves = false;
    bool Active;

    void Start()
    {
        Waves = God.LM.Ruleset.Waves > 0;
        God.LM.NPCSpawns.Add(this);
        Active = StartOn;
        if(Active)
            Spawn();
    }

    void Update()
    {
        if (Waves || !Active) return;
        if (!SpawnEndless && Children.Count > 0) return;
        Countdown -= Time.deltaTime;
        if (Countdown <= 0)
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        Countdown = RespawnTime;
        NPCController p = GetPrefab();
        if (p == null) return;
        NPCController n = Instantiate(GetPrefab(), Holder.transform.position, Quaternion.identity);
        n.Spawner = this;
        Children.Add(n);
    }
    
    public NPCController GetPrefab()
    {
        if (Prefabs.Count == 1) return Prefabs[0];
        if (Prefabs.Count == 0) return null;
        if (Waves)
        { 
            if(Prefabs.Count > God.LM.CurrentWave)
                return Prefabs[God.LM.CurrentWave];
        }
        return Prefabs[Random.Range(0,Prefabs.Count)];
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
