﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public enum HeatCubeShape
{
    CUBE,
    SPHERE
};

public class HeatMapViewer : DataViewer
{
    //This two go together
    [MenuItem("Window/Tool/DataViz/HeatMap")]
    static void Init()
    {
        HeatMapViewer window = (HeatMapViewer)EditorWindow.GetWindow(typeof(HeatMapViewer));
        window.Show();

    }

    int lastRenderedFrame = 0;
    // Start is called before the first frame update
    bool dirty;
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
    HeatCube[,] heatmap;
    List<HeatCube> selected = new List<HeatCube>();
    Dictionary<string, Pair<BaseEvent, int>> histogram = new Dictionary<string, Pair<BaseEvent, int>>();
    int max_histogram = 0;
    HeatSelection selection;
    public HeatMapRenderer renderer;
    int selected_amount = 0;

    Mesh cube = null;
    Mesh sphere = null;
    HeatCubeShape shape;

    float m_Value;


    void Awake()
    {
        getRenderer();
        getEventHandler();
        loadMeshes();
      
        selection = new HeatSelection(this);
    }

   public  void getRenderer()
    {
        if ((renderer = GameObject.FindObjectOfType<HeatMapRenderer>()))
        {
            renderer.heatmap = this;
        }
        else
        {
            Debug.LogError("There is not a HeatMapRenderer Component in the scene");
        }
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
        dirty = false;
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

   

   
    public void adjoustmentsToCubes()
    {

        for (int i = 0; i < heatmap.GetLength(0); i++)
        {
            for (int j = 0; j < heatmap.GetLength(1); j++)
            {
                heatmap[i, j].generateEventsInUse();
                heatmap[i, j].generateSize();
                heatmap[i, j].generateColor();
                heatmap[i, j].generateHeight();
                heatmap[i, j].generateTransform();
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
                       if(!selected.Contains(heatmap[i,j]))
                       {
                            selected.Add(heatmap[i, j]);
                        }
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

    void generateDictionary()
    {
        histogram.Clear();
        if(visualize_selection)
        {
            foreach(HeatCube cube in selected)
            {
                foreach(BaseEvent ev in cube.events)
                {
                    if(checkIfUsingEvent(ev.name))
                    {
                        if(histogram.ContainsKey(ev.name))
                        {
                            histogram[ev.name].Second++;
                        }
                        else
                        {
                            histogram.Add(ev.name, new Pair<BaseEvent, int>(ev, 1));
                        }
                    }
                }
            }
        }
        else
        {
            foreach(EventContainer cont in events)
            {
                if(!cont.empty && cont.in_use)
                {
                    if (histogram.ContainsKey(cont.name))
                    {
                        histogram[cont.name].Second++;
                    }
                    else
                    {
                        histogram.Add(cont.name, new Pair<BaseEvent, int>(cont.events[0], cont.events.Count));//Nasty
                    }
                }
            }
        }
        max_histogram = 0;
        foreach(var tmp in histogram)
        {
            if(tmp.Value.Second >max_histogram)
            {
                max_histogram = tmp.Value.Second;
            }
        }
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
        //https://answers.unity.com/questions/594420/how-to-flush-mesh-batch-in-editor-or-how-to-draw-a.html
        if (lastRenderedFrame != Time.renderedFrameCount)
        {
            RenderHeatMap();
            lastRenderedFrame = Time.renderedFrameCount;
        }

        //doing this in update causes extreme lag bruv
        if (heatmap != null && selecting)
        {
            selection.SelectCubes(heatmap);
            foreach(HeatCube cube in selected)
            {
                if (!cube.selected)
                    selected.Remove(cube);
            }
        }

        if (selection != null)
        {
            if(selecting)
                selection.selectionByHandles();
        }
        else
        {
            selection = new HeatSelection(this);
        }
    }

    void OnGUI()
    {
        setStyles();
       
       
        GUILayout.Label("HeatMap",inspector_title);
        if(dirty)
        {
            GUILayout.Label("Generate the Heatmap to apply changes");
        }
        if (!material)
        {

            EditorGUILayout.LabelField("Add a Material");
            GUI.enabled = false;
        }
        if(events.Count==0)
        {

            EditorGUILayout.LabelField("No Events Loaded");
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
           // 04/06 still does not work
           RenderHeatMap();
            //EditorWindow view = EditorWindow.GetWindow<SceneView>();
          //  SceneView.currentDrawingSceneView.Repaint();
            lastRenderedFrame = 0;
           // view.Repaint();
        }
        shape = (HeatCubeShape)EditorGUILayout.EnumPopup("Heat Cubes Shape: ", shape);


        EditorGUI.BeginChangeCheck();

        cube_size = EditorGUILayout.FloatField("Cube Size", cube_size);
        if(EditorGUI.EndChangeCheck())
        {
            dirty = true;
        }
        EditorGUI.BeginChangeCheck();

        modify_size = EditorGUILayout.Toggle("Modify Size", modify_size);
        if(modify_size)
            size_multiplier  = EditorGUILayout.FloatField("Modifier Size", size_multiplier);
        if (EditorGUI.EndChangeCheck())
        {
            adjoustmentsToCubes();
        }

            EditorGUILayout.LabelField("Selected Cubes: " + selected_amount);
        EditorGUILayout.LabelField("X cells: " + x_cells);
        EditorGUILayout.LabelField("Z cells: " + z_cells);
        EditorGUILayout.LabelField("Max events per cell: " + max_events);

     

        if (GUILayout.Button("Generate Dictionary"))
        {
            generateDictionary();
        }

        foreach(var entry in histogram)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(entry.Value.First.name + ": "+entry.Value.Second);
            Rect r = EditorGUILayout.GetControlRect();
            r.width = ((float)entry.Value.Second/(float)max_histogram)*(position.width *2/3);
            r.x -= position.width / 4;
            Debug.Log("Width: " + r.width);

            EditorGUI.DrawRect(r, getEventColor(entry.Value.First.name));
            GUILayout.EndHorizontal();
        }
        if (getEventHandler())
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
                EditorGUILayout.BeginHorizontal();

                EditorGUI.BeginChangeCheck();
                ev.in_use = EditorGUILayout.Toggle("View " + ev.name, ev.in_use);
                if (EditorGUI.EndChangeCheck())
                {
                    if (heatmap != null)
                        adjoustmentsToCubes();
                }
                ev.color = EditorGUILayout.ColorField(ev.name + "'s Color", ev.color);
                EditorGUILayout.EndHorizontal();
            }
            else
            {
               Debug.LogWarning("There is no CSV file for " + ev.name);
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

