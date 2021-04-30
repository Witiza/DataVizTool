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

    Camera camera;
    RenderTexture texture;


    private void Start()
    {
        camera = GetComponent<Camera>();
    }
}

[CustomEditor(typeof(DataViewer))]
public class DataViewerEditor : Editor
{

}
