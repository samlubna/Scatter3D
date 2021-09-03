using System.Collections.Generic;
using UnityEngine;
/*
The 2D scatter plot class. It extends the base 
2D graph.
*/
public class ScatterPlot2D : Graph2D
{
    // Handles the color while sends the data and names to the base class
    public ScatterPlot2D(Dictionary<string, double []> data, string [] names, Color cl) : base(data,names) {
        color = new Color[1];
        color[0] = cl;
    }
    // Overriding the plotting logic specific to this graph
    public override void plottingLogic()
    {
        // Filling the position vector array with normalized values
        for (int i = 0; i < pos.Length; i++) {
             pos[i] =  new Vector3 (
                Mathf.Abs((float)((data[names[1]][i] - offsets[0])/offsets[1])) + 1,
                Mathf.Abs((float)((data[names[2]][i] - offsets[2])/offsets[3])) + 1 - 1.2f,
                2);
         }
    }
}
