using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TriggerZoneScript : TriggerScript
{
    public List<TriggerableController> Targets;
    [FormerlySerializedAs("EnterMessageX")] public TriggerMessages EnterMessage;
    [FormerlySerializedAs("ExitMessageX")] public TriggerMessages ExitMessage;
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
                t.Trigger(EnterMessage, go);
            }
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
            t.Trigger(ExitMessage,go);
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
