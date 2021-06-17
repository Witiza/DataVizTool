using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SDVEventContainer
{
    public SDVEventContainer(string _name)
    {
        name = _name;
        events = new List<SDVBaseEvent>();
    }
    public bool in_use = true;
    public string name;
    public DataType data_type;
    public DataEventType type;
    public List<SDVBaseEvent> events;
    public bool use_position;
    public bool use_target;
    public bool empty = true;
    public Color color = Color.black;
}
public class SDVBaseEvent
{
    public string name;
    int playerID;
    int sessionID;
    string timestamp;
    bool use_pos=false; //corner cases amirigth
    public Vector3 position;
    public GameObject target_go;
    public string target_GUID = "";
    public string target_name="";

    public SDVBaseEvent(string _name,int player_id, int session_id, Vector3? pos, GameObject _target)
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
        if (_target != null)
        {
            SDVEventTracker tmp = _target.GetComponent<SDVEventTracker>();
            //   Debug.Log(tmp.gameObject.name);
            if (tmp != null)
            {
                target_GUID = tmp.getGUID();
                //Debug.Log("GUID: " + tmp.GUID);
            }
            else
            {
                target_name = _target.name;
            }
            //else
            //{
            //    Debug.LogWarning("Trying to add GameObject " + _target + " to a multi targeted event without and EventTracker component");
            //}
        }

    }

    public SDVBaseEvent(string line, string _name, bool use_position, bool use_target)
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
            string subline = line.Substring(start, end - start);
            if (subline[subline.Length-1] == '-')
            {
                target_GUID = subline;
                target_go = SDVCSVhandling.getGameObject(target_GUID);
            }
            else
            {
                target_name = subline;
                if (!(target_go = GameObject.Find(target_name)))
                {
                    //events[i].target = CSVhandling.getGameObject(events[i].target_GUID);
                    if (target_go == null)
                    {
                        Debug.LogError("Couldnt find target GameObject with name "+target_name );
                    }

                }
            }
        }
    }

    public virtual void saveToCSV(StreamWriter file)
    {
        file.Write(playerID + "," + sessionID + "," + timestamp + ",");
        if (use_pos)
        {
            file.Write(position.x+ "," + position.y + "," + position.z + ",");
        }
        if(target_GUID != "")
        {
          file.Write(target_GUID + ",");
        }
        else if(target_name != "")
        {
            file.Write(target_name + ",");
        }
    }
};

class SDVBoolEvent : SDVBaseEvent
{
    public bool data;
    public SDVBoolEvent(bool ev, string _name, int player_id, int session_id, Vector3? pos, GameObject target) :base(_name,player_id,session_id,pos,target)
    {
        data = ev;
    }

    public SDVBoolEvent(string line, string _name,bool use_position, bool use_target) :base(line,_name,  use_position, use_target)
    {
        data = bool.Parse(line.Substring(line.LastIndexOf(',') + 1));
    }
    public override void saveToCSV(StreamWriter file)
    {
        base.saveToCSV(file);
        file.Write(data);
    }
};

class SDVIntEvent : SDVBaseEvent
{
    public int data;
    public SDVIntEvent(int ev, string _name, int player_id, int session_id, Vector3? pos = null, GameObject target = null) : base(_name, player_id, session_id, pos, target)
    {
        data = ev;
    }
    public SDVIntEvent(string line, string _name, bool use_position, bool use_target) : base(line, _name, use_position, use_target)
    {
        data = int.Parse(line.Substring(line.LastIndexOf(',') + 1));
    }
    public override void saveToCSV(StreamWriter file)
    {
        base.saveToCSV(file);
        file.Write(data);
    }
};

class SDVFloatEvent : SDVBaseEvent
{
    public float data;
    public SDVFloatEvent(float ev, string _name, int player_id, int session_id, Vector3? pos = null, GameObject target = null) : base(_name, player_id, session_id, pos, target)
    {
        data = ev;
    }
    public SDVFloatEvent(string line, string _name, bool use_position, bool use_target) : base(line, _name, use_position, use_target)
    {
        data = float.Parse(line.Substring(line.LastIndexOf(',') + 1));
    }
    public override void saveToCSV(StreamWriter file)
    {
        base.saveToCSV(file);
        file.Write(data);
    }
};
class SDVStringEvent : SDVBaseEvent
{
    
    public string data;
    public SDVStringEvent(string ev, string _name, int player_id, int session_id, Vector3? pos = null, GameObject target = null) : base(_name, player_id, session_id, pos,target)
    {
        data = ev;
    }
    public SDVStringEvent(string line, string _name, bool use_position, bool use_target) : base(line, _name, use_position, use_target)
    {
        data = line.Substring(line.LastIndexOf(',') + 1);
    }
    public override void saveToCSV(StreamWriter file)
    {
        base.saveToCSV(file);
        file.Write(data);
    }
};
public class SDVVector3Event : SDVBaseEvent
{
    public Vector3 data;
    public SDVVector3Event(Vector3 ev, string _name, int player_id, int session_id, Vector3? pos = null, GameObject target = null) : base(_name, player_id, session_id, pos, target)
    {
        data = ev;
    }

    public SDVVector3Event(string line, string _name, bool use_position, bool use_target) : base(line, _name, use_position, use_target)
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
        file.Write(data.x + "," + data.y + "," + data.z+",");

    }
};
