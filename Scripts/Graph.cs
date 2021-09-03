using System.Collections.Generic;
using UnityEngine;
/* 
The base class for all graphs. It contains methods which 
   are common amongst all graphs
*/
public abstract class Graph
{
    /* The labels for the graph, 
    such as the axis names and the graph's title */
    protected string [] names;
    // The CSV data for the graph
    protected Dictionary<string, double [] > data;
    // The list of selected points
    protected int selected = -1;
    // The graph gameobject
    protected GameObject parentObject;
    // Calculating the range for normalizing the data values
    protected double [] range;
    // The dimension length of the axes
    protected const int dimensionLength = 10;
    // An array to hold the scaling offsets
    protected double [] offsets = new double [8];
    // The position values for the data points.
    protected Vector3 [] pos;
    // A variable to store the rotation of the graph
    protected Quaternion orientation;
    // An array to store the color information.
    protected Color [] color;
    // A toogle for the color bar
    protected bool colorbar = false;
    // A variable to hold the camera's zoom
    protected float cameraZoom;
    // A variable to hold the camera's position
    protected Vector3 cameraPosition;
    // The constructor handles the labels and the data values
    protected Graph (Dictionary<string,double[]> data, string [] names) 
    {
        this.data = data;
        this.names = names;
        // Some hard-coded values to initalize the camera's orientation with
        this.cameraZoom = 51f;
        this.cameraPosition = new Vector3(4,3.39f,-15);
    }
    /*
        This method contains the plotting fucntion for the graph.
        It is implemented at the bottom of the hierarchy.
    */
    public abstract void plottingLogic();
    /*
        This method gets the resources from the pooler object, which
        in turn gets it from the resource manager. 
    */
    public abstract void getResources();
    /*
        This method fills the axes labels and values of the plot.
    */
    public abstract void generateAxes();
    // A helper function for finding the minimum and maximum of the ranges.
    public static void MinMax(double [] array, out double [] val ) {
        val = new double[2];
        double min,max; 
        max = min =  array[0];
        for (int i = 1; i < array.Length; i++) {
            if (array[i] > max)
                max = array[i];
            if (array[i] < min)
                min = array[i];
        }
        val[0] = min; 
        val[1] = max;
    }
    // A helper function is return the color of a data point from the color array
    public Color getColor (int id) {
        Color temp = color[(id-1)%color.Length];
        return new Color(temp[0],temp[1],temp[2]);
    }
    // A helper function for storing the index of the selected data point.
    public void setSelectedIndex (int index) {
        selected = index;
    }
    // A helper fucntion for retrieving the selected index.
    public int getSelectedIndex() {
        return selected;
    }
    // A helper function to set the orientation of the graph.
    public void setOrientation (Quaternion r) 
    { 
        parentObject.transform.rotation = r;
    }
    // A helper function to disable the graph
    public void disableGraph() {
        // Store the current orientation 
        orientation = parentObject.transform.rotation;
        // The camera's Position
        cameraPosition = Camera.main.transform.position;
        // The camera's zoom
        cameraZoom = Camera.main.fieldOfView;
        // Deactivate the graph
        parentObject.SetActive(false);
        // Disable the color bar if there is one.
        if (colorbar)
            ColorBar.disable();
    }
    /*
        This method normalizes the data point values to 
        fit inside the graph
    */
    public virtual void findIncrements() {}
    // A helper function for retrieving the numerical information of a data point
    public string dataText(int index) {
        string text = "";
        if (index != -1) {
            text = "ID : "+ index + "\n";
            foreach (string key in data.Keys) {
                text += key +" : "+data[key][index-1]+"\n";
            }
            text.TrimEnd('\n');
        }
        return text;
    }
    // A helper function for getting the title
    public string getTitle () {
        return names[0];
    }
    // A helper function for getting the number of data points
    public int getCount () {
        return pos.Length;
    }
    // A helper function for disabling the graph's script
    public void disableController(bool isDisable) {
        parentObject.transform.GetComponent<GraphController>().enabled = isDisable;
    }
    // A helper function to reset the camera to the current graph's state when switching
    public void setCamera () {
        CameraController.resetCameraOrientation(
            cameraZoom,
            cameraPosition
        );
    }
}
