using System.Collections.Generic;
using UnityEngine;
/*
 This class extends from the Graph object and forms 
 the basis for the 3D graphs
*/
public abstract class Graph3D : Graph
{
    public Graph3D (Dictionary<string,double []> value, string [] names, bool bar) : base(value,names)
    {
        colorbar = bar;
        // Hard-coded values or presets
        // Initialize the orientation.
        orientation = Quaternion.Euler(-23,50,-27);
        // Initialize the positions array
        pos = new Vector3 [data[names[1]].Length];
        // Add a color color bar if one is asked
        if (colorbar) {
            // Normalize the values.
            Graph.MinMax(value[names[4]],out range);
            offsets[6] = range[0]; offsets[7] = range[1];
        }
        findIncrements();
    }
    
    public override void generateAxes  () {
        /*
        Similar to the 2D version, except with an extra dimension
        */
        getResources();
        int [] maxLength = {0,0,0};
        double startx = offsets[0], starty = offsets[2], startz = offsets[4];
        string dimName;
        for (int i = 0; i < dimensionLength; i++) {
            dimName = FormattingMethods.numDisplayFormat(startx);
            maxLength[0] = Mathf.Max(maxLength[0],dimName.Length);
            Pooler.instance.Values3D.xLabels[i].text = dimName;
            dimName = FormattingMethods.numDisplayFormat(starty);
            maxLength[1] = Mathf.Max(maxLength[1],dimName.Length);
            Pooler.instance.Values3D.yLabels[i].text = dimName;
            dimName = FormattingMethods.numDisplayFormat(startz);
            maxLength[2] = Mathf.Max(maxLength[2],dimName.Length);
            Pooler.instance.Values3D.zLabels[i].text = dimName;
            startx += offsets [1];
            starty += offsets [3];
            startz += offsets [5];
        } 
        Pooler.instance.Values3D.resetXYZ(maxLength[0],maxLength[1],maxLength[2]);
        NavigationPanel.addText(pos.Length);
        Pooler.instance.Values3D.axesLabels[0].text = names[1];
        Pooler.instance.Values3D.axesLabels[1].text = names[2];
        Pooler.instance.Values3D.axesLabels[2].text = names[3];
        Pooler.instance.spawnData(Pooler.types.data,pos,parentObject);
         // Apply the orientations
        parentObject.transform.rotation = orientation;
        // Fill the color bar is one is present
         if (colorbar) {
            ColorBar.fillLabels(offsets[6],offsets[7]);
            ColorBar.setTitle(names[4]);
         }
    }
    // A method for getting the resources from the resource pool 
    public override void getResources() {
        parentObject = Pooler.instance.spawnGraph(Pooler.types.graph3D, 
        Quaternion.identity);
       
    }
    public override void findIncrements()
    {
        // Finding the ranges for each column 
        /* The offsets array containes the minimum element
           and the range for a given column. These values are calculated 
           for all dimesnions to noramlize entires onto the given scale.
                     val_i - val_min
           Formula = ------------------
                     val_max - val_min    
        */
        for (int i = 1; i < 4; i++) {
            Graph.MinMax(data[names[i]], out range);
            offsets[(i-1)*2] = range[0];
            offsets[(i-1)*2 + 1] = (range[1] - range[0]) / (dimensionLength - 1);
        }
    }
}
