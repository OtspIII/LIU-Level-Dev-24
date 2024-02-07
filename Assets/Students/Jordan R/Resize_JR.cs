using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Resize_JR : GenericPower
{
    public float scale1 = 3f; // Scale for key 1
    public float scale2 = 1f; // Scale for key 2
    public float scale3 = 0.5f; // Scale for key 3

    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ScalePlayer(player, scale1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ScalePlayer(player, scale2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ScalePlayer(player, scale3);
            }
        }
    }

    void ScalePlayer(GameObject player, float scale)
    {
        player.transform.localScale = new Vector3(scale, scale, scale);
    }
}