using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
/*
 The file loader form class. It consumes the file path
 provided by the user.
*/
public class FileLoader :  MonoBehaviour
{
    // A reference to the input field.
   private TMPro.TMP_InputField inputField; 
   // A variable for storing feedback messages.
   private TMPro.TMP_Text messages;
   // A button reference.
   private Button next;
   /// <summary>
   /// Awake is called when the script instance is being loaded.
   /// </summary>
   void Awake()
   {
       // Get the UI components 
       inputField = GetComponentInChildren<TMPro.TMP_InputField>();
       messages = transform.GetChild(0).GetComponentInChildren<TMPro.TMP_Text>();
       next = transform.GetChild(transform.childCount-1)
       .GetComponentInChildren<Button>();
   }
    // A helper function to read the file.
      public void readFile() 
   {
       // A message to show that the file is loading.
       messages.text = "Loading...";
       // Retrieve the supplied path
       string path = inputField.text;
        // A dictionary for storing the data.
       Dictionary<string,double []> results = new Dictionary<string, double[]> ();
       // A try-catch block to deal with exceptions.
       try {  
        // Deactivate the next button
        next.interactable = false;
        // Read all lines from the file
        string [] file = File.ReadAllLines(path);
        // Set the columns size
        int columns = file[0].Length;
        // Set the row size: the first row is treated as the columns names
        int rows = file.Length - 1;
        // intializing the results 
        foreach (string key in file[0].Split(','))
                results.Add(key,new double[rows]);
        // Filling in the dictionary as key-value pairs.
        for (int i = 1; i < rows + 1; i++) {
            int cl = 0; 
            string [] value = file[i].Split(',');
            foreach(string key in results.Keys) {
                // Check for missing values: columns of differnet sizes
                if (!double.TryParse (value[cl], out results[key][i-1]))
                    throw new System.ArgumentNullException(
                        "Column: "+key+", Row: "+(i+1),"Missing or non-numerical values");
                cl += 1;
            } }
        // A message to show that the file was loaded
        messages.text = "Loaded Successfully";
        // Preparing to tranfer the data onto the next form
        FormScript.data = results;
        FormScript.messageState = 1;
        // Make the next button active.
        next.interactable = true;
        // For catching the errors.
       }  catch (System.Exception e) {
           messages.text = e.Message;
       }
   }

    void OnEnable()
   {
       // Some initial messages when the form is enabled
        ControllerEvents.incrementCounter();
        if (FormScript.messageState == 2) 
            messages.text = "Load another?";
        else
        messages.text = "Note: Make sure " +
        "that the data file is arranged in a CSV format with named columns.";
    }
    // A helper function attached to the cancel button.
    public void cancel () {
        // Clear the input field.
        inputField.text = "";
        // Reset the message state
        FormScript.messageState = 0;
        // Deactive the form
        transform.gameObject.SetActive(false);
        // Clear the data
        FormScript.data = null;
        // Deactive the next button.
        next.interactable = false;
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        // Decrement the UI counter.
         ControllerEvents.decrementCounter();
    }
}
