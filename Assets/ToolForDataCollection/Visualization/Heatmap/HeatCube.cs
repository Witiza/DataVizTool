using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HeatCube
{
    public List<BaseEvent> events = new List<BaseEvent>();

    public Material lineMaterial =null;
    Material mat;
    public int max_events;
    public float alpha = 0;
    HeatMapViewer parent;
    public bool selected = false;
    

    Matrix4x4 transform;
    public Vector3 position;
    Vector3 scale;
    Quaternion rotation;

    public HeatCube(Vector3 _position,Vector3 _scale, Material material, HeatMapViewer map)
    {
        position = _position;
        rotation = Quaternion.identity;
        scale = _scale;
        parent = map;
        transform = Matrix4x4.TRS(position, Quaternion.identity, scale);
        mat = new Material(material);
       
        //CreateLineMaterial();
    }

    public int getEventAmount()
    {
        int ret = 0;
        foreach(BaseEvent tmp in events)
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
        int count = 0;
        foreach(BaseEvent ev in events)
        {
            if(parent.checkIfUsingEvent(ev.name))
            {
                count++;
            }
        }
        alpha = count /(float)max_events;
        if(alpha != 0)
        {
            Debug.Log("Alpha: " + alpha);
        }
        Color color = parent.gradient.Evaluate(alpha);
        mat.color = color;
    }
  
    public void RenderHeat(Mesh mesh)
    {
        if (mat.color.a > 0.0f)
        {
            Graphics.DrawMesh(mesh, transform, mat, 0); //LAYER AS AN OPTION
        }
    }

    public void generateHeight()
    {
        float median_height = 0;
        foreach(BaseEvent ev in events)
        {
            median_height += ev.position.y;
        }
        median_height /= events.Count+1; //Fuck u NaN
        position.y = median_height;
        transform.SetTRS(position, rotation, scale);
    }


}
