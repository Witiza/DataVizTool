using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HeatCube
{
    public List<Vector3Event> events = new List<Vector3Event>();
    Mesh mesh;
    public Material lineMaterial =null;
    Vector3 position;
    Material mat;
    public int max_events;

    HeatMap parent;
    

    Matrix4x4 transform;

    public HeatCube(Vector3 position,Vector3 scale, Material material, HeatMap map)
    {
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
        
        float alpha = (getEventAmount()) / (float)(max_events);
        Color color = mat.color;
        color.a = alpha;

        //if(getEventAmount() >0)
        //{
        //    Debug.Log("Event amount: " + getEventAmount());
        //    Debug.Log("Alpha: " + alpha);
        //    Debug.Log("Actual divison: "+ getEventAmount() / (float)max_events);
        //}

        if (getEventAmount() != 0)
        {
            Debug.Log("Num of events: " + events.Count);
            Debug.Log("Max events: " + max_events);
            Debug.Log("Alpha: " + alpha);
        }
        mat.color = color;
    }
    void CreateLineMaterial()
    {

           // mat = new Material(Shader.Find("Unlit/Color"));
           // Color mat_color = Color.green;
           // mat_color.a = 0.5f;
           // mat.color = mat_color;
           // mat.


           // // Unity has a built-in shader that is useful for drawing
           // // simple colored things.
           // //Shader shader = Shader.Find("Unlit/Color");
           // //lineMaterial = new Material(shader);
           // //lineMaterial.color = Color.green;
           // //lineMaterial.hideFlags = HideFlags.HideAndDontSave;

           // // Turn on alpha blending
           //mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
           // mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
           // // Turn backface culling off
           // //  lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
           // // Turn off depth writes
           // //  lineMaterial.SetInt("_ZWrite", 0);

        
    }
    public void RenderHeat()
    {
        Graphics.DrawMesh(mesh,transform,mat,0); //LAYER AS AN OPTION
    }
}
