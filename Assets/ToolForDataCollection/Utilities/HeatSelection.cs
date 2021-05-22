using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public  class HeatSelection
{
    bool selecting = false;
    Vector3 initial_pos;
    Vector3 final_pos;
    Mesh mesh;
    int selected = 0;

    // Start is called before the first frame update
    public void MouseCheck(SceneView sv)
    {
        GameObject.FindObjectOfType<DataViewer>().GetComponent<DataViewer>().setBoundingBox(initial_pos, (final_pos-initial_pos)/2);
        //button values are 0 for left button, 1 for right button, 2 for the middle button
        if ( Event.current.type == EventType.MouseDrag && Event.current.button == 1)
        {
            if (!selecting)
            {
                RaycastHit hit;
                selecting = true;
                //Vector3 mousePos = Event.current.mousePosition;
                //mousePos.y = sv.camera.pixelHeight - mousePos.y;
                //Ray ray = sv.camera.ScreenPointToRay(mousePos);
                if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition),out hit))
                {
                    initial_pos = hit.point;
                    final_pos = hit.point;
                }
        
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(Event.current.mousePosition), out hit))
                {
                    final_pos = hit.point;
                }
            }
            
        }
        else if(Event.current.type == EventType.MouseUp && Event.current.button == 1)
        {
            if(selecting)
            {
                selecting = false;
            }
        }
    }

    public void SelectCubes(HeatCube[,] heatmap)
    {
        if(selecting)
        {

            float magnitude = ((final_pos - initial_pos) / 2).magnitude;
            if (magnitude > 1)
            {

                Bounds square = new Bounds(initial_pos, (final_pos - initial_pos));
                //BoundingSphere sphere = new BoundingSphere(initial_pos, (final_pos - initial_pos).magnitude / 2);
                for (int i = 0; i < heatmap.GetLength(0); i++)
                {
                    for (int j = 0; j < heatmap.GetLength(1); j++)
                    {
                        if (square.Contains(heatmap[i, j].position))
                        {
                            if(heatmap[i,j].selected == false)
                                selected++;
                            heatmap[i, j].selected = true;
                        }
                    }
                }
            }
        }
        Debug.Log("GameObjects Selected: " + selected);
    }
    public void drawSelection()
    {

        if (selecting)
        {

            float magnitude = (final_pos - initial_pos).magnitude;
            // Draw.Cube(initial_pos,new Vector3(magnitude,magnitude,magnitude) );
            
            Matrix4x4 transform = Matrix4x4.TRS(initial_pos, Quaternion.identity, new Vector3(magnitude, magnitude, magnitude)/2);
            
            if (mesh != null)
            {
                Material mat = new Material(Shader.Find("Diffuse"));
                Graphics.DrawMesh(mesh, Matrix4x4.identity,mat,0);
            }
            else
            {
                mesh = Shapes.GetUnityPrimitiveMesh(PrimitiveType.Cube);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
