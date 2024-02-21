using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Kevin : GenericPower
{
  public override void Activate()
  {
    //transform.position += new Vector3(5, 0, 0);
    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    pos.z = 0;
    transform.position = pos;

  }
  //private void Update()
    //{
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    StartCoroutine((Teleport));
      //  }
    //}

    //private void StartCoroutine(string methodName)
    //{
      //  throw new NotImplementedException();
    //}

    //IEnumerable Teleport()
    //{
        //SpriteRenderer.enabled = false;
        //yield return new WaitForSeconds(1);
        //transform.position = new Vector2()Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main
       //     .ScreenToWorldPoint(Input.mousePosition).y;
     //   SpriteRenderer.enabled = true;

   // }
}
