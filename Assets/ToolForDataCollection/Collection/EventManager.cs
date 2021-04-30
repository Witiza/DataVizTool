using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EventManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class BaseEvent
{
    string name;
    int playerID;
    int sessionID;
    string timestamp;
    public BaseEvent(string _name,int player_id, int session_id)
    {
        name = _name;
        playerID = player_id;
        sessionID = session_id;
        timestamp = System.DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss.fff");
    }

    public virtual void saveToCSV(StreamWriter file)
    {
        file.Write(name + "," + playerID + "," + sessionID + "," + timestamp + ",");
    }
};

class BoolEvent : BaseEvent
{
    bool data;
    public BoolEvent(bool ev, string _name, int player_id, int session_id):base(_name,player_id,session_id)
    {
        data = ev;
    }
    public override void saveToCSV(StreamWriter file)
    {
        base.saveToCSV(file);
        file.WriteLine(data);
    }
};

class IntEvent : BaseEvent
{
    int data;
    public IntEvent(int ev, string _name, int player_id, int session_id) : base(_name, player_id, session_id)
    {
        data = ev;
    }
    public override void saveToCSV(StreamWriter file)
    {
        base.saveToCSV(file);
        file.WriteLine(data);
    }
};

class FloatEvent : BaseEvent
{
    float data;
    public FloatEvent(float ev, string _name, int player_id, int session_id ) : base(_name, player_id, session_id)
    {
        data = ev;
    }
    public override void saveToCSV(StreamWriter file)
    {
        base.saveToCSV(file);
        file.WriteLine(data);
    }
};
class Vector3Event :BaseEvent
{
    Vector3 data;
    public Vector3Event(Vector3 ev, string _name, int player_id, int session_id) : base(_name, player_id, session_id)
    {
        data = ev;
    }
    public override void saveToCSV(StreamWriter file)
    {
        base.saveToCSV(file);
        file.WriteLine(data.x+","+data.y+","+data.z);
        
    }
}
