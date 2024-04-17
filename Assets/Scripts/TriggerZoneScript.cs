using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TriggerZoneScript : TriggerScript
{
    public List<TriggerableController> Targets;
    [FormerlySerializedAs("EnterMessageX")] public TriggerMessages EnterMessage;
    [FormerlySerializedAs("ExitMessageX")] public TriggerMessages ExitMessage;
    public TriggerMessages DelayMessage;
    public float Delay = 0;
    public int Points = 0;
    [TextArea]
    public List<string> TextMessage;

    public override void Trigger(GameObject go)
    {
        base.Trigger(go);
        if (EnterMessage != TriggerMessages.None)
        {
            foreach (TriggerableController t in Targets)
            {
                if (t == null) continue;
                t.Trigger(EnterMessage, go);
            }
        }

        if (DelayMessage != TriggerMessages.None)
        {
            God.LM.StartCoroutine(DelayTrigger(go, Delay));
        }

        if (TextMessage.Count > 0)
        {
            God.LM.StartCoroutine(God.LM.Cutscene(TextMessage, Points));
        }
        else if (Points > 0)
        {
            God.LM.GetPoint(Points);
        }
    }

    public override void Untrigger(GameObject go)
    {
        base.Untrigger(go);
        if (ExitMessage == TriggerMessages.None) return;
        foreach (TriggerableController t in Targets)
        {
            if (t == null) continue;
            t.Trigger(ExitMessage,go);
        }

        
    }
    
    
    public virtual IEnumerator DelayTrigger(GameObject go,float time)
    {
        yield return new WaitForSeconds(time);
        foreach (TriggerableController t in Targets)
        {
            if (t == null) continue;
            t.Trigger(DelayMessage, go);
        }
    }

    public override void Trigger(TriggerMessages type = TriggerMessages.None, GameObject target = null)
    {
        base.Trigger(type, target);
        switch (type)
        {
            case TriggerMessages.Toggle:
            case TriggerMessages.Start:
            {
                Trigger(target);
                break; 
            }
            case TriggerMessages.Stop:
            {
                Untrigger(target);
                break; 
            }
        }
    }
}

public enum TriggerMessages
{
    None=0,
    Exist=1,
    Vanish=2,
    ToggleExist=3,
    Start=4,
    Stop=5,
    Up=6,
    Down=7,
    Toggle=8,
    Spawn=9,
    Die=10,
}
