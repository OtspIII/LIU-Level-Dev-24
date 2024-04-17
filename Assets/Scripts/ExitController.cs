using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitController : MonoBehaviour
{
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            int sceneN = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(sceneN);
            //StartCoroutine(LoadLevel(sceneN + 1));
        }
            
    }
    
    private void OnCollisionEnter(Collision other)
    {
        FirstPersonController p = other.gameObject.GetComponent<FirstPersonController>();
        if (p != null)
        {
            int sceneN = SceneManager.GetActiveScene().buildIndex;
            StartCoroutine(p.LoadLevel(sceneN + 1));
            
            God.LM.MakeAnnounce("YOU WIN");
        }
    }

    
}
