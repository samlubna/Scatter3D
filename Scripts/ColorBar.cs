using UnityEngine;
using UnityEngine.UI;
/*
The color bar is pertinent to the heat map.
*/
public class ColorBar : MonoBehaviour
{
    // A rawimage variable to store the 2D color bar.
    private RawImage image;
    // An array of text elements to populate the lables.
    private static TMPro.TMP_Text [] ColorLabels;
    // A reference to the the color bar object.
    private static ColorBar instance;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // Assigning the instance to the current script
        instance  = this;
        // Retrieving the image component of the color bar. 
        image = transform.GetComponent<RawImage>();
        // Generating a 2D texture.
        Texture2D tex = GetTexture2D (740,1);
        // Assigning the texture to the image.
        image.texture = tex;
        // Getting the label components from the color bar.
        ColorLabels = transform.
        GetComponentsInChildren<TMPro.TMP_Text>();
        // Deactivating the color bar at the start.
        transform.parent.gameObject.SetActive(false);
        transform.parent.transform.parent.gameObject.SetActive(false);

    }
    // A helper function for getting a mixture of two colors (gradient).
    private static Color RenderColor (float time) {
        // The two colors are green and red.
        return Color.Lerp(Color.green,Color.red,time);
    }
    // A helper function for returning the 2D texture
    // with a filled gradient. 
    public Texture2D GetTexture2D (int width, int height) {
        Texture2D texture2D = new Texture2D(width, height);
        Color [] colors = new Color[Mathf.Max(width,height)];
        for (int i = 0 ; i <  width; i++) {
            colors[i] = RenderColor( (float)i / (width-1));
        }
        texture2D.SetPixels(colors);
        texture2D.Apply();
        return texture2D;
    }
    // A helper function for returning a color array
    // for a list of values.
    public static Color [] colorArray(double [] values) {
        Color [] colors = new Color [values.Length];
        double [] minMax;
        Graph.MinMax(values,out minMax);
        for (int i = 0 ; i < values.Length; i++) {
            colors[i] = RenderColor(
                (float)(values[i] - minMax[0])
                /
                (float)(minMax[1] - minMax[0]) 
                );
        }
        return colors;
    }
    // A helper function for populating the color bar's labels.
    public static void fillLabels (double min, double max) {
        double diff = (max - min) / 10;
        for (int i = 0; i < 9; i++) {
            ColorLabels[i].text = FormattingMethods.
            numDisplayFormat(max);
            max -= diff;
        }
        instance.transform.parent.gameObject.SetActive(true);
    }
    // A helper function for disabling the color bar.
    public static void disable () {
         instance.transform.parent.gameObject.SetActive(false);
    }
    // A helper function for setting the title of the color bar.
    public static void setTitle (string name) {
        ColorLabels[9].text = name;
    }


}
