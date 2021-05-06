using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class HeatMap : MonoBehaviour
{
    // Start is called before the first frame update
    public Material material;
    public int cube_size = 1;
    float max_x = 0;
    float min_x=0;
    float max_z=0;
    float min_z=0;
    int x_cells;
    int z_cells;
    int max_events = 0;
    HeatCube[,] heatmap;
    public List<EventContainer> events = new List<EventContainer>();

    public void createHeatMap()
    {
        heatmap = null;
        if (events.Count == 0)
        {
            events.Add(CSVhandling.LoadCSV("Position", "TestScene", "VECTOR3"));
            events.Add(CSVhandling.LoadCSV("Position2", "TestScene", "VECTOR3"));
        }
       // Debug.Log("Events count:" + events.Count + "Amount of positions: " + events[0].events.Count);
        calculateSize();
        //Debug.Log("MAX X: " + max_x + "MAX Y: " + max_z);
        //Debug.Log("MIN X: " + min_z + "MIN Y: " + min_z);

        float x_dist = Mathf.Abs(max_x - min_x);
        float z_dist = Mathf.Abs(max_z - min_z);

        //cute +1
         x_cells = Mathf.CeilToInt(x_dist / cube_size)+1;
         z_cells = Mathf.CeilToInt(z_dist / cube_size)+1;
        heatmap = new HeatCube[x_cells,z_cells];

        for (int i = 0; i < heatmap.GetLength(0); i++)
        {
            for (int j = 0; j < heatmap.GetLength(1); j++)
            {
                heatmap[i, j] = new HeatCube(new Vector3(min_x+i*cube_size ,10, min_z + j * cube_size),new Vector3(cube_size, cube_size, cube_size),material,this);
            }
        }

        distributeEvents();
        calculateAndAssignMaxEvents();
        generateColors();
    }

    public bool checkIfUsingEvent(string name)
    {

        foreach(EventContainer tmp in events)
        {
            if(tmp.name == name)
            {
                if(tmp.in_use)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void generateColors()
    {
        for (int i = 0; i < heatmap.GetLength(0); i++)
        {
            for (int j = 0; j < heatmap.GetLength(1); j++)
            {
                heatmap[i, j].generateColor();
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
            for (int i = 0; i < heatmap.GetLength(0); i++)
            {
                for (int j = 0; j < heatmap.GetLength(1); j++)
                {
                    heatmap[i, j].RenderHeat();
                }
            }
        }

    }

    void distributeEvents()
    {
        foreach (EventContainer ev in events)
        {
            if (ev.type == DataType.VECTOR3)
            {
                assignEvent(ev);
            }
        }
    }

    void calculateAndAssignMaxEvents()
    {
        for (int i = 0; i < heatmap.GetLength(0); i++)
        {
            for (int j = 0; j < heatmap.GetLength(1); j++)
            {
                int cube_events = heatmap[i, j].getEventAmount();
                if (max_events < cube_events )
                {
                    max_events = cube_events;
                }
            }
        }

        for (int i = 0; i < heatmap.GetLength(0); i++)
        {
            for (int j = 0; j < heatmap.GetLength(1); j++)
            {
                heatmap[i, j].max_events = max_events;
            }
        }
    }
    private void OnDrawGizmos()
    {
      
    }



    void assignEvent(EventContainer ev)
    {

        foreach (Vector3Event tmp in ev.events)
        {
            int x_pos = Mathf.FloorToInt(Mathf.Abs(tmp.data.x - min_x) / cube_size);
            int z_pos = Mathf.FloorToInt(Mathf.Abs(tmp.data.z - min_z) / cube_size);
            heatmap[x_pos, z_pos].events.Add(tmp);
        }

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
        EditorUtility.SetDirty(target);//it executes every frame
        DrawDefaultInspector();
        if(GUILayout.Button("Generate Heatmap"))
        {
            map.createHeatMap();
        }
        if (GUILayout.Button("Delete Heatmap"))
        {
            map.deleteHeatmap();
        }

        foreach(EventContainer ev in map.events)
        {
            EditorGUI.BeginChangeCheck();
            ev.in_use = EditorGUILayout.Toggle("View " + ev.name, ev.in_use);
            if(EditorGUI.EndChangeCheck())
            {
                map.generateColors();
            }
        }
    }
}
