using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGravity_Dillon : GenericPower
{
    private Rigidbody2D RB;

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Player.SetGravity(Player.GetGravity() * -1);
        }
    }
}
