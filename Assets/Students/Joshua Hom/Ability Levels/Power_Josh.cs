using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power_Josh : GenericPower
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public override void Activate()
    {
        Player.SetGravity(Player.GetGravity() * -1);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
