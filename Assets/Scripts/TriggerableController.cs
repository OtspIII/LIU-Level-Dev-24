using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerableController : MonoBehaviour
{
    public Rigidbody RB;
    public int HP = -1;
    
    public virtual void TakeDamage(int amt, TriggerableController source = null)
    {
        Debug.Log("TD: " + amt + " / " + HP + " / " + gameObject);
        if (HP < 0) return;
        Debug.Log("TD2");
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
        Debug.Log("BREAK: " + gameObject + " / " + source);
        Destroy(gameObject);
        
        // if(God.LM.Respawn(this))
        //     Reset();
        // else
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
