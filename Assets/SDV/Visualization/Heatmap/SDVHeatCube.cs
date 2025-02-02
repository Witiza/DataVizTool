﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SDVHeatCube
{
    public List<SDVBaseEvent> events = new List<SDVBaseEvent>();

    Material mat;
    public int max_events;
    public float alpha = 0;
    SDVHeatmap parent;
    public bool selected = false;

    int events_in_use = 0;
    

    Matrix4x4 transform;
    public Vector3 position;
    Vector3 scale;
    Quaternion rotation;

    public SDVHeatCube(Vector3 _position,Vector3 _scale, Material material, SDVHeatmap map)
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
        foreach(SDVBaseEvent tmp in events)
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
        alpha = events_in_use / (float)max_events;
        Color color = parent.gradient.Evaluate(alpha);
        mat.color = color;
    }
  
    public void RenderHeat(Mesh mesh)
    {
        if (alpha>0)
        {
            Graphics.DrawMesh(mesh, transform, mat, 0); //LAYER AS AN OPTION
        }
    }

    public void RenderGizmo(HeatCubeShape shape)
    {
        if (alpha > 0)
        {
            Gizmos.color = mat.color;
            switch (shape)
            {
                case HeatCubeShape.CUBE:
                    Gizmos.DrawCube(position, scale);
                    break;
                case HeatCubeShape.SPHERE:
                    Gizmos.DrawSphere(position, scale.magnitude);

                    break;
            }

           
        }
    }

    public void generateHeight()
    {
        float median_height = 0;
        foreach(SDVBaseEvent ev in events)
        {
            median_height += ev.position.y;
        }
        median_height /= events.Count; 
        position.y = median_height;
    }

    public void generateEventsInUse()
    {
        events_in_use = 0;
        foreach (SDVBaseEvent ev in events)
        {
            if (parent.checkIfUsingEvent(ev.name))
            {
                events_in_use++;
            }
        }
    }
    public void generateSize()
    {
        scale = new Vector3(parent.cube_size, parent.cube_size, parent.cube_size);
        if (parent.modify_size)
        {
            scale += Vector3.one *(parent.size_multiplier * events_in_use);
        }
    }

    public void generateTransform()
    {
        transform.SetTRS(position, rotation, scale);
    }



}
