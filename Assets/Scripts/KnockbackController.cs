using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackController : MonoBehaviour
{
    public Vector2 Knockback = new Vector2(15,15);
	public bool NegativeYIsOkay = false;
    
	void Start(){
		if(Knockback.y < 0 && !NegativeYIsOkay)
			Knockback.y = 0;
	}

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        PlayerController p = other.gameObject.GetComponent<PlayerController>();
        if (p == null && other.transform.parent)
        {
            p = other.transform.parent.gameObject.GetComponent<PlayerController>();
        }

        Debug.Log("Knockback: " + p + " / " + other.gameObject.name);
        if (p != null)
        {
            p.Knockback(Knockback);
        }
    }
}
