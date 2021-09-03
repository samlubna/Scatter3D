using UnityEngine;
 /*
 The CameraController has all the methods 
 relevant to moving the camera and zooming.
 */
public class CameraController : MonoBehaviour
{
    // A variable for controlling the zooming speed.
    private float smooth = 150f;
    // The initial zoom value
    private static float zoom = 51f;
    // The limits of how far the camera can be dragged along the x and y axis, respectively
    private Vector2 limits = new Vector2(12,10);
    // A variable to store the initial postion of the camera when the user starts to move it.
    private Vector3 initial;
    // A variable to store the starting position of the camera when the application first loads.
    private Vector3 startPos;
    // A reference to the main camera.
    public static CameraController instance;
    
    // Start is called before the first frame update
    void Awake()
    {
        // Store the starting positon of the camera.
        startPos = Camera.main.transform.position;
        // Set the initial zoom of the camera.
        Camera.main.fieldOfView = zoom;
        // Get a reference to the main camera.
        instance = Camera.main.transform.GetComponent<CameraController>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Polling for user inputs // 
        // Get the scroll wheel's x and y deltas 
        Vector2 mv = Input.mouseScrollDelta;
        // Only the y delta is needed
        if (mv.y != 0) {
            /*
            if delta y < 0: zoom in
            else zoom out
            */
            float increment = (mv.y < 0f) ? -smooth : smooth;
            // Add the delta to the zoom value.
            zoom += increment*Time.deltaTime;
            // Clamp the values between the maximum and minimums of the zoom
            zoom =  Mathf.Clamp(zoom,5,51);
            // Assign the new zoom to the camera.
            Camera.main.fieldOfView = zoom;
        }
        // Check for the left mouse button
        // if pressed, reset the camera's postions and zoom
        if (Input.GetMouseButtonDown(1)) {
            Camera.main.transform.position = startPos;
            zoom = 51;
            Camera.main.fieldOfView  = zoom;
            GraphManager.resetOrientation();
        }
        // Check the scroll wheel 
        if (Input.GetMouseButtonDown(2)) {
            // The starting postion of the mouse when the user 
            // first presses the scroll wheel
            initial = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }
        // Check if the user has held down the scroll wheel 
        else if (Input.GetMouseButton(2)) {
            // Get the new postions of the mouse cursor
            Vector3 follow = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            // Find the difference between the cursor's initial and current positions.
            Vector3 temp = initial - follow;
            // Get the camera's current position
            Vector3 pos = Camera.main.transform.position;
            // Multiply the differnce with a factor as the values are between -1 and 1
            // Then add it to the camera's current postion.
            temp*=20;  pos += temp;
            // Ensure that the camera doesn't go out of bounds
            pos.x = Mathf.Clamp(pos.x, -limits.x, limits.x);
            pos.y = Mathf.Clamp(pos.y, -limits.y, limits.y);
            // Update the camera's position.
            Camera.main.transform.position = pos;
            // Update the initial point to the current one. 
            initial = follow;
        }
    }
    // A public, helper function for disabling the camera's script
    public void disable (bool isDisabled) {
        this.enabled = isDisabled;
    }
    // A helper function to reset the camera's orientation when switching graphs
    public static void resetCameraOrientation(float zoomValue, Vector3 positionValue) {
        zoom = zoomValue;
        Camera.main.transform.position = positionValue;
        Camera.main.fieldOfView = zoom;
    }
}
