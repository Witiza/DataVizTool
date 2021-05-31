using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public enum HeatCubeShape
{
    CUBE,
    SPHERE
};

public class HeatMapViewer : EditorWindow
{
    //This two go together
    [MenuItem("Window/Tool/DataViz/HeatMap")]
    static void Init()
    {
        HeatMapViewer window = (HeatMapViewer)EditorWindow.GetWindow(typeof(HeatMapViewer));
        window.Show();

    }

    // Start is called before the first frame update
    public Material material;
    public float cube_size = 1f;
    public bool selecting = false;
    public bool visualize_selection = false;
    float max_x = 0;
    float min_x = 0;
    float max_z = 0;
    float min_z = 0;
    int x_cells;
    int z_cells;
    int max_events = 0;
    HeatCube[,] heatmap;
    public List<EventContainer> events = new List<EventContainer>();
    HeatSelection selection = new HeatSelection();
    public Gradient gradient = new Gradient();
    int selected_amount = 0;
    EventHandler event_handler = null;
    Mesh cube = null;
    Mesh sphere = null;
    HeatCubeShape shape;


    GUIStyle title = new GUIStyle();

    GUIStyle text = new GUIStyle();
    void Awake()
    {
        if(!(event_handler = GameObject.FindObjectOfType<EventHandler>()))
        {
            Debug.LogError("There is no EventHandler component in the scene");
        }
        loadMeshes();
    }

    void setStyles()
    {
        title.fontSize = 20;
        title.normal.textColor = Color.white;
        title.alignment = TextAnchor.MiddleCenter;
    }

    void loadMeshes()
    {
        if(cube == null)
            cube = Shapes.GetUnityPrimitiveMesh(PrimitiveType.Cube);
        if (sphere == null)
            sphere = Shapes.GetUnityPrimitiveMesh(PrimitiveType.Sphere);
    }
    public void createHeatMap()
    {
        heatmap = null;
        x_cells = 0;
        z_cells = 0;
        max_x = 0;
        min_x = 0;
        max_z = 0;
        min_z = 0;
        max_events = 0;
        calculateSize();


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
        loadMeshes();
        selected_amount = 0;

        if (heatmap != null)
        {
            for (int i = 0; i < heatmap.GetLength(0); i++)
            {
                for (int j = 0; j < heatmap.GetLength(1); j++)
                {
                    if (!visualize_selection || heatmap[i, j].selected)
                    {
                        switch(shape)
                        {
                            case HeatCubeShape.CUBE:
                                heatmap[i, j].RenderHeat(cube);
                                break;
                            case HeatCubeShape.SPHERE:
                                heatmap[i, j].RenderHeat(sphere);
                                break;
                        }
                    }
                    if(heatmap[i,j].selected)
                    {
                        selected_amount++;
                    }
                }
            }
        }

    }

    void distributeEvents()
    {
        foreach (EventContainer ev in events)
        {
            if (ev.use_position&&!ev.empty)
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

                if(cube_events >0)
                {
                    Debug.Log("Events in Cube: "+cube_events);
                }
                if (max_events < cube_events)
                {
                    max_events = cube_events;

                }
            }
        }

        Debug.Log("MAX EVENTS ONLY"+max_events);

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

    private void Update()
    {
        selection.drawSelection();
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDestroy()
    {
        SceneView.duringSceneGui -= OnSceneGUI;

    }

    void OnSceneGUI(SceneView sv)
    {
        //doing this in update causes extreme lag bruv
        if (heatmap != null && selecting)
           selection.SelectCubes(heatmap);

        if (selection != null)
        {
            //  selection.MouseCheck(sv);
            if(selecting)
                selection.selectionByHandles();
        }
        else
        {
            selection = new HeatSelection();
        }
    }

    void OnGUI()
    {

        setStyles();
        
        RenderHeatMap();
       
        GUILayout.Label("HeatMap",title);
        if (!material)
        {
            GUI.enabled = false;
            EditorGUILayout.LabelField("Add a Material");
        }
        if(events.Count==0)
        {
            GUI.enabled = false;
            EditorGUILayout.LabelField("No Events Loaded");
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

        if(selecting)
        {
            if (GUILayout.Button("Stop HeatMap selection"))
            {
                selecting = false;
            }
        }
        else
        {
            if (GUILayout.Button("Select HeatMap area"))
            {
                selecting = true;
            }
        }
        EditorGUI.BeginChangeCheck();
        visualize_selection = EditorGUILayout.Toggle("Only Visualize Selection", visualize_selection);
        if (EditorGUI.EndChangeCheck())
        {
            Debug.Log("swiiitchi"); 
            //THIS DOESNT WORK !!!!!!!!!!!!!!!!!!!!!!!!!!!!
            RenderHeatMap();
            EditorWindow view = EditorWindow.GetWindow<SceneView>();
            //SceneView.currentDrawingSceneView.Repaint();
            view.Repaint();
        }

        shape = (HeatCubeShape)EditorGUILayout.EnumPopup("Heat Cubes Shape: ", shape);



        EditorGUILayout.LabelField("Selected Cubes: " + selected_amount);
        EditorGUILayout.LabelField("X cells: " + x_cells);
        EditorGUILayout.LabelField("Z cells: " + z_cells);
        EditorGUILayout.LabelField("Max events per cell: " + max_events);

        if(event_handler)
        {
            foreach(StandardEvent st_ev in event_handler.events)
            {
               if(!checkIfLoaded(st_ev))
                {
                    if(GUILayout.Button("Load "+st_ev.name+" Events"))
                    {
                        events.Add(CSVhandling.LoadCSV(st_ev.name, SceneManager.GetActiveScene().name,CSVhandling.dataTypeToString(st_ev.data_type)));
                    }
                }
            }
        }
        else
        {
            Debug.LogError("No EventHandler in the Scene");
        }
        foreach (EventContainer ev in events)
        {
            if (!ev.empty)
            {
                EditorGUI.BeginChangeCheck();
                ev.in_use = EditorGUILayout.Toggle("View " + ev.name, ev.in_use);
                if (EditorGUI.EndChangeCheck())
                {
                    if (heatmap != null)
                        adjoustmentsToCubes();
                }
            }
            else
            {
                EditorGUILayout.LabelField("There is no CSV file for " + ev.name);
            }
        }
    }

    bool checkIfLoaded(StandardEvent ev)
    {
        bool ret = false;
        foreach(EventContainer tmp in events)
        {
            if(tmp.name == ev.name)
            {
                ret = true;
            }
        }
        return ret;
    }
}

