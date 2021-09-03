using UnityEngine;
/*
     This class is responsible for storing the
     original assets of the application and returning 
     copies when asked.
*/
public static class ResourceManager
{
    // A gameobject for the datapoint
    private static GameObject Point;
    // A gameobject for the 2D plot
    private static GameObject plot2DCP;
    // A gameobject for the 3D plot
    private static GameObject plot3DCP;
    // A material object for highlighting
    private static Material custom;

    // A method for returning the data point asset
    public static GameObject getPoint() {
        if (Point == null)
            Point = Resources.Load<GameObject>("Prefabs/DataPoint");
        return Point;
    }
    // A method for returning the 2D graph asset
    public static GameObject getGraph2DCP () {
        if (plot2DCP == null) {
            plot2DCP = Resources.Load<GameObject>("Prefabs/Graph2D/Graph2DCP");
        }
        return plot2DCP;   
    }
    // A method for returning the 3D graph Asset
    public static GameObject getGraph3DCP () {
        if (plot3DCP == null) {
            plot3DCP = Resources.Load<GameObject>("Prefabs/Graph3D/Graph3DCP");
        }
        return plot3DCP;   
    }
    // A mthod for returning the material asset
    public static Material getCustomMaterial () {
        if (custom == null)
            custom = new Material(Resources.Load<Shader>("Outline"));
        return custom;
    }
}
