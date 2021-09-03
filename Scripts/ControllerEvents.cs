/*
This class tallies all the active UIs on the screen.
Its purpose is to ensure that certain events don't 
occur while particular UI elements are active. 
*/
public class ControllerEvents
{
    // A variable to count all the active UI objects.
    private static int activeUIs = 0 ;
    // Increments the UI counter.
    public static void incrementCounter() {
        if (activeUIs == 0) {
            GraphManager.disableGraphController(false);
            CameraController.instance.disable(false);
        }
        activeUIs += 1;
    }
    // Decrements the UI counter
    public static void decrementCounter () {
        activeUIs -= 1;
        if (activeUIs  == 0 ) {
            GraphManager.disableGraphController(true);
            CameraController.instance.disable(true);
        }

    }
}
