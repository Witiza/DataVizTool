using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EventContainer
{
    public EventContainer(string _name)
    {
        name = _name;
        events = new List<BaseEvent>();
    }
    public bool in_use = true;
    public string name;
    public DataType type;
    public List<BaseEvent> events;
    public bool use_position;
    public bool use_target;
    public bool empty = false;
}
public class BaseEvent
{
    public string name;
    int playerID;
    int sessionID;
    string timestamp;
    bool use_pos=false; //corner cases amirigth
    public Vector3 position;
    public GameObject target_go;
    public string target_GUID = "";

    public BaseEvent(string _name,int player_id, int session_id, Vector3? pos, GameObject _target)
    {
        name = _name;
        playerID = player_id;
        sessionID = session_id;
        timestamp = System.DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss.fff");
        if (pos != null)
        {
            position = pos.Value;
            use_pos = true;
        }

        EventTracker tmp = _target.GetComponent<EventTracker>();
        Debug.Log(tmp.gameObject.name);
        if (tmp != null)
        {
            Debug.Log("tracker exists");
            target_GUID = tmp.getGUID();
            //Debug.Log("GUID: " + tmp.GUID);
        }
        else
        {
            Debug.LogWarning("Trying to add GameObject " + _target + " without and EventTracker component");
        }
    }

    public BaseEvent(string line, string _name, bool use_position, bool use_target)
    {
        name = _name;
        int start = 0;
        int end = line.IndexOf(',');
        playerID = int.Parse(line.Substring(start, end - start));

        start = end + 1;
        end = line.IndexOf(',', start);
        sessionID = int.Parse(line.Substring(start, end - start));

        start = end + 1;
        end = line.IndexOf(',', start);
        timestamp = line.Substring(start, end - start);

        if (use_position)
        {
            use_pos = true;
            start = end + 1;
            end = line.IndexOf(',', start);
            position.x = float.Parse(line.Substring(start, end - start));

            start = end + 1;
            end = line.IndexOf(',', start);
            position.y = float.Parse(line.Substring(start, end - start));

            start = end + 1;
            end = line.IndexOf(',', start);
            position.z = float.Parse(line.Substring(start, end - start));
        }
        if(use_target)
        {

            start = end + 1;
            end = line.IndexOf(',', start);
            target_GUID = line.Substring(start, end - start);
            target_go = CSVhandling.getGameObject(target_GUID);
        }
    }

    public virtual void saveToCSV(StreamWriter file)
    {
        file.Write(playerID + "," + sessionID + "," + timestamp + ",");
        if (use_pos)
        {
            file.Write(position.x+ "," + position.y + "," + position.z + ",");
        }
        Debug.Log(target_GUID);
        if(target_GUID != "")
        {
                file.Write(target_GUID + ",");
        }
    }
};

class BoolEvent : BaseEvent
{
    public bool data;
    public BoolEvent(bool ev, string _name, int player_id, int session_id, Vector3? pos, GameObject target) :base(_name,player_id,session_id,pos,target)
    {
        data = ev;
    }

    public BoolEvent(string line, string _name,bool use_position, bool use_target) :base(line,_name,  use_position, use_target)
    {
        data = bool.Parse(line.Substring(line.LastIndexOf(',') + 1));
    }
    public override void saveToCSV(StreamWriter file)
    {
        base.saveToCSV(file);
        file.WriteLine(data);
    }
};

class IntEvent : BaseEvent
{
    public int data;
    public IntEvent(int ev, string _name, int player_id, int session_id, Vector3? pos = null, GameObject target = null) : base(_name, player_id, session_id, pos, target)
    {
        data = ev;
    }
    public IntEvent(string line, string _name, bool use_position, bool use_target) : base(line, _name, use_position, use_target)
    {
        data = int.Parse(line.Substring(line.LastIndexOf(',') + 1));
    }
    public override void saveToCSV(StreamWriter file)
    {
        base.saveToCSV(file);
        file.WriteLine(data);
    }
};

class FloatEvent : BaseEvent
{
    public float data;
    public FloatEvent(float ev, string _name, int player_id, int session_id, Vector3? pos = null, GameObject target = null) : base(_name, player_id, session_id, pos,target)
    {
        data = ev;
    }
    public FloatEvent(string line, string _name, bool use_position, bool use_target) : base(line, _name, use_position, use_target)
    {
        data = float.Parse(line.Substring(line.LastIndexOf(',') + 1));
    }
    public override void saveToCSV(StreamWriter file)
    {
        base.saveToCSV(file);
        file.WriteLine(data);
    }
}

class CharEvent : BaseEvent
{
    public char data;
    public CharEvent(char ev, string _name, int player_id, int session_id, Vector3? pos = null, GameObject target = null) : base(_name, player_id, session_id, pos, target)
    {
        data = ev;
    }
    public CharEvent(string line, string _name, bool use_position, bool use_target) : base(line, _name, use_position, use_target)
    {
        data = char.Parse(line.Substring(line.LastIndexOf(',') + 1));
    }
    public override void saveToCSV(StreamWriter file)
    {
        base.saveToCSV(file);
        file.WriteLine(data);
    }
}
class StringEvent : BaseEvent
{
    
    public string data;
    public StringEvent(string ev, string _name, int player_id, int session_id, Vector3? pos = null, GameObject target = null) : base(_name, player_id, session_id, pos,target)
    {
        data = ev;
    }
    public StringEvent(string line, string _name, bool use_position, bool use_target) : base(line, _name, use_position, use_target)
    {
        data = line.Substring(line.LastIndexOf(',') + 1);
    }
    public override void saveToCSV(StreamWriter file)
    {
        base.saveToCSV(file);
        file.WriteLine(data);
    }
};
public class Vector3Event : BaseEvent
{
    public Vector3 data;
    public Vector3Event(Vector3 ev, string _name, int player_id, int session_id, Vector3? pos = null, GameObject target = null) : base(_name, player_id, session_id, pos, target)
    {
        data = ev;
    }

    public Vector3Event(string line, string _name, bool use_position, bool use_target) : base(line, _name, use_position, use_target)
    {

        data = new Vector3();
        int start = line.LastIndexOf(',') + 1;
        int end = 0;
        data.z = float.Parse(line.Substring(start));

        end = start;
        start = line.LastIndexOf(',', end - 2) + 1;
        data.y = float.Parse(line.Substring(start, end - start - 1));

        end = start;
        start = line.LastIndexOf(',', end - 2) + 1;
        data.x = float.Parse(line.Substring(start, end - start - 1));


    }
    public override void saveToCSV(StreamWriter file)
    {
        base.saveToCSV(file);
        file.WriteLine(data.x + "," + data.y + "," + data.z);

    }
}
