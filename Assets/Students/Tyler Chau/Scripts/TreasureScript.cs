using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TreasureScript : MonoBehaviour
{
    public int Value = 1;
    public bool Collected = false;
    public string ID;
    public static List<string> CollectedIDs = new List<string>();
    public static int CurrentScene;

    public void Start()
    {
        CameraController.Main.AddTreasure(this);
        ID = transform.position.ToString();
        if (CollectedIDs.Contains(ID))
        {
            BeCollected(PlayerController.PC);
        }

        int cs = SceneManager.GetActiveScene().buildIndex;
        if (CurrentScene != cs)
        {
            CollectedIDs.Clear();
            CurrentScene = cs;
        }

    }

    public void BeCollected(PlayerController pc)
    {
        Collected = true;
        pc.Score += Value;
        Destroy(gameObject);
        if(!CollectedIDs.Contains(ID))
            CollectedIDs.Add(ID);
    }
}
