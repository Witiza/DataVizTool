using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public enum DataEventType
{
    DEFAULT,
    GLOBAL,
    TARGETED,
    POSITION,
    LEVEL_START,
    LEVEL_SUCCESS,
    LEVEL_FAILURE
};
public class EventHandler : MonoBehaviour
{
    public List<StandardEvent> events = new List<StandardEvent>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddEvent()
    {
        StandardEvent tmp = new StandardEvent();
        events.Add(tmp);
    }
}




[CustomEditor(typeof(EventHandler))]
[CanEditMultipleObjects]
public class EventHandlerEditor : Editor
{
    SerializedProperty EventHandler;
    List<bool> foldouts = new List<bool>();
    public Object obj;


    void OnEnable()
    {

    }

    public override void OnInspectorGUI()
    {
        EventHandler handler = (EventHandler)target;
        DrawDefaultInspector();


        
        EditorGUILayout.HelpBox("This is a help box", MessageType.Info);
        if(GUILayout.Button("Add Event"))
        {
            handler.AddEvent();
            bool tmp = false;
            foldouts.Add(tmp);
        }

        for(int i = 0;i<handler.events.Count;i++)
        {
            StandardEvent tmp = handler.events[i];
            foldouts[i] = EditorGUILayout.BeginFoldoutHeaderGroup(foldouts[i], tmp.name);
            if (foldouts[i])
            {
                tmp.name = EditorGUILayout.TextField("Event Name: ", tmp.name);
                tmp.type = (DataEventType)EditorGUILayout.EnumPopup("Event Type: ", tmp.type);
                tmp.interval = EditorGUILayout.FloatField("Event Frequency", tmp.interval);
                switch (tmp.type)
                {
                    case DataEventType.TARGETED:
                        tmp.target = (GameObject)EditorGUILayout.ObjectField("Target: ", tmp.target,typeof(GameObject),true);
                        break;
                };
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

           
        }

        if (GUILayout.Button("TEST"))
        {
            StreamReader text = new StreamReader("Assets/Resources/SampleEvent.txt");


            string sample = text.ReadToEnd();
            File.WriteAllText("Assets/ToolForDataCollection/Utilities/SampleEvent.cs", sample);
            AssetDatabase.Refresh();
            Debug.Log(sample);
            //path += "/Utilities/";
            //Debug.Log(path);
        }
    }
}


public class StandardEvent
{
    public float interval = 0;
    uint eventID = 0;
    public string name;
    public GameObject target;
    Vector3 position;
    public DataEventType type;
    bool use_frequency;

    float event_float;
    bool event_bool;
    string event_string;
    public StandardEvent()
    {
        name = "EVENTNAME";
        generateID();
        type =DataEventType.DEFAULT;
    }

    void generateID()
    {
        eventID = (uint)Random.Range(1, 999); //TEMPORARY
    }
}
