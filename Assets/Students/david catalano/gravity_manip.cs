using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravity_manip : GenericPower
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<sup_david>() != null)
        {
            Debug.Log("HI");
            Player.SetGravity(Player.Gravity * -1);
        }

        //player gravity switched


    }
}

