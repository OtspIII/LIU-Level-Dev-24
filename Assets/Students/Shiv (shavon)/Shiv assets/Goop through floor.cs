using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goopthroughfloor : GenericPower
{
    public override void Activate()
    {
        //put your code for power here shiv

        Player.CanUseOneWayPlatforms = true;
    }
}
