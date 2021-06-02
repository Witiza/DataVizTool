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



    void Awake()
    {
        getEventHandler();
        trackers = new List<EventTracker>(GameObject.FindObjectsOfType<EventTracker>());
        if(trackers.Count<=0)
        {
            Debug.LogWarning("Tyring to visualize events in the scene without any EventTracker");
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

                    foreach (EventTracker tracker in trackers)
                    {
                        if(ev.target_go == tracker.gameObject) //there must be a better way to do this for sue
                        {
                            Debug.Log("YABBA DABBA DOOOOH");
                            tracker.events.Add(ev);
                        }
                    }
                }
            }
        }
    }

    private void OnGUI()
    {
        setStyles();
        GUILayout.Label("SceneViewer", inspector_title);

        if(GUILayout.Button("Distribute Events"))
        {
            assignEvents();
        }

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
                EditorGUI.BeginChangeCheck();
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



