using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerableController : MonoBehaviour
{
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
        }
    }
}
