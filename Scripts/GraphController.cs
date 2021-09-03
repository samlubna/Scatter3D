using UnityEngine;
/*
This class contains all the navigation controls for 
interacting with the graph.
*/
public class GraphController : MonoBehaviour
{
    // A variable for storing the rotation speed.
     public float RotationSpeed = 70f;
    // A variable for storing the graph's initial rotation
     private Vector3 point;
     /*
     A variable that acts as a reference point for rotating around the X and Z axis.
     */
     private Vector3 XZaxes;
     /// <summary>
     /// Start is called on the frame when a script is enabled just before
     /// any of the Update methods is called the first time.
     /// </summary>
     void Awake()
     {
         // Store the initial postion onAwake
         point = transform.position;
     }

     /// <summary>
     /// Update is called every frame, if the MonoBehaviour is enabled.
     /// </summary>
     void Update()
     {
         // Get the horizontal input
         float deltaX = Input.GetAxis("Horizontal")*RotationSpeed;
         // Get the vertical input
         float deltaY = Input.GetAxis("Vertical")*RotationSpeed;
         // Rotate around the y axis.
         transform.RotateAround(point, Vector3.down,deltaX*Time.deltaTime);
         // Was R pressed
         XZaxes = (Input.GetKey(KeyCode.R)) ? Vector3.forward : Vector3.right;
         // Either rotate around the X or Z axis.
         // Rotate arounf the Z axis if R is pressed
         transform.RotateAround(point, XZaxes,deltaY*Time.deltaTime);
     }

}
