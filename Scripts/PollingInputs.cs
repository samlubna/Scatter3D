using UnityEngine;
/*
    This class centralizes all the polling logic
    that is not connected to movement.
*/
public class PollingInputs : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Exit out of fullscreen by presseing esc
         if (Input.GetKeyDown(KeyCode.Escape)) {
          Screen.fullScreen = false;
        }
    }
}
