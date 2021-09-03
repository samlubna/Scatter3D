using UnityEngine;
using UnityEngine.UI;
/*
 This class is attached to the Canvas object 
 that hosts all the UI elements on-screen. 
 The methods here access points for frequently used
 child UIs.
*/
public class CanvasUtilities : MonoBehaviour
{
    // A variable for storing text
    private static TMPro.TMP_Text info;
    // A button instance
    private static Button button;
    // A string variable for holding the placeholder text.
    private static string placeHolder;
    // The color panel gameobject.
    private static GameObject colorPanel;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // Get a reference to the message for displaying information.
        info  = transform.GetChild(1).GetComponentInChildren<TMPro.TMP_Text>();
        // Get the navigate button 
        button = transform.Find("Navigate").GetComponent<Button>();
        // Initialize the placeholder as a empty string
        placeHolder = "";
        // Get access to the color panel.
        colorPanel = transform.Find("ColorPanel").gameObject;
    }

    // A helper function for showing text on the information pane.
    public static void showInfo(string message) {
        if (message == "") info.text = placeHolder;
        else info.text = message;
    }
    // A helper function for setting the placeholder text.
    public static void setPlaceholder (string v) {
        placeHolder = v;
    }
    // A helper function for activating the navigation button.
    public static void activateNavigation (bool activate) {
        button.interactable = activate;
    }
    // A helper function to toggle the color panel.
    public void toggleColorPanel(bool toggle) {
        colorPanel.SetActive(toggle);
    }
}
