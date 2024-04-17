using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionDetector : MonoBehaviour
{
    public GameObject Target;
    public string EnterMessage = "Trigger";
    public string ExitMessage = "Untrigger";

    private void OnTriggerEnter(Collider other)
    {
        
        if(EnterMessage != "")
            Target.SendMessage(EnterMessage);
    }

    private void OnTriggerExit(Collider other)
    {
        if(ExitMessage != "")
            Target.SendMessage(ExitMessage);
    }
}
