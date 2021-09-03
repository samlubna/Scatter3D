using UnityEngine;
/*
 This class is reponsible for driving the Navigation 
 panel.
*/
public class NavigationPanel : MonoBehaviour
{
    // A dropdown list for all the active graphs
    private static TMPro.TMP_Dropdown graphList;
    // A text element for noting the tally of all the datapoints 
    private static TMPro.TMP_Text textPane;
    // The search field
    private TMPro.TMP_InputField userInput;
    // An instance of the script itself
    private static NavigationPanel instance;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // Assign the instance
        instance = this;
        // Retrieve the UI elements 
        graphList = transform.
        GetComponentInChildren<TMPro.TMP_Dropdown>();
        userInput = transform.Find("UserInput")
        .GetComponent<TMPro.TMP_InputField>();
        textPane = transform.Find("TextPane").
        GetComponent<TMPro.TMP_Text>();
        // The panel is initially inactive
        transform.gameObject.SetActive(false);
    }
    // The methods for adding the name of the graph to the dropdown list
    public static void addEntry(string name) {
        graphList.options.Add (
            new TMPro.TMP_Dropdown.OptionData (name)
        ); 
        // Refresh the values after adding.
        graphList.RefreshShownValue();
    }
    // An event-based method that tells the graph manager to switch when the user has selected a different graph
    public void onValueChanged(int value) {
        GraphManager.swithGraphs(value);
    }
    // An event-based method that creates the dropdown list.
    public void showList() {
        GameObject tp = transform.GetChild(0).
        gameObject.transform.Find("Dropdown List").gameObject;
        tp.GetComponent<RectTransform>().sizeDelta= new Vector2(0,150);
        tp.AddComponent<Temporaray>();
        ControllerEvents.incrementCounter();
    }
    // An event-based method that tells the graph manager to delete a graph.
    public void delete() {
        int size = graphList.options.Count;
        // Is there any graph left
        if (size != 0) {
            int value = graphList.value;
            // Delete the graph
            GraphManager.deleteGraph (value);
            graphList.options.RemoveAt(value);
            // Move onto the next graph if there is one.
            if (size - 1 > 0) {
                graphList.value = value%(size-1);
                GraphManager.swithGraphs(value%(size-1));
                graphList.RefreshShownValue();
        }   else {
            // Deactive the naviagtion panel
            if (transform.gameObject.activeSelf) 
                transform.gameObject.SetActive(false);
            // Deactive the navigation button
            else
                disableNav();
        }
        }
    }
    // An event-based method to delete all the graphs
    public void deleteAll () {
        if (graphList.options.Count!=0) {
            graphList.ClearOptions();
            GraphManager.deleteGraph(0,true);
           if (transform.gameObject.activeSelf) 
                transform.gameObject.SetActive(false);
            else
                disableNav();
        }
    }
    // A helper function to display the total number of points in a graph
    public static void addText(int t) {
        textPane.text = "Total Points : " + t;
    }
    // An event-based function that searches for a point by its ID.
    public void search () {
        int index; 
        // Get the total number of data points
        int count = GraphManager.getDataCount() + 1;
        // Is it a valid integer
        if (int.TryParse(userInput.text, out index)) {
            index = Mathf.Abs(index); 
            // Is it within range
            if (index < count && index != GraphManager.getCurrentSelected()) 
            {
                GraphManager.selected(index);
                CanvasUtilities.showInfo("");
                Pooler.instance.getPointController(index-1)
                .activate();
            }
        }
    }
    // An event-based method to clear the search bar and the selected datapoint's info
    public void clear() {
        userInput.text = "";
        if (GraphManager.getCurrentSelected() != -1) {
            GraphManager.selected(-1);
            CanvasUtilities.showInfo("");
        }
    }
    // A helper function to tell that the user is currently on the search bar
    public void onSelect() {
       ControllerEvents.incrementCounter();
    }
    // A helper function to tell that the user is no longer on the search bar
    public void onDeselect () {
        ControllerEvents.decrementCounter();
    }
    // An event-based function to exit the application.
    public void Quit() {
        Application.Quit();
    }
    // An event-based function to move to the previous graph from the current.
    public void Prev() {
        if (graphList.options.Count > 1) {
        int index = graphList.options.Count + graphList.value - 1;
        graphList.value = index%graphList.options.Count;
        GraphManager.swithGraphs(graphList.value);
        graphList.RefreshShownValue();
        }
    }
    // An event-based function to move to the next graph from the current.
    public void Next() {
        if (graphList.options.Count > 1) {
        int index = graphList.value + 1;
        graphList.value = index%graphList.options.Count;
        GraphManager.swithGraphs(graphList.value);
        graphList.RefreshShownValue();
        }
    }
    // A helper function to disable the navigation button
    public void disableNav () {
        CanvasUtilities.activateNavigation(false);
    }
    // A helper function to enable the navigation button
     public void enableNav () {
        CanvasUtilities.activateNavigation(true);
    }


}
