using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danny_RainingCloudFake : MonoBehaviour
{
    public GameObject Rain;
    public GameObject RainDrop;
    public void Update()
    {
       RainDrop= Instantiate(Rain,transform.position + new Vector3(Random.Range(-2.5f, 1.1f), 0, 0),transform.rotation);
       RainDrop.transform.parent = transform;
    }
}
