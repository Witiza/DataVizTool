using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public enum DataEventType
{
    DEFAULT,
    GLOBAL,
    POSITION,
    LEVEL_START,
    LEVEL_SUCCESS,
    LEVEL_FAILURE,
    CUSTOM
};

public enum DataType
{
    NULL,
    BOOL,
    INT,
    FLOAT,
    CHAR,
    STRING,
    VECTOR3
};

public class EventHandler : MonoBehaviour
{
    public List<StandardEvent> events = new List<StandardEvent>();
    List<List<BaseEvent>> ingame_events = new List<List<BaseEvent>>();

    void OnEnable()
    {
        LoadEditorEvents();
    }

    private void OnDisable()
    {
        SaveEditorEvents();
    }
    private void Awake()
    {
        LoadEditorEvents();
        for (int i = 0; i < events.Count; i++)
        {
            events[i].ingame_events = new List<BaseEvent>();
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0;i<events.Count;i++)
        {
            StandardEvent tmp = events[i];
            if(tmp.use_frequency)
            {
                tmp.current_interval += Time.deltaTime;
                if(tmp.current_interval >= tmp.interval )
                {

                }
            }
        }
    }

    void IntervalEvents()
    {

    }

    public void AddEvent()
    {
        StandardEvent tmp = new StandardEvent();
        events.Add(tmp);
    }

    public void SaveEditorEvents()
    {
        string path = Application.persistentDataPath + "/EditorEvents/";
        Directory.CreateDirectory(path);
        path += "Events.dta";
        Debug.Log(path);
        FileStream fs = new FileStream(path, FileMode.Create);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(fs, events);
    }

   public  void LoadEditorEvents()
    {
        string path = Application.persistentDataPath + "/EditorEvents/Events.dta";
        if (File.Exists(path))
        {
            Stream stream = File.OpenRead(path);
            if (stream.Length > 0)
            {
                BinaryFormatter formater = new BinaryFormatter();
                events = (List<StandardEvent>)formater.Deserialize(stream);
            }
            stream.Close();
        }
    }
}




[CustomEditor(typeof(EventHandler))]
[CanEditMultipleObjects]
public class EventHandlerEditor : Editor
{
    EventHandler handler;
    List<bool> foldouts = new List<bool>();
    bool loaded_events = false;
   

    public override void OnInspectorGUI()
    {
        handler = (EventHandler)target;
        if (!loaded_events)
        {
            handler.LoadEditorEvents();
            loaded_events = true;
        }

        //DrawDefaultInspector();


        
        EditorGUILayout.HelpBox("This is a help box", MessageType.Info);
        if(GUILayout.Button("Add Event"))
        {
            handler.AddEvent();
            bool tmp = false;
            foldouts.Add(tmp);
        }
        if(foldouts.Count!=handler.events.Count)
        {
            GenerateFoldouts();
        }
        for(int i = 0;i < handler.events.Count; i++)
        {
            //StandardEvent tmp = events.GetArrayElementAtIndex(i).objectReferenceValue as StandardEvent;
            StandardEvent tmp = handler.events[i];
            foldouts[i] = EditorGUILayout.BeginFoldoutHeaderGroup(foldouts[i], tmp.name);
            if (foldouts[i])
            {
                tmp.name = EditorGUILayout.TextField("Event Name: ", tmp.name);
                tmp.type = (DataEventType)EditorGUILayout.EnumPopup("Event Type: ", tmp.type);
                if(tmp.use_frequency = EditorGUILayout.Toggle("Use Frequency", tmp.use_frequency))
                {
                    tmp.interval = EditorGUILayout.FloatField("Event Frequency", tmp.interval);
                }
                switch (tmp.type)
                {
                    case DataEventType.POSITION:
                        tmp.target_name = EditorGUILayout.TextField("Name of the target GameObject: ", tmp.target_name);
                        break;
                    case DataEventType.CUSTOM:
                        tmp.data_type = (DataType)EditorGUILayout.EnumPopup("Data Type: ", tmp.data_type);
                        break;
                };
                if (GUILayout.Button("Delete Event"))
                {
                    handler.events.RemoveAt(i);
                    foldouts.RemoveAt(i);
                }
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

    void GenerateFoldouts()
    {
        foldouts.Clear();
        for(int i=0;i<handler.events.Count;i++)
        {
            foldouts.Add(false);
        }
    }

    
}

[System.Serializable]
public class StandardEvent
{
    public float interval = 0;
    public float current_interval = 0;
    uint eventID = 0;
    public string name;
    string scene;
    public string target_name;
    [System.NonSerialized]
    Vector3 position;
    [System.NonSerialized]
    public List<BaseEvent> ingame_events;
    public DataEventType type;
    public DataType data_type;
    public bool use_frequency;

    [System.NonSerialized]
    float event_float;
    [System.NonSerialized]
    bool event_bool;
    [System.NonSerialized]
    string event_string;
    public StandardEvent()
    {
        name = "EVENTNAME";
       // generateID();
        type =DataEventType.DEFAULT;
        data_type = DataType.NULL;
      //  scene = SceneManager.GetActiveScene().name;
    }

    public void StoreEvent()
    {
       //Need different types of variables
    }

    void generateID()
    {
        eventID = (uint)Random.Range(1, 999); //TEMPORARY
    }
}
