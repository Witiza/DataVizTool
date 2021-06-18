using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class SDVHeatmapRenderer : MonoBehaviour
{
    Vector3 initial_pos;
    Vector3 final_pos;

    [HideInInspector]
    public SDVHeatmap heatmap;

    public Color SelectionColor = Color.black;


    void Start()
    {
        heatmap = EditorWindow.GetWindow<SDVHeatmap>();
    }



    public void setBoundingBox(Vector3 pos,Vector3 size)
    {
        initial_pos = pos;
        final_pos = size;
    }
    private void OnDrawGizmos()
    {
        if (heatmap != null)
        {
            if (heatmap.selecting)
            {
                Vector3 center = (final_pos + initial_pos) / 2;
                if (center.magnitude > 0.1)
                {
                    SelectionColor.a = 0.3f;
                    Gizmos.color = SelectionColor;
                    Gizmos.DrawCube(center, (final_pos - initial_pos));
                }
            }
        }
        else
        {
            //We dont want to open a new one always, we just want to get the reference if the window is open
           if(EditorWindow.HasOpenInstances<SDVHeatmap>())
            {
                heatmap=EditorWindow.GetWindow<SDVHeatmap>();
            }
        }
    }
}

