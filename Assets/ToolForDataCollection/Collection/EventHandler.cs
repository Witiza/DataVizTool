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
    NO_TARGET,
    TARGETED,
    MULTI_TARGET
};

public enum DataType
{
    NULL,
    BOOL,
    INT,
    FLOAT,
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
            if(tmp.use_frequency&&tmp.active)
            {
                tmp.current_interval += Time.deltaTime;
                if(tmp.current_interval >= tmp.interval )
                {
                    if (tmp.type == DataEventType.POSITION|| tmp.type == DataEventType.ROTATION|| tmp.type == DataEventType.TARGETED)
                    {
                        Vector3? pos = null;
                        if(tmp.save_position)
                        {
                            pos = tmp.target.transform.position;
                        }
                        switch (tmp.type)
                        {
                            case DataEventType.POSITION:
                                tmp.StoreEvent(tmp.target.transform.position ,tmp.target);
                                break;
                            case DataEventType.ROTATION:
                                tmp.StoreEvent(tmp.target.transform.rotation.eulerAngles, pos,tmp.target);
                                break;
                            case DataEventType.TARGETED:
                                tmp.s_obj.Update();
                                switch(tmp.s_property.type)
                                {
                                    case "int":
                                        tmp.StoreEvent(tmp.s_property.intValue,pos,tmp.target);
                                        break;
                                    case "float":
                                        tmp.StoreEvent(tmp.s_property.floatValue,pos, tmp.target);
                                        break;
                                    case "string":
                                        tmp.StoreEvent(tmp.s_property.floatValue,pos, tmp.target);
                                        break;
                                    case "bool":
                                        tmp.StoreEvent(tmp.s_property.floatValue,pos, tmp.target);
                                        break;
                                    case "Vector3":
                                        tmp.StoreEvent(tmp.s_property.vector3Value,pos, tmp.target);
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

    static public void StoreEventStatic(string event_name, string data, Vector3? pos = null, GameObject target = null)
    {
        //Weird https://es.stackoverflow.com/questions/172069/error-cs0201-only-assignment-call-increment-decrement-await-and-new-object
        EventHandler tmp;
        if (tmp = (EventHandler)FindObjectOfType(typeof(EventHandler)))
        {

            tmp.StoreEvent(event_name, data, pos, target);

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
    static public void StoreEventStatic(string event_name, Vector3? pos = null, GameObject target = null)
    {
        //Weird https://es.stackoverflow.com/questions/172069/error-cs0201-only-assignment-call-increment-decrement-await-and-new-object
        EventHandler tmp;
        if (tmp = (EventHandler)FindObjectOfType(typeof(EventHandler)))
        {

            tmp.StoreEvent(event_name, pos, target);

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
    public void StoreEvent(string event_name, string data, Vector3? pos = null, GameObject target = null)
    {
        for (int i = 0; i < events.Count; i++)
        {
            if (events[i].name == event_name)
            {
                events[i].StoreEvent(data, pos, target);
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

    public void StoreEvent(string event_name, Vector3? pos = null, GameObject target = null)
    {
        for (int i = 0; i < events.Count; i++)
        {
            if (events[i].name == event_name)
            {
                events[i].StoreEvent(pos, target);
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

    public GUIStyle inspector_title = new GUIStyle();
    public GUIStyle text = new GUIStyle();
    public void setStyles()
    {
        inspector_title.fontSize = 20;
        inspector_title.normal.textColor = Color.white;
        inspector_title.alignment = TextAnchor.MiddleCenter;
    }

    public override void OnInspectorGUI()
    {
        setStyles();
        if (!Application.isPlaying)
        {
            handler = (EventHandler)target;
            if (!loaded_events)
            {
                handler.LoadEditorEvents();
                loaded_events = true;
            }

            EditorGUILayout.LabelField("Event Handler", inspector_title);

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
                StandardEvent tmp = handler.events[i];
                foldouts[i] = EditorGUILayout.BeginFoldoutHeaderGroup(foldouts[i], tmp.name);
                if (foldouts[i])
                {
                    tmp.name = EditorGUILayout.TextField("Event Name: ", tmp.name);
                    tmp.active = EditorGUILayout.Toggle("Event is Active", tmp.active);
                    if(!tmp.active)
                    {
                        GUI.enabled = false;
                    }
                    EditorGUI.BeginChangeCheck();
                    tmp.type = (DataEventType)EditorGUILayout.EnumPopup("Event Type: ", tmp.type);
                    if(EditorGUI.EndChangeCheck())
                    {
                        switch(tmp.type)
                        {
                            case DataEventType.POSITION:
                                tmp.data_type = DataType.NULL;
                                tmp.save_position = true;
                                tmp.use_target = true;
                                tmp.use_frequency = true;
                                break;
                            case DataEventType.ROTATION:
                                tmp.data_type = DataType.VECTOR3;
                                tmp.save_position = true;
                                tmp.use_target = true;
                                tmp.use_frequency = true;
                                break;
                            case DataEventType.NO_TARGET: 
                                tmp.use_target = false;
                                tmp.save_position = false;
                                tmp.use_frequency = false;
                                break;
                            case DataEventType.MULTI_TARGET:
                                tmp.use_target = false;
                                tmp.save_position = false;
                                tmp.use_frequency = false;
                                break;
                            case DataEventType.TARGETED:
                                tmp.use_frequency = true;
                                tmp.use_target = true;
                                break;
                        }
                    }
                    if (tmp.use_target)
                    {
                        EditorGUI.BeginChangeCheck();
                        tmp.target = (GameObject)EditorGUILayout.ObjectField("Target GameObject", tmp.target, typeof(GameObject), true);
                        if (EditorGUI.EndChangeCheck())
                        {
                            tmp.target_name = tmp.target.name;
                        }
                        if (tmp.target != null && tmp.type == DataEventType.TARGETED)
                        {
                            EditorGUI.BeginChangeCheck();
                            tmp.script_name = EditorGUILayout.TextField("Script containing the variable", tmp.script_name);
                            if (EditorGUI.EndChangeCheck())
                            {
                                tmp.s_obj = null;
                                var component = tmp.target.GetComponent(tmp.script_name);
                                if (component != null)
                                {
                                    tmp.s_obj = new SerializedObject(component);
                                }
                            }

                            if (tmp.s_obj == null)
                            {
                                EditorGUILayout.LabelField("Could not find script " + tmp.script_name + " in the GO: " + tmp.target.name);
                            }
                            EditorGUI.BeginChangeCheck();
                            tmp.variable_name = EditorGUILayout.TextField("Variable to track", tmp.variable_name);
                            if (EditorGUI.EndChangeCheck())
                            {
                                tmp.s_property = tmp.s_obj.FindProperty(tmp.variable_name);
                            }
                            if (tmp.s_property == null)
                            {

                                EditorGUILayout.LabelField("Could not find variable " + tmp.variable_name + " in the script. Remember to make it serializable");
                            }

                        }
                    }
                    if (tmp.type == DataEventType.TARGETED)
                    {
                        tmp.use_frequency = EditorGUILayout.Toggle("Use Frequency", tmp.use_frequency);
                        tmp.save_position = EditorGUILayout.Toggle("Save Position", tmp.save_position);
                    }
                  if(tmp.use_frequency)
                    {
                        tmp.interval = EditorGUILayout.FloatField("Event Frequency", tmp.interval);
                    }
                  
                    tmp.data_type = (DataType)EditorGUILayout.EnumPopup("Data Type: ", tmp.data_type);
                    if (GUILayout.Button("Delete Event"))
                    {
                        handler.events.RemoveAt(i);
                        foldouts.RemoveAt(i);
                    }
                }
                if (!tmp.active)
                {
                    GUI.enabled = true;
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
    public bool active = true;
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
        type =DataEventType.NO_TARGET;
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
    public void StoreEvent(Vector3 ev, Vector3? pos = null, GameObject target = null)
    {
        Vector3Event tmp = new Vector3Event(ev, name, playerID, sessionID, pos, target);

        ingame_events.Add(tmp);
    }
    public void StoreEvent(Vector3? pos=null, GameObject target = null)
    {
        BaseEvent tmp = new BaseEvent(name, playerID, sessionID, pos, target);
        ingame_events.Add(tmp);
    }
    void generateID()
    {
        eventID = (uint)Random.Range(1, 999); //TEMPORARY
    }
}
