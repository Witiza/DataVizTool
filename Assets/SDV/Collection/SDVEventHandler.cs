using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public enum SDVEventType
{
    POSITION,
    ROTATION,
    NO_TARGET,
    TARGETED,
    MULTI_TARGET
};

public enum SDVDataType
{
    NULL,
    BOOL,
    INT,
    FLOAT,
    STRING,
    VECTOR3
};

public class SDVEventHandler : MonoBehaviour
{
    public List<StandardEvent> events = new List<StandardEvent>();

    static SDVEventHandler handler;

    void OnEnable()
    {
        LoadEditorEvents();
        for (int i = 0; i < events.Count; i++)
        {
            events[i].ingame_events = new List<SDVBaseEvent>();
        }
    }

    private void OnDisable()
    {
        SaveEditorEvents();
        if (Application.isPlaying)
        {
            for (int i = 0; i < events.Count; i++)
            {
                SDVCSVhandling.SaveToCSV(events[i],SceneManager.GetActiveScene().name);
            }
        }
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
                    if (tmp.type == SDVEventType.POSITION|| tmp.type == SDVEventType.ROTATION|| tmp.type == SDVEventType.TARGETED)
                    {
                        Vector3? pos = null;
                        if(tmp.save_position)
                        {
                            pos = tmp.target.transform.position;
                        }
                        switch (tmp.type)
                        {
                            case SDVEventType.POSITION:
                                tmp.StoreEvent(tmp.target.transform.position ,tmp.target);
                                break;
                            case SDVEventType.ROTATION:
                                tmp.StoreEvent(tmp.target.transform.rotation.eulerAngles, pos,tmp.target);
                                break;
                            case SDVEventType.TARGETED:
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

    public  SDVEventType getType(string name)
    {
        SDVEventType ret = SDVEventType.NO_TARGET;
        foreach(StandardEvent ev in events)
        {
            if(ev.name == name)
            {
                ret = ev.type;
            }


        }
        Debug.Log(ret);
        return ret;
    }
    static public void StoreEventStatic(string event_name, bool data, Vector3? pos = null,GameObject target = null) 
    {
        //Weird https://es.stackoverflow.com/questions/172069/error-cs0201-only-assignment-call-increment-decrement-await-and-new-object

        if (handler == null)
        {
            if (handler = (SDVEventHandler)FindObjectOfType(typeof(SDVEventHandler)))
            {
                handler.StoreEvent(event_name, data, pos, target);
            }
            else
            {
                Debug.LogError("Trying to store an event without an EventHandler in the scene");
            }
        }
        else
        {
            handler.StoreEvent(event_name, data, pos, target);
        }
    }
    static public void StoreEventStatic(string event_name, int data, Vector3? pos = null, GameObject target = null)
    {
        if (handler == null)
        {
            if (handler = (SDVEventHandler)FindObjectOfType(typeof(SDVEventHandler)))
            {
                handler.StoreEvent(event_name, data, pos, target);
            }
            else
            {
                Debug.LogError("Trying to store an event without an EventHandler in the scene");
            }
        }
        else
        {
            handler.StoreEvent(event_name, data, pos, target);
        }
    }
    static public void StoreEventStatic(string event_name, float data, Vector3? pos = null, GameObject target = null)
    {
        if (handler == null)
        {
            if (handler = (SDVEventHandler)FindObjectOfType(typeof(SDVEventHandler)))
            {
                handler.StoreEvent(event_name, data, pos, target);
            }
            else
            {
                Debug.LogError("Trying to store an event without an EventHandler in the scene");
            }
        }
        else
        {
            handler.StoreEvent(event_name, data, pos, target);
        }
    }

    static public void StoreEventStatic(string event_name, string data, Vector3? pos = null, GameObject target = null)
    {
        if (handler == null)
        {
            if (handler = (SDVEventHandler)FindObjectOfType(typeof(SDVEventHandler)))
            {
                handler.StoreEvent(event_name, data, pos, target);
            }
            else
            {
                Debug.LogError("Trying to store an event without an EventHandler in the scene");
            }
        }
        else
        {
            handler.StoreEvent(event_name, data, pos, target);
        }
    }
    static public void StoreEventStatic(string event_name, Vector3 data, Vector3? pos = null, GameObject target = null)
    {
        if (handler == null)
        {
            if (handler = (SDVEventHandler)FindObjectOfType(typeof(SDVEventHandler)))
            {
                handler.StoreEvent(event_name, data, pos, target);
            }
            else
            {
                Debug.LogError("Trying to store an event without an EventHandler in the scene");
            }
        }
        else
        {
            handler.StoreEvent(event_name, data, pos, target);
        }
    }
    static public void StoreEventStatic(string event_name, Vector3? pos = null, GameObject target = null)
    {
        if (handler == null)
        {
            if (handler = (SDVEventHandler)FindObjectOfType(typeof(SDVEventHandler)))
            {
                handler.StoreEvent(event_name, pos, target);
            }
            else
            {
                Debug.LogError("Trying to store an event without an EventHandler in the scene");
            }
        }
        else
        {
            handler.StoreEvent(event_name, pos, target);
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

    SDVParentHelper generateActiveList()
    {
        SDVParentHelper ret = new SDVParentHelper();

        foreach(StandardEvent ev in events)
        {
            ret.list.Add(new SDVHelper(ev.name,ev.active));
        }
        return ret;
    }



    public void SaveEditorEvents()
    {
    
        SDVParentHelper actives = generateActiveList();
        string path = Application.persistentDataPath + "/EditorEvents/";
        Directory.CreateDirectory(path);

        string path_evs = path+"Events.dta";
        FileStream fs = new FileStream(path_evs, FileMode.Create);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(fs, events);
        fs.Close();

        string path_actives = path + SceneManager.GetActiveScene().name + ".aev";
        fs = new FileStream(path_actives, FileMode.Create);
        formatter = new BinaryFormatter();
        formatter.Serialize(fs, actives);
        fs.Close();
    }

    void applyActiveList()
    {
        SDVParentHelper ret = new SDVParentHelper();
        string path = Application.persistentDataPath + "/EditorEvents/" + SceneManager.GetActiveScene().name + ".aev";
        if (File.Exists(path))
        {
            Stream stream = File.OpenRead(path);
            if (stream.Length > 0)
            {
                BinaryFormatter formater = new BinaryFormatter();
                //var tst = formater.Deserialize(stream);
                ret = (SDVParentHelper)formater.Deserialize(stream);
            }
            stream.Close();
        }

        foreach (StandardEvent ev in events)
        {
            foreach (var act in ret.list)
            {
                ev.active = false;
                if (act.name == ev.name)
                {
                    Debug.Log("Name: " + act.name);
                    Debug.Log("Value: " + act.active);
                    ev.active = act.active;
                    break;
                }
            }
        }
    }

    public  void LoadEditorEvents()
    {
        Debug.Log("-------------------------LOADING EVENTS---------------------------");
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
        applyActiveList();
        for(int i = 0;i<events.Count;i++)
        {
            if(events[i].active && events[i].target_name != null&& events[i].target_name != "")
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
                    if (events[i].type != SDVEventType.POSITION)
                    {
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




[CustomEditor(typeof(SDVEventHandler))]
[CanEditMultipleObjects]
public class EventHandlerEditor : Editor
{
    SDVEventHandler handler;
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
            handler = (SDVEventHandler)target;
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
                    tmp.type = (SDVEventType)EditorGUILayout.EnumPopup("Event Type: ", tmp.type);
                    if(EditorGUI.EndChangeCheck())
                    {
                        switch(tmp.type)
                        {
                            case SDVEventType.POSITION:
                                tmp.data_type = SDVDataType.NULL;
                                tmp.save_position = true;
                                tmp.use_target = true;
                                tmp.use_frequency = true;
                                break;
                            case SDVEventType.ROTATION:
                                tmp.data_type = SDVDataType.VECTOR3;
                                tmp.save_position = true;
                                tmp.use_target = true;
                                tmp.use_frequency = true;
                                break;
                            case SDVEventType.NO_TARGET: 
                                tmp.use_target = false;
                                tmp.save_position = false;
                                tmp.use_frequency = false;
                                break;
                            case SDVEventType.MULTI_TARGET:
                                tmp.use_target = true;
                                tmp.save_position = false;
                                tmp.use_frequency = false;
                                break;
                            case SDVEventType.TARGETED:
                                tmp.use_frequency = true;
                                tmp.use_target = true;
                                break;
                        }
                    }
                    if (tmp.use_target&&tmp.type != SDVEventType.MULTI_TARGET)
                    {
                        EditorGUI.BeginChangeCheck();
                        tmp.target = (GameObject)EditorGUILayout.ObjectField("Target GameObject", tmp.target, typeof(GameObject), true);
                        if (EditorGUI.EndChangeCheck())
                        {
                            tmp.target_name = tmp.target.name;
                        }
                        if (tmp.target != null && tmp.type == SDVEventType.TARGETED)
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
                    if (tmp.type == SDVEventType.TARGETED||tmp.type == SDVEventType.MULTI_TARGET)
                    {
                        if (tmp.type == SDVEventType.TARGETED)
                        {
                            tmp.use_frequency = EditorGUILayout.Toggle("Use Frequency", tmp.use_frequency);
                        }
                        tmp.save_position = EditorGUILayout.Toggle("Save Position", tmp.save_position);
                    }
                  if(tmp.use_frequency)
                    {
                        tmp.interval = EditorGUILayout.FloatField("Event Frequency", tmp.interval);
                    }
                  if(tmp.type != SDVEventType.POSITION && tmp.type != SDVEventType.ROTATION )
                    tmp.data_type = (SDVDataType)EditorGUILayout.EnumPopup("Data Type: ", tmp.data_type);
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
    public bool active = false;
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
    public List<SDVBaseEvent> ingame_events;
    public SDVEventType type;
    public SDVDataType data_type;
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
        type =SDVEventType.NO_TARGET;
        data_type = SDVDataType.INT;
        ingame_events = new List<SDVBaseEvent>();
      //  scene = SceneManager.GetActiveScene().name;
    }

    public void StoreEvent(bool ev, Vector3? pos = null, GameObject target = null)
    {

        SDVBoolEvent tmp = new SDVBoolEvent(ev,name,playerID,sessionID,pos,target);
        ingame_events.Add(tmp);
    }
    public void StoreEvent(int ev, Vector3? pos = null, GameObject target = null)
    {
        SDVIntEvent tmp = new SDVIntEvent(ev, name, playerID, sessionID,pos, target);
        ingame_events.Add(tmp);
    }
    public void StoreEvent(float ev, Vector3? pos = null, GameObject target = null)
    {
        SDVFloatEvent tmp = new SDVFloatEvent(ev, name, playerID, sessionID,pos, target);
        ingame_events.Add(tmp);
    }
    public void StoreEvent(string ev, Vector3? pos = null, GameObject target = null)
    {
        SDVStringEvent tmp = new SDVStringEvent(ev, name, playerID, sessionID, pos, target);
        ingame_events.Add(tmp);
    }
    public void StoreEvent(Vector3 ev, Vector3? pos = null, GameObject target = null)
    {
        SDVVector3Event tmp = new SDVVector3Event(ev, name, playerID, sessionID, pos, target);

        ingame_events.Add(tmp);
    }
    public void StoreEvent(Vector3? pos=null, GameObject target = null)
    {
        SDVBaseEvent tmp = new SDVBaseEvent(name, playerID, sessionID, pos, target);
        ingame_events.Add(tmp);
    }
    void generateID()
    {
        eventID = (uint)UnityEngine.Random.Range(1, 999); //TEMPORARY
    }
}

[Serializable]
public class SDVHelper
{
    public SDVHelper(string _name, bool _active)
    {
        name = _name;
        active = _active;
    }

    public string name;
    public bool active;
}
[Serializable]
public class SDVParentHelper
{
    public List<SDVHelper> list = new List<SDVHelper>();
}
