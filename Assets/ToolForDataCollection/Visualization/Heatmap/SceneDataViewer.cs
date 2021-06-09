using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class SceneDataViewer : DataViewer
{
    //This two go together
    [MenuItem("Window/Tool/DataViz/SceneData")]
    static void Init()
    {
        SceneDataViewer window = (SceneDataViewer)EditorWindow.GetWindow(typeof(SceneDataViewer));
        window.Show();
    }


    List<EventTracker> trackers = new List<EventTracker>();
    bool generated;

    void generateSceneView()
    {
        cleanTrackers();
        assignEvents();
        generateMaxEvents();
        generateColors();
    }
    void Awake()
    {
        getEventHandler();
        trackers = new List<EventTracker>(GameObject.FindObjectsOfType<EventTracker>());
        if(trackers.Count<=0)
        {
            Debug.LogWarning("Tyring to visualize events in the scene without any EventTracker");
        }
        generated = false;
    }

    void cleanTrackers()
    {
        foreach(EventTracker tracker in trackers)
        {
            tracker.events.Clear();
        }
    }
    void assignEvents()
    {
        foreach(EventContainer container in events)
        {

            if(container.use_target)
            {
                foreach(BaseEvent ev in container.events)
                {
                    EventTracker tracker = ev.target_go.GetComponent<EventTracker>();
                    if(tracker)
                    {
                        tracker.events.Add(ev);
                    }
                    else
                    {
                        Debug.LogWarning("Event discarded because there was no event_tracker attached to the target");
                    }


                    //foreach (EventTracker tracker in trackers)
                    //{
                    //    if(ev.target_GUID == tracker.getGUID()) //there must be a better way to do this for sue
                    //    {
                    //        tracker.events.Add(ev);
                    //    }
                    //}
                }
            }
        }
    }

    void generateMaxEvents()
    {
        foreach(EventTracker tracker in trackers)
        {
            if(tracker.events.Count > max_events)
            {
                max_events = tracker.events.Count;
            }
        }
    }


    void generateColors()
    {
        foreach (EventTracker tracker in trackers)
        {
            tracker.generateColor();
        }
    }
    private void OnGUI()
    {
        setStyles();
        GUILayout.Label("SceneViewer", inspector_title);

        gradient = EditorGUILayout.GradientField("Color: ", gradient);

        DrawUILine(Color.white,5,20);

        if (GUILayout.Button("Distribute Events"))
        {
            generateSceneView();
        }

        DrawUILine(Color.white,5,20);

        if (getEventHandler())
        {
            foreach (StandardEvent st_ev in event_handler.events)
            {
                if (!checkIfLoaded(st_ev))
                {
                    if (GUILayout.Button("Load " + st_ev.name + " Events"))
                    {
                        events.Add(CSVhandling.LoadCSV(st_ev.name, SceneManager.GetActiveScene().name, CSVhandling.dataTypeToString(st_ev.data_type)));
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
                ev.in_use = EditorGUILayout.Toggle("View " + ev.name, ev.in_use);
                if (EditorGUI.EndChangeCheck())
                {
                   //do something
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
        foreach (EventContainer tmp in events)
        {
            if (tmp.name == ev.name)
            {
                ret = true;
            }
        }
        return ret;
    }
}



