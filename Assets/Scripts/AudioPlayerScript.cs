using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerScript : TriggerableController
{
    public AudioSource AS;
    public AudioClip Clip;
    
    public override void Trigger(TriggerMessages type = TriggerMessages.None, GameObject target = null)
    {
        base.Trigger(type, target);
        switch (type)
        {
            case TriggerMessages.Start:
            {
                AS.Play();
                break;
            }
            case TriggerMessages.Stop:
            {
                AS.Stop();
                break;
            }
            case TriggerMessages.Spawn:
            {
                AS.PlayOneShot(Clip);
                break;
            }
        }
    }
}
