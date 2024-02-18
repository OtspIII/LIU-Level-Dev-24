using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

public class Danny_RainingCloud : MonoBehaviour
{
    public GameObject Rain;
    public void Update()
    {
        Instantiate(Rain,transform.position + new Vector3(Random.Range(-10.01f, 4.27f), 0, 0),transform.rotation);
    }
}
