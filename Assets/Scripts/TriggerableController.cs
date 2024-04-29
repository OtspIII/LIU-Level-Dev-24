using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerableController : MonoBehaviour
{
    public Rigidbody RB;
    public int HP = -1;
    
    public List<MessageThing> DeathMessages;
    
    public virtual void TakeDamage(int amt, TriggerableController source = null)
    {
        if (HP < 0) return;
        HP -= amt;
        if (HP <= 0)
        {
            Die(source);
        }
    }

    public virtual void TakeKnockback(Vector3 kb)
    {
        if (RB == null) return;
        RB.velocity = kb;
    }
    
    public virtual void Die(TriggerableController source=null)
    {
        foreach (MessageThing m in DeathMessages)
        {
            if (m.Target == null) continue;
            if (m.Target.transform.parent == transform) m.Target.transform.parent = null;
            if(m.Timing != MessageTiming.Delay)
                m.Target.Trigger(m.Message);
            else
            {
                God.LM.StartCoroutine(DelayTrigger(gameObject, m.Delay,m.Message,new List<TriggerableController>(){m.Target}));
            }
                
        }
        Destroy(gameObject);
        
        // if(God.LM.Respawn(this))
        //     Reset();
        // else
    }
    
    
    public virtual IEnumerator DelayTrigger(GameObject go,float time,TriggerMessages m,List<TriggerableController> targs)
    {
        yield return new WaitForSeconds(time);
        foreach (TriggerableController t in targs)
        {
            if (t == null) continue;
            t.Trigger(m, go);
        }
    }

    public virtual void Trigger(TriggerMessages type=TriggerMessages.None,GameObject target=null)
    {
        switch (type)
        {
            case TriggerMessages.Exist:
            {
                gameObject.SetActive(true);
                break;
            }
            case TriggerMessages.Vanish:
            {
                gameObject.SetActive(false);
                break;
            }
            case TriggerMessages.ToggleExist:
            {
                gameObject.SetActive(!gameObject.activeSelf);
                break;
            }
            case TriggerMessages.Die:
            {
                Die(target != null ? target.GetComponent<ActorController>() : null);
                break;
            }
        }
    }
}
