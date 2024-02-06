using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureScript : MonoBehaviour
{
    public int Value = 1;
    public bool Collected = false;

    public void Start()
    {
        CameraController.Main.AddTreasure(this);
    }

    public void BeCollected(PlayerController pc)
    {
        Collected = true;
        pc.Score += Value;
        Destroy(gameObject);
    }
}
