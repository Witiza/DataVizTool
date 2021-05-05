using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HeatCube
{
    Mesh mesh;
    static Material lineMaterial =null;
    Vector3 position;
    Material mat;
    

    Matrix4x4 transform;

    public HeatCube(Vector3 position,Vector3 scale)
    {
        transform = Matrix4x4.TRS(position, Quaternion.identity, scale);
        mesh = Shapes.GetUnityPrimitiveMesh(PrimitiveType.Cube);
        CreateLineMaterial();
    }

    void CreateLineMaterial()
    {

            mat = new Material(Shader.Find("Unlit/Color"));
            mat.color = Color.green;
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            //Shader shader = Shader.Find("Unlit/Color");
            //lineMaterial = new Material(shader);
            //lineMaterial.color = Color.green;
            //lineMaterial.hideFlags = HideFlags.HideAndDontSave;

            // Turn on alpha blending
           mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            //  lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            //  lineMaterial.SetInt("_ZWrite", 0);

        
    }
    public void RenderHeat()
    {
        Graphics.DrawMesh(mesh,transform,mat,0); //LAYER AS AN OPTION
    }
}
