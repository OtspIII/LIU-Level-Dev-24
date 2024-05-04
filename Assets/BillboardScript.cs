using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BillboardScript : MonoBehaviour
{
    public bool Reverse;
    public float Speed = 1f;

    public AudioSource AS;
    public AudioClip Clip1;
    
    void Update()
    {
        if(Points > 3) 
            AS.clip = Clip1;
        float old = transform.rotation.eulerAngles.y;
        transform.LookAt(God.Player.transform);
        float after = transform.rotation.eulerAngles.y;
        if (Reverse == true)
            after += 180;
        if (Speed > 0)
        {
            after = Mathf.LerpAngle(old, after, Speed * Time.deltaTime);
        }
        transform.rotation = Quaternion.Euler(0,after,0);
    }
}
