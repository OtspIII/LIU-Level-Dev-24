using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveController : TriggerZoneScript
{
    public Animator Anim;
    public string UseAnim = "";
    bool Alt = false;
    public string ResetAnim = "";
    
    public override void Trigger(GameObject go)
    {
        if (Alt && ExitMessage != TriggerMessages.None)
        {
            Untrigger(go);
        }
        else
        {
            base.Trigger(go);            
        }
        if (Anim != null)
        {
            if(ResetAnim != "" && Alt)
                Anim.Play(ResetAnim);
            else
                Anim.Play(UseAnim);
        }
        Alt = !Alt;
    }

    public override IEnumerator DelayTrigger(GameObject go, float time, TriggerMessages m,List<TriggerableController> targs)
    {
        yield return new WaitForSeconds(time);
        if (Alt && m == ExitMessage)
        {
            if(ResetAnim != "")
                Anim.Play(ResetAnim);
            Alt = false;
        }
        foreach (TriggerableController t in targs)
        {
            t.Trigger(m, go);
        }
    }
    
}
