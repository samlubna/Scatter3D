using System.Collections.Generic;
using UnityEngine;
/*
The 3D scatter plot class. It extends the base 
3D graph.
*/
public class ScatterPlot3D : Graph3D
{
    // Deals with the color and whether a color bar was asked.
    // The base class handles the data and the names.
    public ScatterPlot3D(Dictionary<string,double []> value, string [] names, Color cl, bool bar = false) : base(value,names,bar) {
        // Heatmap with the fourth axis being the colorbar
        if (value.Count > 3){
            // Get the gradient for the color bar
            color = ColorBar.colorArray(data[names[4]]);
        }
        // If no heatmap, then proceed as normal 
        else{
             color = new Color[1];
             color[0] = cl;
        }
    }
    // The plotting logic for the 3D scatter plot
    public override void plottingLogic()
    {
        // Normalized values 
         for (int i = 0; i < pos.Length; i++) {
             pos[i] =  new Vector3 (
                Mathf.Abs((float)((data[names[1]][i] - offsets[0])/offsets[1])) + 1,
                Mathf.Abs((float)((data[names[2]][i] - offsets[2])/offsets[3])) + 1,
                Mathf.Abs((float)((data[names[3]][i] - offsets[4])/offsets[5])) + 1
                 );
         }
    }
}
