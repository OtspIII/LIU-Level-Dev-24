using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string Author;

    void Awake()
    {
        God.GM = this;
    }
    
    void Update()
    { 
       
    }
    
    
}
