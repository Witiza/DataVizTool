using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        timestamp = System.DateTime.UtcNow.Millisecond.ToString();
    }
};

class BoolEvent : BaseEvent
{
    bool boolean;
    public BoolEvent(bool ev, string _name, int player_id, int session_id):base(_name,player_id,session_id)
    {
        boolean = ev;
    }
};

class IntEvent : BaseEvent
{
    int integer;
    public IntEvent(int ev, string _name, int player_id, int session_id) : base(_name, player_id, session_id)
    {
        integer = ev;
    }
};

class FloatEvent : BaseEvent
{
    float floating;
    public FloatEvent(float ev, string _name, int player_id, int session_id ) : base(_name, player_id, session_id)
    {
        floating = ev;
    }
};
class Vector3Event :BaseEvent
{
    Vector3 vector;
    public Vector3Event(Vector3 ev, string _name, int player_id, int session_id) : base(_name, player_id, session_id)
    {
        vector = ev;
    }
}
