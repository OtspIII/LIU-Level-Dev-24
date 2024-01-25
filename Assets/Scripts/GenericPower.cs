using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericPower : MonoBehaviour
{
    public PlayerController Player;

    private void Awake()
    {
        Player = GetComponent<PlayerController>();
    }

    //This gets called whenever the player hits 'X'
    public virtual void Activate()
    {
        
    }

    //This gets called when you die. If it returns true, the player doesn't run their normal death code
    //You can use this to make something alternate happen when the player dies
	public virtual bool DeathOverride(GameObject source){
		return false;
	}
}
