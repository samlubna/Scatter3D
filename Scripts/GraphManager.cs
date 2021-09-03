using System.Collections.Generic;
using UnityEngine;
/*
     Manages the graphs that the user has loaded.
*/
public class GraphManager : MonoBehaviour
{
    // A list of graphs
   private static List<Graph> graphs = new List<Graph>();
   // The active graph
   private static Graph activeGraph;
   // Some presets for the 2D and 3D graphs.
   // These are used when resetting graphs' positions.
   private static Quaternion [] orientationPresets = {
       Quaternion.identity,
       Quaternion.Euler(-23,50,-27)
   };
   // A method to return the color of the active graph
   public static Color activeColor (int id) {
       if (activeGraph!=null)
        return activeGraph.getColor(id);
        return Color.white;
   }
   // A method for adding the graph in the list of graphs
   public static void AddGraph (Graph g) {
       // Calling the graphs plotting logic
       g.plottingLogic();
       // If no graph is currenlty active, then make the newest entry active
       if (activeGraph == null){
        activeGraph = g;
        g.generateAxes();
        // Enable the navigation panel
        CanvasUtilities.activateNavigation(true);
       }
       // Add the graph to the list
       graphs.Add(g);
       // Add the graph's title to the panel
       NavigationPanel.addEntry(g.getTitle());
   }
    // A mthod for setting the selected data point 
   public static void selected (int ind) {
       // Get the current selection
       int current = activeGraph.getSelectedIndex();
       // Nothing selected.
            if(current == -1) 
                activeGraph.setSelectedIndex(ind);
        // Deselect
            else if (current == ind)
                activeGraph.setSelectedIndex(-1);
        // New selection
            else {
                Pooler.instance.getPointController(current-1).clearSelection();
                activeGraph.setSelectedIndex(ind);
            } 
            // Print the numerical information of the selected point on the information panel
            int newIndex = activeGraph.getSelectedIndex();
            CanvasUtilities.setPlaceholder(
                activeGraph.dataText(newIndex)
            );
   }
   // A helper method for retrieving the infromtion for a data point
   public static string getInfo (int ID) {
       return activeGraph.dataText(ID);
   }
    // A method for switching between graphs
   public static void swithGraphs(int index) {
       // Deallocate the resources inside the pool
        Pooler.instance.orphan();
        // Disable the current graph
        activeGraph.disableGraph();
        // Get the selected point.
        int activePoint = activeGraph.getSelectedIndex();
        // A point was selected in the previous graph
        if (activePoint != -1){
            // Clear the selected point
            Pooler.instance.getPointController(activePoint-1).
            clearSelection();
        }
        // Switch to the new graph
        activeGraph = graphs[index];
        activePoint = activeGraph.getSelectedIndex();
        // Reselect the selected point of the new graph if it has one
        if (activePoint != -1) {
        Pooler.instance.
        getPointController(activePoint-1).activate();
        }
        // Show the selected point's informaruib
        CanvasUtilities.setPlaceholder(
            activeGraph.dataText(activePoint)
        );
        CanvasUtilities.showInfo("");
        activeGraph.generateAxes();
        // The current graph's camera
        activeGraph.setCamera(); 
   }
    /* 
    A method for deleting the graph. There are two ways of removing graphs:
    delete the current one, or delete all.
    */
   public static void deleteGraph(int index, bool all = false) {
       // delete all
       if (all)  {
           Graph temp = activeGraph;
           foreach (Graph g in graphs) {
               activeGraph = g;
               selected(-1);
           }
           graphs.Clear();
           activeGraph = temp;
           }
           // Delete a specific one
       else  {
           graphs.RemoveAt(index);
           selected(-1);
           }
           // No graphs left
       if (graphs.Count == 0) {
           activeGraph.disableGraph();
           activeGraph = null;
           CanvasUtilities.setPlaceholder("");
           CanvasUtilities.showInfo("");
       }
   }
   // A method to return the number of data points
   public static int getDataCount () {
       return activeGraph.getCount();
   }
    // A helper function for retrieving the highlighted data point's index
    public static int getCurrentSelected () {
        return activeGraph.getSelectedIndex();
    }
    // A helper function for resetting the graphs orientation.
   public static void resetOrientation () {
       if (activeGraph != null) {
       if (activeGraph is Graph2D)
            activeGraph.setOrientation(
                orientationPresets[0]
            );
        else 
            activeGraph.setOrientation(
                orientationPresets[1]
            );
        }
   }
    // A helper method for disabling the graph's controller.
    public static void disableGraphController (bool isDisable) {
        if (activeGraph!=null)
            activeGraph.disableController(isDisable);
    }

}
