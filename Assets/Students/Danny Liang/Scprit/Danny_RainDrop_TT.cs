using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danny_RainDrop_TT : MonoBehaviour
{
   

    void Update()
    {
        Invoke("DESTORY",6f);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Danny_Rain"))
        {
            Destroy(gameObject);
        }
    }
    public void DESTORY()
    {
        Destroy(gameObject);
    }
}
