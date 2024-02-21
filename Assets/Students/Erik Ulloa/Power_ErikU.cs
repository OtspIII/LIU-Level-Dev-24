using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power_ErikU : GenericPower
{
    public override void Activate()
    {
        Destroy(Player);
    }


}
