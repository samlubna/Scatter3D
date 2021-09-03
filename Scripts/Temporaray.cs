using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
A temporary class for modifying the OnDestroy method
of the dropdown list in the navigation panel
*/
public class Temporaray : MonoBehaviour
{
    /// <summary>
   /// This function is called when the MonoBehaviour will be destroyed.
   /// </summary>
   void OnDestroy()
   {
      ControllerEvents.decrementCounter();
   }

   

}
