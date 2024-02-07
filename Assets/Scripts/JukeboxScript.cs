using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JukeboxScript : MonoBehaviour
{
    public static JukeboxScript Prime;
    // public static int Level = -1;
    public AudioSource AS;

    public bool Doomed = false;
    public bool DontMurder = false;
    
    private void Start()
    {
        if (Prime != null)
        {
            if (Prime.AS.clip == AS.clip)
            {
                Prime.DontMurder = true;
                Destroy(gameObject);
                return;
            }
        }
        Prime = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += MurderMe;
    }

    void Update()
    {
        if (DontMurder)
        {
            Doomed = false;
            DontMurder = false;
        }
        if (Doomed)
        {
            if (Prime == this)
            {
                Prime = null;
                // Level = -1;
            }

            Destroy(gameObject);
        }
    }
    
    void MurderMe(Scene scene, LoadSceneMode mode)
    {
        // if (SceneManager.GetActiveScene().buildIndex == Level)
        //     return;
        Doomed = true;
    }

}
