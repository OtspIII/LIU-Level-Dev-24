using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerableController : MonoBehaviour
{
    public virtual void Trigger(string type="",GameObject target=null)
    {
        switch (type)
        {
            case "On":
            case "TurnOn":
            {
                gameObject.SetActive(true);
                break;
            }
            case "Off":
            case "TurnOff":
            {
                gameObject.SetActive(false);
                break;
            }
            case "ToggleOn":
            {
                gameObject.SetActive(!gameObject.activeSelf);
                break;
            }
        }
    }
}
