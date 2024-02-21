using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudScript : MonoBehaviour
{
  public GameObject Button;
  public float Timer = 6f;
  public GameObject Off;
  public GameObject On;
  public GameObject Range;
  public bool InRange = false;
  public bool activated = false;
  

  public void Start()
  {
   
  }

  public void Update()
  {
    if (Timer <= 0f)
    {
      activated = false;
      Timer = 6f;
      On.SetActive(false);
      Off.SetActive(true);
      Range.SetActive(true);
    }
    
    
    
    
    if (activated)
    {
      gameObject.GetComponent<MovingPlatformController>().enabled = false;
      Timer -= Time.deltaTime;
    }
    else if (activated == false)
    {
      gameObject.GetComponent<MovingPlatformController>().enabled = true;
    }
    if (InRange)
    {
      Button.SetActive(true);
      if (activated)
      {
        Button.SetActive(false);
      }
      if (Input.GetKeyDown(KeyCode.E))
      {
        On.SetActive(true);
        Off.SetActive(false);
        Range.SetActive(false);
        activated = true;
      }
    }else if (InRange == false)
    {
      Button.SetActive(false);
    }

  }

  public void OnTriggerEnter2D(Collider2D col)
  { 
    Debug.Log("InRange");
    if (col.CompareTag("Player"))
    {
      InRange = true;
    }
    

  }
  public void OnTriggerExit2D(Collider2D col)
  { 
    Debug.Log("Out");
    if (col.CompareTag("Player"))
    {
      InRange = false;
    }

  }
  
  }

