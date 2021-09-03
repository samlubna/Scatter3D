using System.Collections.Generic;
using UnityEngine;
/*
    This class drives the UI form for selecting 
    the axes after the CSV file is loaded
*/
public class UIformScript : MonoBehaviour
{
    // An array for holding the axes objects
    private GameObject [] Axes = new GameObject[4];
    // A dropdown for presenting the graph options.
    private TMPro.TMP_Dropdown graphChoices;
    // Input field objects for acquiring the fieldnames
    private TMPro.TMP_InputField [] fieldNames = new TMPro.TMP_InputField[5];
    // Dropdown objects for retrieving the columns names from the CSV files
    private TMPro.TMP_Dropdown [] fieldDropdowns = new TMPro.TMP_Dropdown[4];
    // A message box for giving feedback
    private TMPro.TMP_Text messageBox;
    // A color picker object
    private GameObject colorPicker;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // Get references to all the different UI elements on the form
        graphChoices =  transform.GetChild(0).transform.GetChild(0).
        GetComponent<TMPro.TMP_Dropdown>();
        graphChoices.onValueChanged.AddListener (
            delegate {
                selectGraph(graphChoices);
            }
        );
        fieldNames[0] =  transform.GetChild(1).
        GetComponentInChildren<TMPro.TMP_InputField>();

        for (int i = 0; i < 4; i++) {
            Axes[i] = transform.GetChild(i+2).gameObject;
            fieldNames[i+1] = Axes[i].
            GetComponentInChildren<TMPro.TMP_InputField>();
            fieldDropdowns[i] = Axes[i].
            GetComponentInChildren<TMPro.TMP_Dropdown>();
        }
        GameObject temp = transform.Find("BottomHalf").gameObject;

        colorPicker = temp.transform.GetChild(0).gameObject;
        messageBox = temp.transform.
        GetChild(1).GetComponentInChildren<TMPro.TMP_Text>();
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
         ControllerEvents.incrementCounter();
         // Communicating with the loader script
        if (FormScript.messageState == 1) {
            FormScript.messageState = 0;
            // Fill the dropdown lists with the column names
            for (int i = 0; i < Axes.Length; i++){ 
                TMPro.TMP_Dropdown temp = Axes[i].GetComponentInChildren<TMPro.TMP_Dropdown>();
                temp.ClearOptions();
                foreach (string key in FormScript.data.Keys) 
                    temp.options.Add(new TMPro.TMP_Dropdown.OptionData (key));
                temp.RefreshShownValue();
            }
        }
        messageBox.text = "";
    }
    // A method for selecting the graph and reflecting the changes on the UI form
    public void selectGraph (TMPro.TMP_Dropdown dp) {
        // Get the option from the dropdown list
        string value = dp.options[dp.value].text;
        // Check which type it is.
        string type = value.Substring(value.Length-2,2);
        // Switching logic for showing the axes fields.
        switch (type) 
        {
            case "2D":
                Axes[2].SetActive(false);
                Axes[3].SetActive(false);
                colorPicker.SetActive(true);
                break;
            case "3D":
                Axes[2].SetActive(true);
                Axes[3].SetActive(false);
                colorPicker.SetActive(true);
                break;
            case "4D":
                Axes[2].SetActive(true);
                Axes[3].SetActive(true);
                colorPicker.SetActive(false);
                break;
        }
    }
    // An event-based method that send the graph to the graph manager for rendering
    public void Finish () {
        // A set to check for duplicates: keys can't be the same
        HashSet<string> duplicates = new HashSet<string>();
        // Get the chosen graph type
        string dim = graphChoices.options[graphChoices.value].text;
        // Separate the numerical part from the name
        int dimIndex = (dim[dim.Length-2] - '0') + 1;
        // An array for storing the dimension names
        string [] names = new string[dimIndex];
        // A dictionary for extracting a set of columns
        Dictionary<string, double []> dataValues = new Dictionary<string, double[]>();
        // A try-catch block is used as a safety net
        try{ 
            // Variables for the old and new keys
            // The old keys is the column name from the csv file
            // The new key is the name given by the user
            string newKey, oldKey;
        for (int i = 1; i < dimIndex; i++) {
            // Get the column name
            oldKey = fieldDropdowns[i-1].
            options[fieldDropdowns[i-1].value].text;
            // The name has to be given and two axes can't have the same name
            if (fieldNames[i].text=="" || fieldNames[i-1].text=="")
                throw new System.ArgumentException (
                    "Can\'t have empty name fields"
                );
            // If correct, assign the new key
            newKey = fieldNames[i].text;
            // Add this new key into the set of duplicates
            duplicates.Add(newKey);
            // if the set size of the duplicate isn't equal to the i (the iterator)
            if (duplicates.Count != i)
            // Throw an exception
                throw new System.ArgumentException(
                    "Can\'t have duplucate Axes lables"
                );
            // Add the column with the new key
            dataValues.Add(newKey,FormScript.data[oldKey]);
            // Add the newkey into the names array
            names[i] = newKey;
        } 
        // The 0th index is the title of the graph
        names[0] = fieldNames[0].text;
        // get the graph from the factory
        Graph g = GraphFactory.getIntsance(dataValues,names,
        ColorPicker.GetColor(),dimIndex-1);
        // A loading message
        messageBox.text = "Loading...";
        // Add the graph to the manager.
        GraphManager.AddGraph(g);
        // Change message states for the previous UI form (the loader)
        FormScript.messageState = 2;
        // Deactive the form
        transform.gameObject.SetActive(false);
        transform.parent.gameObject.transform.
        GetChild(0).gameObject.SetActive(true);
        // Show the error message on the message box
        } catch (System.Exception e) {
            messageBox.text = e.Message;
        }
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
         ControllerEvents.decrementCounter();
    }
}
