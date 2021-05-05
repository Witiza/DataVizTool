﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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

    public BaseEvent(string line)
    {
        int start = 0;
        int end = line.IndexOf(',');
        name = line.Substring(start, end - start);

        start = end + 1;
        end = line.IndexOf(',', start);
        playerID = int.Parse(line.Substring(start, end - start));

        start = end + 1;
        end = line.IndexOf(',', start);
        sessionID = int.Parse(line.Substring(start, end - start));

        start = end + 1;
        end = line.IndexOf(',', start);
        timestamp = line.Substring(start, end - start);
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

    public BoolEvent(string line) : base(line)
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
    int data;
    public IntEvent(int ev, string _name, int player_id, int session_id) : base(_name, player_id, session_id)
    {
        data = ev;
    }
    public IntEvent(string line):base(line)
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
    float data;
    public FloatEvent(float ev, string _name, int player_id, int session_id ) : base(_name, player_id, session_id)
    {
        data = ev;
    }
    public FloatEvent(string line):base(line)
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
    char data;
    public CharEvent(char ev, string _name, int player_id, int session_id) : base(_name, player_id, session_id)
    {
        data = ev;
    }
    public CharEvent(string line) : base(line)
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
    
    string data;
    public StringEvent(string ev, string _name, int player_id, int session_id) : base(_name, player_id, session_id)
    {
        data = ev;
    }
    public StringEvent(string line):base(line)
    {
        data = line.Substring(line.LastIndexOf(',') + 1);
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
    public Vector3Event(string line):base(line)
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
        data.z = float.Parse(line.Substring(start, end - start - 1));


    }
    public override void saveToCSV(StreamWriter file)
    {
        base.saveToCSV(file);
        file.WriteLine(data.x+","+data.y+","+data.z);
        
    }
}
