using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power_Misha : GenericPower
{
    public override void Activate()
    {
        Player.SetGravity(Player.GetGravity() * -1);
    }
}
