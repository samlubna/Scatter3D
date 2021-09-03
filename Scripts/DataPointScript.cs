using UnityEngine;
using UnityEngine.EventSystems;
/*
This class is attached to every data point object on-screen.
*/
public class DataPointScript : MonoBehaviour
{
    // The ID of the data point (its row entry in the CSV file)
    private int ID = 0;
    // A variable to track whether the point is highlighted.
    private int toggle = 0;
    // A variable to synchronize hover events.
    private int inStep = 0;
    // To change the color of the point.
    private Renderer render;
    // A reference to the material of the data point model 
    private Material mat;
    // A refernce to the highlighted material.
    private Material highlight;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // Get the renderer component
        render = GetComponent<Renderer>();
        // Get the material
        mat = render.material;
        // Get the highlighted material
        highlight = new Material(Pooler.Highlight());
    }
    // A helper function to set the ID.
    public void setID(int ID) {
        this.ID = ID;
    }
    
   /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject ()){
            toggle^=1;
            /*
            If toggle = 1, then tell the graph manager about the 
            selected point
            */
            if (toggle == 0) 
                inStep = 1;
            GraphManager.selected(ID);
        }
    }
    /// <summary>
    /// Called when the mouse enters the GUIElement or Collider.
    /// </summary>
    void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject ()){
             if (toggle == 0) {
                CanvasUtilities.showInfo(GraphManager.getInfo(ID));
                render.material = highlight;
                inStep = 1;
             }
        }
    }
    /// <summary>
    /// Called when the mouse is not any longer over the GUIElement or Collider.
    /// </summary>
    void OnMouseExit()
    {
        if (toggle == 0 && inStep == 1) {
            render.material = mat;
            CanvasUtilities.showInfo("");
            inStep = 0;
        }
    }
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        // After switching between graphs, change the colors 
        if (ID != 0) {
            mat.color = GraphManager.activeColor(ID);
            highlight.color = mat.color;
        }
    }
    // A helper fucntion to clear the highlighted point
    public void clearSelection() {
        render.material = mat;
        toggle = 0;
    }
    // A helper function to highlight a point.
    public void activate() {
        render.material = highlight;
        toggle = 1;
    }

}
