using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class HeatMap : MonoBehaviour
{
    // Start is called before the first frame update
    HeatCube test;

    private void OnEnable()
    {
        test = new HeatCube(Vector3.zero,Vector3.one);
    }

    private void Start()
    {
        Camera.onPreRender += OnPostRenderCallback;
    }

    private void OnPostRenderCallback(Camera cam)
    {


    }
    private void Update()
    {
        test.RenderHeat();
    }
    private void OnDrawGizmos()
    {


    }
    void OnDestroy()
    {
        Camera.onPreRender -= OnPostRenderCallback;
    }

}

[CustomEditor(typeof(HeatMap))]
[CanEditMultipleObjects]
public class HeatMapEditor : Editor
{

}
