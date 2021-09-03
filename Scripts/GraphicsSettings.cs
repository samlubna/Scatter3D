using UnityEngine;
/*
    The graphcs setting for the application
*/
public class GraphicsSettings : MonoBehaviour
{
    // An array to store all the resolution qualities.
    private TMPro.TMP_Dropdown [] ResQuality;
    // An array to store all the resolutions available in the monitor.
    private Resolution [] res;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // Retrieve the dropdown element for the resolution quality.
       ResQuality = transform.GetComponentsInChildren<TMPro.TMP_Dropdown>();
       // Get all the available resolutions.
       res = Screen.resolutions;
       // A variable to iterate over the choices with.
       int currentResIndex = 0;
       // Fill the dropdown menu with the options.
       for (int i = 0; i < res.Length; i++) {
           // If the current resolution is found, mark it on the dropdown list
           if (res[i].width == Screen.currentResolution.width &&  res[i].height == Screen.currentResolution.height) 
                currentResIndex = i;
            // Add the options to the list
            ResQuality[0].options.Add(
                new TMPro.TMP_Dropdown.OptionData (
                    res[i].width+" x "+res[i].height
                )
            );
       }
       // Fill in the quality settings
       foreach (string q in QualitySettings.names)
            ResQuality[1].options.Add(
                new TMPro.TMP_Dropdown.OptionData (q)
            );
        // Mark the current resolution and quality, respectively
        ResQuality[0].value = currentResIndex;
        ResQuality[1].value = QualitySettings.GetQualityLevel();
        ResQuality[0].RefreshShownValue();
        ResQuality[1].RefreshShownValue();
        transform.gameObject.SetActive(false);
    }
    // A helper function for choosing the quality level
    public void setQuality (int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    // A helper function for choosing the resolution
    public void setResolution (int level) {
        Screen.SetResolution(
            res[level].width,
            res[level].height,
            Screen.fullScreen
        );
    }
}
