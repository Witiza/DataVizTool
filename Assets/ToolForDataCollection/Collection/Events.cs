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
    public Vector3 position;
    GameObject target;

    public BaseEvent(string _name,int player_id, int session_id, Vector3? pos = null, GameObject target = null)
    {
        name = _name;
        playerID = player_id;
        sessionID = session_id;
        timestamp = System.DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss.fff");
        if(pos != null)
        {
            position = pos.GetValueOrDefault();
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
            string target = line.Substring(start, end - start);
        }
    }

    public virtual void saveToCSV(StreamWriter file)
    {
        file.Write(playerID + "," + sessionID + "," + timestamp + ","+position.x+","+position.y+","+position.z+","+GUID); //needs to have a gameobject to guid funciton (using the event tracker duuh)
    }
};

class BoolEvent : BaseEvent
{
    public bool data;
    public BoolEvent(bool ev, string _name, int player_id, int session_id, Vector3? pos = null, GameObject target = null) :base(_name,player_id,session_id,pos,target)
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
