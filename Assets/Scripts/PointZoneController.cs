using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointZoneController : TriggerScript
{
    public int Points = 1;
    
    public override void Trigger(GameObject go)
    {
        base.Trigger(go);
        God.LM.GetPoint(Points);
    }
}
