using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Danny_RainDrop : MonoBehaviour
{
   

   
    void Update()
    {
        Invoke("DESTORY",2f);
    }

    public int Damage = 1;
    public Vector2 Knockback = new Vector2(0,0);

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Danny_Rain"))
        {
            Destroy(gameObject);
        }
        PlayerController p = other.gameObject.GetComponent<PlayerController>();
        if (p != null)
        {
            p.TakeDamage(gameObject,Damage);
            if(Knockback != Vector2.zero)
                p.Knockback(Knockback);
        }
    }

    public void DESTORY()
    {
        Destroy(gameObject);
    }
}
