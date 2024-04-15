using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZoneScript : TriggerScript
{
    public List<TriggerableController> Targets;
    public string EnterMessage;
    public string ExitMessage;
    public int Points = 0;
    [TextArea]
    public List<string> TextMessage;

    public override void Trigger(GameObject go)
    {
        base.Trigger(go);
        if (EnterMessage != "")
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
        if (ExitMessage == "") return;
        foreach (TriggerableController t in Targets)
        {
            t.Trigger(ExitMessage,go);
        }

        
    }
}
