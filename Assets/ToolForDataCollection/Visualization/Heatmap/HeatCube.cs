using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HeatCube
{
    public List<Vector3Event> events = new List<Vector3Event>();
    Mesh mesh;
    public Material lineMaterial =null;
    Material mat;
    public int max_events;
    float alpha = 0;
    HeatMapViewer parent;
    

    Matrix4x4 transform;
    Vector3 position;
    Vector3 scale;
    Quaternion rotation;

    public HeatCube(Vector3 _position,Vector3 _scale, Material material, HeatMapViewer map)
    {
        position = _position;
        rotation = Quaternion.identity;
        scale = _scale;
        parent = map;
        transform = Matrix4x4.TRS(position, Quaternion.identity, scale);
        mesh = Shapes.GetUnityPrimitiveMesh(PrimitiveType.Cube);
        mat = new Material(material);
       
        //CreateLineMaterial();
    }

    public int getEventAmount()
    {
        int ret = 0;
        foreach(Vector3Event tmp in events)
        {
            if(parent.checkIfUsingEvent(tmp.name))
            {
                ret++;
            }
        }
        return ret;
    }

    public void generateColor()
    {
        //Integer divison LMAOOOOO
        
       alpha = (getEventAmount()) / (float)(max_events);
        Color color = parent.gradient.Evaluate(alpha);
        mat.color = color;
    }
  
    public void RenderHeat()
    {
        if (alpha > 0.0f)
        {
            Graphics.DrawMesh(mesh, transform, mat, 0); //LAYER AS AN OPTION
        }

    }

    public void generateHeight()
    {
        float median_height = 0;
        foreach(Vector3Event ev in events)
        {
            median_height += ev.data.y;
        }
        median_height /= events.Count;
        position.y = median_height;
        transform.SetTRS(position, rotation, scale);
    }


}
