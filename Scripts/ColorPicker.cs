using UnityEngine.UI;
using UnityEngine;
/*
The color picker class is from where the user 
sets the color scheme for the data points.
*/
public class ColorPicker : MonoBehaviour
{
    // A reference to the Color Picker object
    private static ColorPicker instance;
    // A variable to store the color object.
    public Color currentColor;
    // A variable for the image panel that gives the preview of the color.
    private Image image;
    // Sliders for controlling the colors
    private Slider [] sliders;
    // Input fields for setting the colors via RGB values.
    private TMPro.TMP_InputField [] inputFields;
    // Variable to holf the RGB values.
    private float red = 0f, green =  0f, blue = 0f ;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // Set the instance variable to the current object.
        instance = this;
        // Create the color.
        currentColor = new Color(red,green,blue);
        // Get the image, sliders, and inputfields from the color picker UI.
        image = transform.GetChild(0).transform.GetComponent<Image>();
        sliders = GetComponentsInChildren<Slider>();
        inputFields = GetComponentsInChildren<TMPro.TMP_InputField>();
        // Set the preview color to the current color.
        image.color = currentColor;
    }
    // A helper function to set the red value
    public void setRed(float value) {
        red = value;
        inputFields[0].text = ((int)(red*255)).ToString();
        setColor();
    }
    // A helper function to set the blue value
    public void setBlue(float value) {
        blue = value;
        inputFields[2].text = ((int)(blue*255)).ToString();
        setColor();
    }
    // A helper function to set the green value
    public void setGreen(float value) {
        green = value;
        inputFields[1].text = ((int)(green*255)).ToString();
        setColor();
    }
    // A helper function to set the color.
    private void setColor () 
    {
        currentColor = new Color(red,green,blue);   
        image.color = currentColor;
    }
    // A helper function to get R value from the input field.
    public void getInputRed(string value) {
        int term;
        if (int.TryParse(value,out term)) {
            red = (float)Mathf.Abs(term)%256/255;
            sliders[0].value = red;
            setColor();
            }
        else {
            sliders[0].value = 0;
        }
    }
    // A helper function to get G value from the input field.
    public void getInputGreen(string value) {
        int term;
        if (int.TryParse(value,out term)) {
            green = (float)Mathf.Abs(term)%256/255;
            sliders[1].value = green;
            setColor();
            }
        else {
            sliders[1].value = 0;
        }
    }
    // A helper function to get B value from the input field.
    public void getInputBlue(string value) {
        int term;
        if (int.TryParse(value,out term)) {
            blue = (float)Mathf.Abs(term)%256/255;
            sliders[2].value = blue;
            setColor();
            }
        else {
            sliders[2].value = 0;
        }
    }
    // A helper function to get the finialized color.
    public static Color GetColor() {
        return instance.currentColor;
    }
}
