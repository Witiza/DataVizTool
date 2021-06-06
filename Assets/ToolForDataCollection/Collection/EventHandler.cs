using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public enum DataEventType
{
    POSITION,
    ROTATION,
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


    void OnEnable()
    {
        LoadEditorEvents();
        for (int i = 0; i < events.Count; i++)
        {
            events[i].ingame_events = new List<BaseEvent>();
        }
    }

    private void OnDisable()
    {
        SaveEditorEvents();
        if (Application.isPlaying)
        {
            for (int i = 0; i < events.Count; i++)
            {
                CSVhandling.SaveToCSV(events[i],SceneManager.GetActiveScene().name);
            }
        }
    }
    private void Awake()
    {
        //HELLO?

       
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0;i<events.Count;i++)
        {
            StandardEvent tmp = events[i];
            if(tmp.use_frequency)
            {
                tmp.current_interval += Time.deltaTime;
                if(tmp.current_interval >= tmp.interval )
                {
                    if (tmp.target || tmp.use_target)
                    {
                        Vector3? pos = null;
                        if(tmp.save_position)
                        {
                            pos = tmp.target.transform.position;
                        }
                        switch (tmp.type)
                        {
                            case DataEventType.POSITION:
                                tmp.StoreEvent(tmp.target.transform.position ,null);
                                break;
                            case DataEventType.ROTATION:
                                tmp.StoreEvent(tmp.target.transform.rotation.eulerAngles, pos);
                                break;
                            case DataEventType.CUSTOM:
                                tmp.s_obj.Update();
                                switch(tmp.s_property.type)
                                {
                                    case "int":
                                        tmp.StoreEvent(tmp.s_property.intValue,pos);
                                        break;
                                    case "float":
                                        tmp.StoreEvent(tmp.s_property.floatValue,pos);
                                        break;
                                    case "char":
                                        tmp.StoreEvent((char)tmp.s_property.intValue,pos);
                                        break;
                                    case "string":
                                        tmp.StoreEvent(tmp.s_property.floatValue,pos);
                                        break;
                                    case "bool":
                                        tmp.StoreEvent(tmp.s_property.floatValue,pos);
                                        break;
                                    case "Vector3":
                                        tmp.StoreEvent(tmp.s_property.vector3Value,pos);
                                        break;

                                }

                                break;
                        }
                    }
                    tmp.current_interval = 0;
                }
            }
        }
    }

    static public void StoreEventStatic(string event_name, bool data, Vector3? pos = null,GameObject target = null) 
    {
        //Weird https://es.stackoverflow.com/questions/172069/error-cs0201-only-assignment-call-increment-decrement-await-and-new-object

        EventHandler tmp;
        if (tmp=  (EventHandler)FindObjectOfType(typeof(EventHandler)))
        {
               tmp.StoreEvent(event_name, data,pos,target);
        }
        else
        {
            Debug.LogError("Trying to store an event without an EventHandler in the scene");
        }
    }
    static public void StoreEventStatic(string event_name, int data, Vector3? pos = null, GameObject target = null)
    {
        //Weird https://es.stackoverflow.com/questions/172069/error-cs0201-only-assignment-call-increment-decrement-await-and-new-object
        EventHandler tmp;
        if (tmp = (EventHandler)FindObjectOfType(typeof(EventHandler)))
        {
           tmp.StoreEvent(event_name, data, pos,target);  
        }
        else
        {
            Debug.LogError("Trying to store an event without an EventHandler in the scene");
        }
    }
    static public void StoreEventStatic(string event_name, float data, Vector3? pos = null, GameObject target = null)
    {
        //Weird https://es.stackoverflow.com/questions/172069/error-cs0201-only-assignment-call-increment-decrement-await-and-new-object
        EventHandler tmp;
        if (tmp = (EventHandler)FindObjectOfType(typeof(EventHandler)))
        {

            tmp.StoreEvent(event_name, data, pos,target);
            
        }
        else
        {
            Debug.LogError("Trying to store an event without an EventHandler in the scene");
        }
    }
    static public void StoreEventStatic(string event_name, Vector3 data, Vector3? pos = null, GameObject target = null)
    {
        //Weird https://es.stackoverflow.com/questions/172069/error-cs0201-only-assignment-call-increment-decrement-await-and-new-object
        EventHandler tmp;
        if (tmp = (EventHandler)FindObjectOfType(typeof(EventHandler)))
        {

            tmp.StoreEvent(event_name, data, pos,target);
            
        }
        else
        {
            Debug.LogError("Trying to store an event without an EventHandler in the scene");
        }
    }

    public void StoreEvent(string event_name, bool data, Vector3? pos = null, GameObject target = null)
    {
        for(int i = 0;i<events.Count;i++)
        {
            if(events[i].name == event_name)
            {
                events[i].StoreEvent(data, pos, target);
            }
        }
    }
    public void StoreEvent(string event_name, int data, Vector3? pos = null, GameObject target = null)
    {
        for (int i = 0; i < events.Count; i++)
        {
            if (events[i].name == event_name)
            {
                events[i].StoreEvent(data,pos, target);
            }
        }
    }
    public void StoreEvent(string event_name, float data, Vector3? pos = null, GameObject target = null)
    {
        for (int i = 0; i < events.Count; i++)
        {
            if (events[i].name == event_name)
            {
                events[i].StoreEvent(data,pos,target);
            }
        }
    }
    public void StoreEvent(string event_name, Vector3 data, Vector3? pos = null, GameObject target = null)
    {
        for (int i = 0; i < events.Count; i++)
        {
            if (events[i].name == event_name)
            {
                events[i].StoreEvent(data,pos,target);
            }
        }
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
        FileStream fs = new FileStream(path, FileMode.Create);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(fs, events);
        fs.Close();
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
        for(int i = 0;i<events.Count;i++)
        {
            if(events[i].target_name != null&& events[i].target_name != "")
            {

                if(!(events[i].target = GameObject.Find(events[i].target_name)))
                {
                    //events[i].target = CSVhandling.getGameObject(events[i].target_GUID);
                    if (events[i].target == null)
                    {
                        Debug.LogError("Couldnt find target GameObject for event: " + events[i].name);
                    }
                  
                }
                else
                {
                    if (events[i].type != DataEventType.POSITION)
                    {
                        Debug.Log("start");
                        var component = events[i].target.GetComponent(events[i].script_name);
                        if (component)
                        {
                            events[i].s_obj = new SerializedObject(component);
                            if (events[i].s_obj != null)
                            {
                                events[i].s_property = events[i].s_obj.FindProperty(events[i].variable_name);
                            }

                        }
                    }
                }

            }
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
        if (!Application.isPlaying)
        {
            handler = (EventHandler)target;
            if (!loaded_events)
            {
                handler.LoadEditorEvents();
                loaded_events = true;
            }

            //DrawDefaultInspector();



            EditorGUILayout.HelpBox("This is a help box", MessageType.Info);
            if (GUILayout.Button("Add Event"))
            {
                handler.AddEvent();
                bool tmp = false;
                foldouts.Add(tmp);
            }
            if (foldouts.Count != handler.events.Count)
            {
                GenerateFoldouts();
            }
            for (int i = 0; i < handler.events.Count; i++)
            {
                //StandardEvent tmp = events.GetArrayElementAtIndex(i).objectReferenceValue as StandardEvent;
                StandardEvent tmp = handler.events[i];
                foldouts[i] = EditorGUILayout.BeginFoldoutHeaderGroup(foldouts[i], tmp.name);
                if (foldouts[i])
                {
                    tmp.name = EditorGUILayout.TextField("Event Name: ", tmp.name);
                    GUI.changed = false;
                    tmp.type = (DataEventType)EditorGUILayout.EnumPopup("Event Type: ", tmp.type);
                    if(GUI.changed)
                    {
                        switch(tmp.type)
                        {
                            
                            case DataEventType.LEVEL_START:
                                tmp.data_type = DataType.BOOL;
                                break;
                            case DataEventType.LEVEL_FAILURE:
                                tmp.data_type = DataType.BOOL;
                                break;
                            case DataEventType.LEVEL_SUCCESS:
                                tmp.data_type = DataType.BOOL;
                                break;
                            case DataEventType.POSITION:
                                tmp.data_type = DataType.NULL;
                                tmp.save_position = true;
                                break;
                            case DataEventType.ROTATION:
                                tmp.data_type = DataType.VECTOR3;
                                tmp.save_position = true;
                                break;
                        }
                    }
                    EditorGUI.BeginChangeCheck();
                    tmp.target = (GameObject)EditorGUILayout.ObjectField("Target GameObject", tmp.target, typeof(GameObject), true);
                    if (EditorGUI.EndChangeCheck())
                    {
                        tmp.target_name = tmp.target.name;
                    }
                    if(tmp.target != null)
                    {
                        EditorGUI.BeginChangeCheck();
                        tmp.script_name = EditorGUILayout.TextField("Script containing the variable", tmp.script_name);
                        if (EditorGUI.EndChangeCheck())
                        {
                            var component = tmp.target.GetComponent(tmp.script_name);
                            if (component)
                            {
                                tmp.s_obj = new SerializedObject(component);
                                Debug.Log("Obj: " + tmp.s_obj);

                            }
                        }
                        if(tmp.s_obj == null)
                        {
                            EditorGUILayout.LabelField("Could not find script " + tmp.script_name + " in the GO: " + tmp.target.name);
                        }
                        EditorGUI.BeginChangeCheck();
                        tmp.variable_name = EditorGUILayout.TextField("Variable to track", tmp.variable_name);
                        if (EditorGUI.EndChangeCheck())
                        {
                            tmp.s_property = tmp.s_obj.FindProperty(tmp.variable_name);
                            //Debug.Log("Property : " + tmp.s_property.type);
                        }
                        if (tmp.s_property == null)
                        {

                            EditorGUILayout.LabelField("Could not find variable " + tmp.variable_name + " in the script. Remember to make it serializable");
                        }

                    }
                    tmp.use_multiple_targets = EditorGUILayout.Toggle("Each event will have an individual target", tmp.use_multiple_targets);
                    if (tmp.type != DataEventType.POSITION && tmp.type != DataEventType.ROTATION)
                    {
                        tmp.use_frequency = EditorGUILayout.Toggle("Use Frequency", tmp.use_frequency);
                        tmp.save_position = EditorGUILayout.Toggle("Save Position", tmp.save_position);
                    }
                  if(tmp.use_frequency)
                    {
                        tmp.interval = EditorGUILayout.FloatField("Event Frequency", tmp.interval);
                    }
                    switch (tmp.type)
                    {
                        case DataEventType.POSITION:

                            if (tmp.target = (GameObject)EditorGUILayout.ObjectField("Target GameObject", tmp.target, typeof(GameObject), true))
                            {
                                tmp.target_name = tmp.target.name;
                            }
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
                handler.SaveEditorEvents();
            }
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
    public float interval = 1;
    public float current_interval = 0;
    uint eventID = 0;
    public string name;
    string scene;
    public string target_name="";
    public string target_GUID = "";
    [System.NonSerialized]
    public GameObject target = null;
    [System.NonSerialized]
    Vector3 position;
   [System.NonSerialized]
    public List<BaseEvent> ingame_events;
    public DataEventType type;
    public DataType data_type;
    public bool use_frequency = false;
    public bool save_position = false;
    public bool use_multiple_targets = false;
    public bool use_target = false;
    int playerID;
    int sessionID;
    [System.NonSerialized]
    public string[] options;
    [System.NonSerialized]
    public SerializedObject s_obj = null;
    [System.NonSerialized]
    public SerializedProperty s_property = null;
    public string script_name = "script name";
    public string variable_name = "variable name";





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
        type =DataEventType.CUSTOM;
        data_type = DataType.INT;
        ingame_events = new List<BaseEvent>();
      //  scene = SceneManager.GetActiveScene().name;
    }

    public void StoreEvent(bool ev, Vector3? pos = null, GameObject target = null)
    {

        BoolEvent tmp = new BoolEvent(ev,name,playerID,sessionID,pos,target);
        ingame_events.Add(tmp);
    }
    public void StoreEvent(int ev, Vector3? pos = null, GameObject target = null)
    {
        IntEvent tmp = new IntEvent(ev, name, playerID, sessionID,pos, target);
        ingame_events.Add(tmp);
    }
    public void StoreEvent(float ev, Vector3? pos = null, GameObject target = null)
    {
        FloatEvent tmp = new FloatEvent(ev, name, playerID, sessionID,pos, target);
        ingame_events.Add(tmp);
    }
    public void StoreEvent(string ev, Vector3? pos = null, GameObject target = null)
    {
        StringEvent tmp = new StringEvent(ev, name, playerID, sessionID, pos, target);
        ingame_events.Add(tmp);
    }
    public void StoreEvent(char ev, Vector3? pos = null, GameObject target = null)
    {
        CharEvent tmp = new CharEvent(ev, name, playerID, sessionID, pos, target);
        ingame_events.Add(tmp);
    }
    public void StoreEvent(Vector3 ev, Vector3? pos = null, GameObject target = null)
    {
        Vector3Event tmp = new Vector3Event(ev, name, playerID, sessionID, pos, target);

        ingame_events.Add(tmp);
    }
    public void StoreEvent(Vector3 pos, GameObject target = null)
    {
        BaseEvent tmp = new BaseEvent(name, playerID, sessionID, pos, target);
        ingame_events.Add(tmp);
    }
    void generateID()
    {
        eventID = (uint)Random.Range(1, 999); //TEMPORARY
    }
}
