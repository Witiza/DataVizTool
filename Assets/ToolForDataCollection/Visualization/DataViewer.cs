using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class DataViewer : MonoBehaviour
{
    // Start is called before the first frame update
    public int lineCount = 100;
    public float radius = 3.0f;
    Vector3 initial_pos;
    Vector3 final_pos;
    Camera texture_camera;
    RenderTexture texture;
    HeatMapViewer heatmap;
    static Material lineMaterial;

    void Start()
    {
        texture_camera = GetComponent<Camera>();
        CreateLineMaterial();
        heatmap = EditorWindow.GetWindow<HeatMapViewer>();
        //texture = new RenderTexture((int)positio)
    }

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

    public void setBoundingBox(Vector3 pos,Vector3 size)
    {
        initial_pos = pos;
        final_pos = size;
    }
    private void OnRenderObject()
    {

            if (Camera.current == texture_camera)
        { 
            CreateLineMaterial();
            lineMaterial.SetPass(0);
            GL.PushMatrix();
            GL.MultMatrix(transform.localToWorldMatrix);
            GL.Begin(GL.QUADS);
            GL.Vertex3(10, 10, 10);
            GL.Vertex3(10, -10, 10);
            GL.Vertex3(-10, -10, 10);
            GL.Vertex3(-10, 10, 10);
            GL.End();
            GL.PopMatrix();
            
            
        }
    }

    void OnDestroy()
    {


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
                    Gizmos.DrawCube(center, (final_pos - initial_pos));
                }
            }
        }
        else
        {
            //We dont want to open a new one always, we just want to get the reference if the window is open
           if(EditorWindow.HasOpenInstances<HeatMapViewer>())
            {
                heatmap=EditorWindow.GetWindow<HeatMapViewer>();
            }
        }
    }
}

