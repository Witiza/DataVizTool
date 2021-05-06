using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class HeatMap : MonoBehaviour
{
    // Start is called before the first frame update
    HeatCube test;
    public int cube_size = 1;
    float max_x = 0;
    float min_x=0;
    float max_z=0;
    float min_z=0;
    HeatCube[,] heatmap;
    List<EventContainer> events = new List<EventContainer>();

    private void OnEnable()
    {
        test = new HeatCube(Vector3.zero,Vector3.one);
    }

    public void createHeatMap()
    {
        events.Add(CSVhandling.LoadCSV("Position", "TestScene", "VECTOR3"));
        Debug.Log("Events count:" + events.Count + "Amount of positions: " + events[0].events.Count);
        calculateSize();
        Debug.Log("MAX X: " + max_x + "MAX Y: " + max_z);
        Debug.Log("MIN X: " + min_z + "MIN Y: " + min_z);

        float x_dist = Mathf.Abs(max_x - min_x);
        float z_dist = Mathf.Abs(max_z - min_z);

        //cute +1
        int x_cells = Mathf.CeilToInt(x_dist / cube_size)+1;
        int z_cells = Mathf.CeilToInt(z_dist / cube_size)+1;

        Debug.Log("X: " + x_cells + "Y: " + z_cells);
        heatmap = new HeatCube[x_cells,z_cells];

        for (int i = 0; i < heatmap.GetLength(0); i++)
        {
            for (int j = 0; j < heatmap.GetLength(1); j++)
            {
                heatmap[i, j] = new HeatCube(new Vector3(min_x+i*cube_size ,10, min_z + j * cube_size),new Vector3(cube_size, cube_size, cube_size));
            }
        }
    }

    void calculateSize()
    {
        foreach(EventContainer ev in events)
        {
            if(ev.type == DataType.VECTOR3)
            {
                foreach(Vector3Event tmp in ev.events)
                {
                    if(tmp.data.x < min_x)
                    {
                        min_x = tmp.data.x;
                    }
                    if (tmp.data.x > max_x)
                    {
                        max_x = tmp.data.x;
                    }
                    if (tmp.data.z < min_z)
                    {
                        min_z =tmp.data.z;
                    }
                    if (tmp.data.z > max_z)
                    {
                        max_z =tmp.data.z;
                    }
                }
            }
        }
    }

    public void deleteHeatmap()
    {
        heatmap = null;
    }
  
    void Update()
    {
        if (heatmap != null)
        {
            Debug.Log(heatmap.Length);
            for (int i = 0; i < heatmap.GetLength(0); i++)
            {
                for (int j = 0; j < heatmap.GetLength(1); j++)
                {
                    heatmap[i, j].RenderHeat();
                }
            }
        }
        else
        {
            Debug.Log("Null heatmap");
        }
        
    }
    private void OnDrawGizmos()
    {


    }


}

[CustomEditor(typeof(HeatMap))]
[CanEditMultipleObjects]
public class HeatMapEditor : Editor
{
    HeatMap map;

    private void OnEnable()
    {
        map  = (HeatMap)target;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if(GUILayout.Button("Generate Heatmap"))
        {
            map.createHeatMap();
        }
        if (GUILayout.Button("Delete Heatmap"))
        {
            map.deleteHeatmap();
        }
    }
}
