using System.Collections.Generic;
using UnityEngine;
/*
    The pooler class is where all the assets are
    dumped to and retrieved from when rendering. 
*/
public class Pooler : MonoBehaviour
{
    // An instance of the pooler class
    public static Pooler instance;
    // A material variable for the highlighted state
    private static Material highlightMaterial;
    // An enum for separating the types of pools
    public enum types {
        data = 0,
        graph2D = 1,
        graph3D = 2
    }
    // Presets for the 2D graph as a struct
    public struct graph2dPresets {
        // The labels
        public TMPro.TMP_Text [] xLabels;
        public TMPro.TMP_Text [] yLabels;
        // The axes Labels.
        public TMPro.TMP_Text [] axesLabels;
        // The label's positions
        public RectTransform [] labeltrans;
        // The label's offsets 
        public Vector3 [] labelOffsets;
        // A helper function for setting the positions of the x and y values 
        public void resetXY (float y, float x) {
            Vector3 tempx,tempy;
            tempx = labelOffsets[0];
            tempy = labelOffsets[1];
            tempx.y += y*-.2f;
            tempy.x += x*-.2f;
            labeltrans[0].position = tempx;
            labeltrans[1].position = tempy;
        }
    }
    // Presets for the 3D graph as a struct
    public struct graph3dPresets {
        // The z,x,y, and axes labels
       public TMPro.TMP_Text [] xLabels;
       public TMPro.TMP_Text [] yLabels;
       public TMPro.TMP_Text [] zLabels;
       public TMPro.TMP_Text [] axesLabels;
       // The labels' position and offsets
       public RectTransform [] labeltrans;
       public Vector3 [] labelOffsets;
    // A helper function for setting the positions of the x,y and z values 
       public void resetXYZ (float z, float y, float x) {
           Vector3 tempx, tempy, tempz;
            tempx = labelOffsets[0];
            tempy = labelOffsets[1];
            tempz = labelOffsets[2];
            tempx.z += z*-.2f;
            tempy.x += y*-.16f;
            tempy.z += y*-.16f;
            tempz.x += x*.2f;
            labeltrans[0].position = tempx;
            labeltrans[1].position = tempy;
            labeltrans[2].position = tempz;
        }

    }
    // The list of pools
    private List<List<GameObject>> list = new List<List<GameObject>>();
    // A list for a specific pool
    private List<GameObject> pool;
    // The variable that sets the initial number of resources to create
    private int resourceLimit =  1;
    // The assets list to create copies from
    private GameObject  [] prefablist;
    // The preset values
    public graph2dPresets Values = new graph2dPresets();
    public graph3dPresets Values3D = new graph3dPresets();
    
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // Get the highlighted material from the resource Manager
        highlightMaterial = ResourceManager.getCustomMaterial();
        // Assign the instance to itself.
        instance = this;
        // Get the prefabs from the resource manager.
        GameObject  [] pre  = {
            ResourceManager.getPoint(),
            ResourceManager.getGraph2DCP(),
            ResourceManager.getGraph3DCP()
        };
        // Initialize the prefabs list
        prefablist = pre;
        // Create a pool list for each prefab and add it into the list of pools
        foreach (int t in System.Enum.GetValues(typeof(types))){
            pool = new List<GameObject>();
            GameObject temp;
            for (int i = 0; i < resourceLimit; i++) {
                temp = Instantiate(prefablist[t]);
                temp.SetActive(false);
                pool.Add(temp);
            }
            list.Add(pool);
        }
        // Attach the datapoint script to the data asset.
        list[(int)types.data][0].GetComponent<DataPointScript>().setID(1);
        // Call the setter methods fot the 2D and 3D presets.
        set2DPresets();
        set3DPresets();
    }
    // A helper function for creating the axes of the 2D or 3D graphs
    public GameObject spawnGraph(types t, Quaternion rotation) {
        GameObject temp;
        // Get the required type 2D or 3D;
        List<GameObject> q = list[(int)t];
        // Instantiate the object
        if (q.Count == 0)
            q.Add(Instantiate(prefablist[(int)t]));
        temp = q[q.Count-1];
        // Active the object.
        temp.SetActive(true);
        // Rotate it to preserve movement
        temp.transform.rotation = rotation;
        return temp;
    }
    // A helper function for creating all the data points on the graphs
    public void spawnData (types t, Vector3 [] points,GameObject parent) {
        // Get the required asset
        List<GameObject> q = list[(int)t];
        GameObject temp;
        // The difference between the pool size and the required number of data points.
        int diff = points.Length - q.Count, index = 0;
        // While pool size < number of data points, fill the deficit
        for (int i = 0; i < diff; i++) {
            // Instantiate the object
            temp = Instantiate(prefablist[(int)t]);
            temp.SetActive(false);
            // Add it to the pool
            q.Add(temp);
            // Set its ID
            temp.GetComponent<DataPointScript>().setID(q.Count);
        }
        GameObject p;
        // Active the object
       for (int i = 0; i < points.Length; i++) {
            p = q[i];
            p.SetActive(true);
            // Position it in 3D space
            p.transform.position = points[index];
            // Attacj it to the graph
            p.transform.SetParent(parent.transform);
            // Increment the index
            index += 1;
        }

    }
    // A helper function to clear the pool. It isn't used.
    public void emptyPool () {
        foreach (List<GameObject> q in list)
            q.Clear();
    }
    /*
        This method acts as an interface for the 2D graph 
        to fill in the axes values and labels    
    */
    private void set2DPresets() {
        // Get the 2D graph asset
        GameObject temp = list[(int)types.graph2D][0];
        // From that asset, get the x marker
        GameObject xMark = temp.transform.GetChild(1).gameObject;
        // Get the y marker
        GameObject yMark = temp.transform.GetChild(2).gameObject;
        // Get the xy labels
        GameObject xyLables = temp.transform.GetChild(3).gameObject;
        // Get a reference to the acquired assets
        Values.xLabels = xMark.GetComponentsInChildren<TMPro.TMP_Text>();
        Values.yLabels = yMark.GetComponentsInChildren<TMPro.TMP_Text>();
        Values.axesLabels = xyLables.GetComponentsInChildren<TMPro.TMP_Text>();
        Values.labeltrans = xyLables.GetComponentsInChildren<RectTransform>();
        // Instatiate the Label offsets and get their positions
        Values.labelOffsets = new Vector3[2];
        Values.labelOffsets[0] = Values.labeltrans[0].position;
        Values.labelOffsets[1] = Values.labeltrans[1].position;
        temp = null;
    }
    /*
        This method acts as an interface for the 3D graph 
        to fill in the axes values and labels    
    */
    private void set3DPresets () {
        GameObject temp = list[(int)types.graph3D][0];
        GameObject xMark = temp.transform.GetChild(1).gameObject;
        GameObject yMark = temp.transform.GetChild(2).gameObject;
        GameObject zMark = temp.transform.GetChild(3).gameObject;
        GameObject  xyzLables = temp.transform.GetChild(4).gameObject;
        Values3D.xLabels = xMark.GetComponentsInChildren<TMPro.TMP_Text>();
        Values3D.yLabels = yMark.GetComponentsInChildren<TMPro.TMP_Text>();
        Values3D.zLabels = zMark.GetComponentsInChildren<TMPro.TMP_Text>();
        Values3D.axesLabels = xyzLables.GetComponentsInChildren<TMPro.TMP_Text>();
        Values3D.labeltrans = xyzLables.GetComponentsInChildren<RectTransform>();
        Values3D.labelOffsets = new Vector3[3];
        Values3D.labelOffsets[0] = Values3D.labeltrans[0].position;
        Values3D.labelOffsets[1] = Values3D.labeltrans[1].position;
        Values3D.labelOffsets[2] = Values3D.labeltrans[2].position;
        temp = null;
    }
    // A helper method for detaching the data point assets
    // from the current graph (done when switching)
    public void orphan() {
        foreach (GameObject p in list[(int)types.data]) {
            p.transform.parent = null;
            p.SetActive(false);
        }
    }
    // A helper method for retrieving the datapointscript
    public DataPointScript getPointController(int index) 
    {
        return list[(int)types.data][index].
        GetComponent<DataPointScript>();
    }
    // A helper function for returning the giglighted material
    public static Material Highlight() {
        return highlightMaterial;
    }

}
