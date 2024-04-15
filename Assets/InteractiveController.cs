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
        if (Alt && ExitMessage != "")
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
}
