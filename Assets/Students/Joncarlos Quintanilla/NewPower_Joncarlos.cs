using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewPower_Joncarlos : GenericPower
{

    public float teleportDistance = 2.2f;
    public float teleportCooldown = 0.5f;
    private float lastTeleportTime;


    public override void Activate()
    {
        Debug.Log("Hello");
        Teleport();
    }

    void Update()
    {
        if (Time.time - lastTeleportTime >= teleportCooldown)
        {
            Teleport();           
        }
        if (transform.position.y <= -5)
        {
            ReloadCurrentScene();
        }
        if (transform.position.y >= 6)
        {
            ReloadCurrentScene();
        }

        //Teleport();
    }

    void Teleport()
    {
        Vector2 currentPlayerPosition = transform.position;
        if (Input.GetKey(KeyCode.X))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Vector2 teleportDestination = new Vector2(currentPlayerPosition.x, currentPlayerPosition.y + teleportDistance);
                transform.position = teleportDestination;
                lastTeleportTime = Time.time;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Vector2 teleportDestination = new Vector2(currentPlayerPosition.x, currentPlayerPosition.y - teleportDistance);
                transform.position = teleportDestination;
                lastTeleportTime = Time.time;
            }
            else { return; }
        }
        else { return; }
    }
     void ReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(currentSceneName);
    }
}
