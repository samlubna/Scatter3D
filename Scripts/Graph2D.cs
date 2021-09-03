using System.Collections.Generic;
using UnityEngine;
/*
 This class extends from the Graph object and forms 
 the basis for the 2D graphs
*/
public abstract class Graph2D : Graph
{
    public Graph2D (Dictionary<string,double[]> data, string [] names) : base(data,names) {
        // Hard-coded values or presets
        // Initialize the orientation.
        orientation = Quaternion.identity;
        // Initialize the positions array
        pos = new Vector3 [data[names[1]].Length];
        // Normalize the values
        findIncrements();
    }

    public override void generateAxes()
    {
        // Get the reources from the pool.
        getResources();
        // A varaible for storing the max and min values of the data points.
        int [] maxLength = {0,0};
        // The starting values for the x and y axis (2D)
        double startx = offsets[0], starty = offsets[2];
        // A variable for storing the dimension label
        string dimName;
        // Iterating over each axis and populating the labels.
        for (int i = 0; i < dimensionLength; i++) {
            // Formatting the starting values for the x axis
            dimName = FormattingMethods.numDisplayFormat(startx);
            // Finding the largest label for the x axis.
            maxLength[0] = Mathf.Max(maxLength[0],dimName.Length);
            // Filling the x labels.
            Pooler.instance.Values.xLabels[i].text = dimName;
            // The process is repeated for the y axis.
            dimName = FormattingMethods.numDisplayFormat(starty);
            maxLength[1] = Mathf.Max(maxLength[1],dimName.Length);
            Pooler.instance.Values.yLabels[i].text = dimName;
            // Increment each starting value to move to the next one.
            startx += offsets [1];
            starty += offsets [3];
        } 
        /*
        This method makes spaace for the axis' title based on the 
        longest value.
        */
        Pooler.instance.Values.resetXY(maxLength[0],maxLength[1]);
        // Add the number of data points onto the navigation panel.
        NavigationPanel.addText(pos.Length);
        // Filling the axis names.
        Pooler.instance.Values.axesLabels[0].text = names[1];
        Pooler.instance.Values.axesLabels[1].text = names[2];
        // Spawnint the data on-screen
        Pooler.instance.spawnData(Pooler.types.data,pos,parentObject);
        // Apply the orientations
        parentObject.transform.rotation = orientation;
    }
    // A method for getting the resources from the resource pool 
    public override void getResources()
    {
        parentObject = Pooler.instance.spawnGraph(Pooler.types.graph2D, 
        Quaternion.identity);
    }

    public override void findIncrements()
    {
        // Finding the ranges for each column 
        /* The offsets array containes the minimum element
           and the range for a given column. These values are calculated 
           for all dimesnions to noramlize entries onto the given scale.
                     val_i - val_min
           Formula = ------------------
                     val_max - val_min    
        */
        for (int i = 1; i < 3; i++) {
            Graph.MinMax(data[names[i]], out range);
            offsets[(i-1)*2] = range[0];
            offsets[(i-1)*2 + 1] = (range[1] - range[0]) / (dimensionLength - 1);
        }
    }

}
