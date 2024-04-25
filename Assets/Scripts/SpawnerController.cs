using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnerController : CharController
{

    public float Timer = 0;
    public float MaxTimer = 3;
    public ThingController Prefab;

    public void Update()
    {
        if (!PlayerController.Moved) return;
        Timer += Time.deltaTime / MaxTimer;
        if (Timer >= 1 && JSON.Drop != ' ')
        {
            Timer = 0;
            ThingController drop = GameManager.Me.SpawnThing(JSON.Drop,GameManager.Me.Creator,transform.position);
            if (drop != null) drop.Source = Source;
            if(JSON.Audio != null) AS.PlayOneShot(JSON.Audio);
        }
    }
    
    public override void ApplyJSON(JSONData data)
    {
        base.ApplyJSON(data);
        if(data.Amount > 0)
            MaxTimer = data.Amount;
    }

    public override void Die()
    {
        Destroy(gameObject);
    }
}