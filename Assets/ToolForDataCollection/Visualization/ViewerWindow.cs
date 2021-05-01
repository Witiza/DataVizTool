using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class ViewerWindow : EditorWindow
{
    Camera camera = null;
    RenderTexture texture;
    DataViewer script = null;

    [MenuItem("Window/Tool/DataViz/ViewerWindow")]
    public static void ShowWindow()
    {
        GetWindow<ViewerWindow>("ViewerWindow");
    }
    static void Init()
    {
        EditorWindow editorWindow = GetWindow(typeof(ViewerWindow));
        editorWindow.autoRepaintOnSceneChange = true;
        editorWindow.Show();
    }
    public void Awake()
    {
        generateTexture();
    }
    void generateTexture()
    {
        texture = new RenderTexture((int)position.width, (int)position.height, (int)RenderTextureFormat.ARGB32);

    }
    public void OnEnable()
    {
        if (!(script = FindObjectOfType<DataViewer>()))
        {
            Debug.LogError("DataViewer component not found in the scene");
        }
        if (!(camera = script.gameObject.GetComponent<Camera>()))
        {
            Debug.LogError("There is not a Camera attached to the DataViewer GameObject");
        }
    }
    void OnPostRender()
    {
        
      
    }

    public void Update()
    {
        if (texture != null)
        {
            if (camera != null)
            {
                camera.targetTexture = texture;
                camera.Render();
                camera.targetTexture = null;
            }
            if (texture.width != position.width ||
                texture.height != position.height)
                texture = new RenderTexture((int)position.width,
                    (int)position.height,
                    (int)RenderTextureFormat.ARGB32);
        }
        else
        {
            generateTexture();
        }
    }

    void OnGUI()
    {
        if (texture != null)
        {
            GUI.DrawTexture(new Rect(position.width/2, 0.0f, position.width, position.height), texture);
        }
        else
        {
            generateTexture();
        }
    }

}
