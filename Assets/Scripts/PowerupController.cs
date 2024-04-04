using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : ThingController
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController pc = other.gameObject.GetComponent<PlayerController>();
        if (pc == null) return;
        if (JSON.Type == SpawnThings.Powerup)
        {
            if (JSON.Amount > 0)
                pc.Buff *= JSON.Amount;
            else
                pc.Buff *= 2;
            if (JSON.Lifetime > 0)
                pc.BuffTime = JSON.Lifetime;
        }
        else if (JSON.Type == SpawnThings.Weapon)
        {
            
        }
        if (JSON.Audio)GameManager.Me.PlaySound(JSON.Audio);
        Destroy(gameObject);
    }
}