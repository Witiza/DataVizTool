using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GLShapes 
{
    static Material material = null;

    static void CreateLineMaterial()
    {
        if (!material)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            material = new Material(shader);
            material.hideFlags = HideFlags.HideAndDontSave;

            // Turn on alpha blending
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            material.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            material.SetInt("_ZWrite", 0);

        }
    }
    static void Circle(float x, float y, float radius, Transform transform, Color color)
    {
        material.SetPass(0);
        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);
        GL.Begin(GL.LINES);
        GL.Color(color);
        for (int i = 0; i < 360; i++)
        {
            float degInRad = i * Mathf.Deg2Rad;
            GL.Vertex3(x, y, 11);
            GL.Vertex3(x + Mathf.Cos(degInRad) * radius, y + Mathf.Sin(degInRad) * radius, 11);
        }
        GL.End();
    }
}
