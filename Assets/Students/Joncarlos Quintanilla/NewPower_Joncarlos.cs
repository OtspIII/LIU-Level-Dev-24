using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewPower_Joncarlos : GenericPower
{

    public Color newColor = Color.blue;
    public Color normalColor = Color.white;
    public Color eyeColor = Color.red;
    public Color realeyeColor = Color.black;
    public SpriteRenderer Body;
    public SpriteRenderer Eye;

    public float teleportDistance = 2.2f;
    public float teleportCooldown = 0.5f;
    private float lastTeleportTime;
    public float timer = 0.5f;
    public bool TP = true;


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
            TP = true;
        }
        if (transform.position.y <= -5)
        {
            ReloadCurrentScene();
        }
        if (transform.position.y >= 6)
        {
            ReloadCurrentScene();
        }


        timer -= Time.deltaTime;
        if (timer <=0)
        {

        }

        //Teleport();
    }

    void Teleport()
    {
        if (TP == true)
        {
            Vector2 currentPlayerPosition = transform.position;
            Eye.color = realeyeColor;
            if (Input.GetKey(KeyCode.X))
            {

                Body.color = newColor;

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    Vector2 teleportDestination = new Vector2(currentPlayerPosition.x, currentPlayerPosition.y + teleportDistance);
                    transform.position = teleportDestination;

                    Body.color = normalColor;
                    Eye.color = eyeColor;
                    lastTeleportTime = Time.time;
                    TP = false;
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    Vector2 teleportDestination = new Vector2(currentPlayerPosition.x, currentPlayerPosition.y - teleportDistance);
                    transform.position = teleportDestination;

                    Body.color = normalColor;
                    Eye.color = eyeColor;
                    lastTeleportTime = Time.time;
                    TP = false;
                }

            }
            else
            {
                Body.color = normalColor;
                return;
            }
        }
    }
     void ReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(currentSceneName);
    }
}
