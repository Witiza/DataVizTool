﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public  class HeatSelection
{
    bool selecting = false;
    Vector3 initial_pos = Vector3.one;
    Vector3 final_pos = Vector3.zero;
    Mesh mesh;

    // Start is called before the first frame update
    public void selectionByHandles()
    {

            initial_pos = Handles.PositionHandle(initial_pos, Quaternion.Inverse(Quaternion.identity));
            final_pos = Handles.PositionHandle(final_pos, Quaternion.identity);
        Vector3 tmp = initial_pos;
        if(initial_pos.x > final_pos.x)
        {
            initial_pos.x = final_pos.x;
            final_pos.x = tmp.x;
        }
        if (initial_pos.y > final_pos.y)
        {
            initial_pos.y = final_pos.y;
            final_pos.y = tmp.y;
        }
        if (initial_pos.z > final_pos.z)
        {
            initial_pos.z = final_pos.z;
            final_pos.z = tmp.z;
        }

        Object.FindObjectOfType<DataViewer>().GetComponent<DataViewer>().setBoundingBox(initial_pos,final_pos);
    }
    public void MouseCheck(SceneView sv)
    {
        GameObject.FindObjectOfType<DataViewer>().GetComponent<DataViewer>().setBoundingBox(initial_pos, final_pos-initial_pos);
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


        float magnitude = (final_pos - initial_pos).magnitude;
            if (magnitude > 1)
            {
            Vector3 center = (final_pos + initial_pos) / 2;
            Bounds square = new Bounds(center, (final_pos - initial_pos));
          //  BoundingSphere sphere = new BoundingSphere(initial_pos, (final_pos - initial_pos).magnitude / 2);
            for (int i = 0; i < heatmap.GetLength(0); i++)
            {
                for (int j = 0; j < heatmap.GetLength(1); j++)
                {
                    if (square.Contains(heatmap[i, j].position))
                    {
                        heatmap[i, j].selected = true;
                    }
                    else
                    {
                        heatmap[i, j].selected = false;
                    }
                }
            }
        }
        

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
