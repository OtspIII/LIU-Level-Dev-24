using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DangerController : MonoBehaviour
{
    public int Damage = 1;
    public Vector2 Knockback = new Vector2(0,0);
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController p = other.gameObject.GetComponent<PlayerController>();
        if (p != null)
        {
            p.TakeDamage(gameObject,Damage);
            if(Knockback != Vector2.zero)
                p.Knockback(Knockback);
        }
    }
}
