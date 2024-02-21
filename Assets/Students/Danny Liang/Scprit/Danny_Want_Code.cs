using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danny_Want_Code : MonoBehaviour
{
   public void Update()
   {
      Invoke("DEs",5f);
   }

   public void DEs()
   {
      Destroy(gameObject);
   }
}
