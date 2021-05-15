using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HeatMapViewer : EditorWindow
{
    [MenuItem("Window/Tool/DataViz/HeatMap")]
    static void Init()
    {
        HeatMapViewer window = (HeatMapViewer)EditorWindow.GetWindow(typeof(HeatMapViewer));
        window.Show();
    }

    // Start is called before the first frame update
    public Material material;
    public int cube_size = 1;
    float max_x = 0;
    float min_x = 0;
    float max_z = 0;
    float min_z = 0;
    int x_cells;
    int z_cells;
    int max_events = 0;
    HeatCube[,] heatmap;
    public List<EventContainer> events = new List<EventContainer>();

    public Gradient gradient = new Gradient();

    public void createHeatMap()
    {
        heatmap = null;
        if (events.Count == 0)
        {
            //events.Add(CSVhandling.LoadCSV("Position", "TestScene", "VECTOR3"));
            //events.Add(CSVhandling.LoadCSV("Position2", "TestScene", "VECTOR3"));
            events.Add(CSVhandling.LoadCSV("Position", "ExampleScene", "NULL"));

        }
        // Debug.Log("Events count:" + events.Count + "Amount of positions: " + events[0].events.Count);
        calculateSize();
        //Debug.Log("MAX X: " + max_x + "MAX Y: " + max_z);
        //Debug.Log("MIN X: " + min_z + "MIN Y: " + min_z);

        float x_dist = Mathf.Abs(max_x - min_x);
        float z_dist = Mathf.Abs(max_z - min_z);

        //cute +1
        x_cells = Mathf.CeilToInt(x_dist / cube_size) + 1;
        z_cells = Mathf.CeilToInt(z_dist / cube_size) + 1;
        heatmap = new HeatCube[x_cells, z_cells];

        for (int i = 0; i < heatmap.GetLength(0); i++)
        {
            for (int j = 0; j < heatmap.GetLength(1); j++)
            {
                heatmap[i, j] = new HeatCube(new Vector3(min_x + i * cube_size, 10, min_z + j * cube_size), new Vector3(cube_size, cube_size, cube_size), material, this);
            }
        }

        distributeEvents();
        calculateAndAssignMaxEvents();
        adjoustmentsToCubes();
    }


    public bool checkIfUsingEvent(string name)
    {

        foreach (EventContainer tmp in events)
        {
            if (tmp.name == name)
            {
                if (tmp.in_use)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void adjoustmentsToCubes()
    {
        for (int i = 0; i < heatmap.GetLength(0); i++)
        {
            for (int j = 0; j < heatmap.GetLength(1); j++)
            {
                heatmap[i, j].generateColor();
                heatmap[i, j].generateHeight();
            }
        }
    }
    void calculateSize()
    {
        foreach (EventContainer ev in events)
        {

                foreach (BaseEvent tmp in ev.events)
                {
                    if (tmp.position.x < min_x)
                    {
                        min_x = tmp.position.x;
                    }
                    if (tmp.position.x > max_x)
                    {
                        max_x = tmp.position.x;
                    }
                    if (tmp.position.z < min_z)
                    {
                        min_z = tmp.position.z;
                    }
                    if (tmp.position.z > max_z)
                    {
                        max_z = tmp.position.z;
                    }
                }
            
        }
    }

    public void deleteHeatmap()
    {
        heatmap = null;
    }

    void RenderHeatMap()
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
            if (ev.use_position)
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
                if (max_events < cube_events)
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
    void assignEvent(EventContainer ev)
    {

        foreach (BaseEvent tmp in ev.events)
        {
            int x_pos = Mathf.FloorToInt(Mathf.Abs(tmp.position.x - min_x) / cube_size);
            int z_pos = Mathf.FloorToInt(Mathf.Abs(tmp.position.z - min_z) / cube_size);
            heatmap[x_pos, z_pos].events.Add(tmp);
        }

    }

    void OnGUI()
    {
        //doing this in update causes extreme lag bruv
        RenderHeatMap();
        if(!material)
        {
            GUI.enabled = false;
        }
        if (GUILayout.Button("Generate Heatmap"))
        {
            createHeatMap();
        }
        GUI.enabled = true;
        if (GUILayout.Button("Delete Heatmap"))
        {
            deleteHeatmap();
        }

        EditorGUI.BeginChangeCheck();
        gradient = EditorGUILayout.GradientField("Color: ", gradient);
        if (EditorGUI.EndChangeCheck())
        {
            if(heatmap != null)
                adjoustmentsToCubes();
        }


        material = (Material)EditorGUILayout.ObjectField("Material: ",material, typeof(Material), true);


        EditorGUILayout.LabelField("X cells: " + x_cells);
        EditorGUILayout.LabelField("Z cells: " + z_cells);
        EditorGUILayout.LabelField("Max events per cell: " + max_events);
        foreach (EventContainer ev in events)
        {
            EditorGUI.BeginChangeCheck();
            ev.in_use = EditorGUILayout.Toggle("View " + ev.name, ev.in_use);
            if (EditorGUI.EndChangeCheck())
            {
                if (heatmap != null)
                    adjoustmentsToCubes();
            }
        }
    }
}
