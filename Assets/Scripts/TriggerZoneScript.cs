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

    public List<MessageThing> ExtraMessages;

    public override void Trigger(GameObject go)
    {
        base.Trigger(go);
        SendMessage(MessageTiming.Enter);

        SendMessage(MessageTiming.Delay);

        if (TextMessage.Count > 0)
        {
            God.LM.StartCoroutine(God.LM.Cutscene(TextMessage, Points,this));
        }
        else if (Points > 0)
        {
            God.LM.GetPoint(Points);
        }
    }

    public override void Untrigger(GameObject go)
    {
        base.Untrigger(go);
        SendMessage(MessageTiming.Exit);
    }

    public void SendMessage(MessageTiming time,GameObject go=null)
    {
        switch (time)
        {
            case MessageTiming.Enter:
            {
                if (EnterMessage != TriggerMessages.None)
                {
                    foreach (TriggerableController t in Targets)
                    {
                        if (t == null) continue;
                        t.Trigger(EnterMessage, go);
                    }
                }
                break;
            }
            case MessageTiming.Delay:
            {
                if (DelayMessage != TriggerMessages.None && Delay > 0)
                {
                    God.LM.StartCoroutine(DelayTrigger(go, Delay,DelayMessage,Targets));
                }
                break;
            }
            case MessageTiming.Exit:
            {
                if (ExitMessage != TriggerMessages.None)
                {
                    foreach (TriggerableController t in Targets)
                    {
                        if (t == null) continue;
                        t.Trigger(ExitMessage, go);
                    }
                }
                break;
            }
            case MessageTiming.AfterCutscene:
            {
                if (DelayMessage != TriggerMessages.None && Delay == 0)
                {
                    foreach (TriggerableController t in Targets)
                    {
                        if (t == null) continue;
                        t.Trigger(DelayMessage, null);
                    }
                }

                break;
            }
        }

        foreach (MessageThing m in ExtraMessages)
        {
            if (m.Timing != time || m.Target == null) continue;
            if(m.Timing != MessageTiming.Delay)
                m.Target.Trigger(m.Message);
            else
            {
                God.LM.StartCoroutine(DelayTrigger(go, m.Delay,m.Message,new List<TriggerableController>(){m.Target}));
            }
                
        }
    }
    
    public virtual void CutsceneEnd()
    {
        SendMessage(MessageTiming.AfterCutscene);

        
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

public enum MessageTiming
{
    None=0,
    Enter=1,
    Exit=2,
    Delay=3,
    AfterCutscene=4,
}

[System.Serializable]
public class MessageThing
{
    public TriggerMessages Message;
    public TriggerableController Target;
    public MessageTiming Timing;
    public float Delay = 0;
}