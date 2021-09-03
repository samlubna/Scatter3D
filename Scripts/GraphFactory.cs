using System.Collections.Generic;
using UnityEngine;
/*
The graph factory for creating different types of graphs
*/
public class GraphFactory
{
    public static Graph getIntsance (Dictionary<string, double[]> data, string [] names, Color cl, int type) {
        if (type==2)
            return new ScatterPlot2D(data,names,cl);
        else if (type == 3)
            return new ScatterPlot3D(data,names,cl);
        else 
            return new ScatterPlot3D(data,names,cl,true);
    }
}
