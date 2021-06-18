using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class SDVGameObjects : SDV
{
    //This two go together
    [MenuItem("Window/SDV/SceneData")]
    static void Init()
    {
        SDVGameObjects window = (SDVGameObjects)EditorWindow.GetWindow(typeof(SDVGameObjects));
        window.Show();
    }


    List<SDVEventTracker> trackers = new List<SDVEventTracker>();
    bool generated;
    public float y_multiplier = 1;
    public bool sepparated;
    public bool selection;

    void generateSceneView()
    {
        cleanTrackers();
        assignEvents();
        generateMaxEvents();
        generateColors();
        sepparateEvents();
    }
    void Awake()
    {
        getEventHandler();
        trackers = new List<SDVEventTracker>(GameObject.FindObjectsOfType<SDVEventTracker>());
        if(trackers.Count<=0)
        {
            Debug.LogWarning("Tyring to visualize events in the scene without any EventTracker");
        }
        generated = false;
    }

    void cleanTrackers()
    {
        foreach(SDVEventTracker tracker in trackers)
        {
            tracker.events.Clear();
            tracker.sepparated_events.Clear();
        }
    }
    void assignEvents()
    {
        foreach(SDVEventContainer container in events)
        {
            
            if(container.use_target&&container.in_use)
            {
                foreach(SDVBaseEvent ev in container.events)
                {
                    SDVEventTracker tracker = ev.target_go.GetComponent<SDVEventTracker>();
                    if(tracker)
                    {
                        tracker.events.Add(ev);
                    }
                    else
                    {
                        Debug.LogWarning("Event discarded because there was no event_tracker attached to the target");
                    }
                }
            }
        }
    }

    void generateMaxEvents()
    {
        foreach(SDVEventTracker tracker in trackers)
        {
            if(tracker.events.Count > max_events)
            {
                max_events = tracker.events.Count;
            }
        }
    }


    void generateColors()
    {
        foreach (SDVEventTracker tracker in trackers)
        {
            tracker.generateColor();
        }
    }

    void sepparateEvents()
    {
        foreach (SDVEventTracker tracker in trackers)
        {
            tracker.sepparateEvents();
        }
    }
    private void OnGUI()
    {
        setStyles();
        GUILayout.Label("SceneViewer", inspector_title);

        gradient = EditorGUILayout.GradientField("Color: ", gradient);

        DrawUILine(5,20);

        if (GUILayout.Button("Distribute Events"))
        {
            generateSceneView();
        }
        DrawUILine(5, 20);

        sepparated = EditorGUILayout.Toggle("View events sepparatedly", sepparated);

        selection = EditorGUILayout.Toggle("View events only for selected GameObjects", selection);

        yoffset = EditorGUILayout.FloatField("Y offset", yoffset);

        y_multiplier = EditorGUILayout.FloatField("Y multiplier", y_multiplier);


        size_multiplier = Mathf.Abs(EditorGUILayout.FloatField("Size multiplier", size_multiplier));


        DrawUILine(5,20);

        if (getEventHandler())
        {
            foreach (StandardEvent st_ev in event_handler.events)
            {
                if (!checkIfLoaded(st_ev))
                {
                    if (GUILayout.Button("Load " + st_ev.name + " Events"))
                    {
                        events.Add(SDVCSVhandling.LoadCSV(st_ev.name, SceneManager.GetActiveScene().name, SDVCSVhandling.dataTypeToString(st_ev.data_type)));
                    }
                }
            }
        }
        else
        {
            Debug.LogError("No EventHandler in the Scene");
        }
        foreach (SDVEventContainer ev in events)
        {
            if (!ev.empty)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUI.BeginChangeCheck();
                ev.in_use = EditorGUILayout.Toggle("View " + ev.name, ev.in_use);
                if (EditorGUI.EndChangeCheck())
                {
                    generateSceneView();
                }
                EditorGUI.BeginChangeCheck();
                ev.color = EditorGUILayout.ColorField(ev.name+"'s Color", ev.color);
                if (EditorGUI.EndChangeCheck())
                {
                    generateSceneView();
                }
                EditorGUILayout.EndHorizontal();
                
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
        foreach (SDVEventContainer tmp in events)
        {
            if (tmp.name == ev.name)
            {
                ret = true;
            }
        }
        return ret;
    }
}



