﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class DataViewer : MonoBehaviour
{
    // Start is called before the first frame update
    public int lineCount = 100;
    public float radius = 3.0f;

    Camera camera;
    RenderTexture texture;

    static Material lineMaterial;
    static void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    private void Start()
    {
        camera = GetComponent<Camera>();
        CreateLineMaterial();
        //texture = new RenderTexture((int)positio)
    }

    private void OnRenderObject()
    {

        if (Camera.current == camera)
        { 
            CreateLineMaterial();
            lineMaterial.SetPass(0);
            GL.PushMatrix();
            GL.MultMatrix(transform.localToWorldMatrix);
            GL.Begin(GL.QUADS);
            GL.Vertex3(100, 100, 10);
            GL.Vertex3(100, -100, 10);
            GL.Vertex3(-100, -100, 10);
            GL.Vertex3(-100, 100, 10);
            GL.End();
            GL.PopMatrix();
        }
    }
}

[CustomEditor(typeof(DataViewer))]
public class DataViewerEditor : Editor
{
    
}
