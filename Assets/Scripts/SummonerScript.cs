using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerScript : TriggerableController
{
    public GameObject Target;
    
    public override void Trigger(TriggerMessages type = TriggerMessages.None, GameObject target = null)
    {
        base.Trigger(type, target);
        switch (type)
        {
            case TriggerMessages.Spawn:
            {
                GameObject who = Target != null ? Target : God.Player.gameObject;
                who.transform.position = transform.position;
                who.transform.rotation = transform.rotation;
                break;
            }
        }
    }
}
