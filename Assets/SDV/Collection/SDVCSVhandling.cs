using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;


public static  class SDVCSVhandling 
{
    public static void SaveToCSV(StandardEvent events, string scene)
    {
        string path = Application.persistentDataPath + "/events/";
        Directory.CreateDirectory(path);
        path += events.name + '-' + scene + '-' + dataTypeToString(events.data_type) + ".csv";
        StreamWriter file;
        if(File.Exists(path))
        {
           file = new StreamWriter(path, true);
        }
        else
        {
            file = new StreamWriter(path,false);
            //First column in metadata indicates if the event uses position;, the second one indicates if the event is using multiple targets

            file.WriteLine(events.save_position + ","+ (events.use_target)+ ",");
            file.Write("PlayerID, SessionID, Timestamp,");
            if (events.save_position)
            {
                file.Write("X,Y,Z,");
            }
           if(events.type == SDVEventType.MULTI_TARGET)
            { 
                file.Write("TargetGUID,");
            }
           if(events.use_target)
           {
                file.Write("TargetName,");
            }
            file.WriteLine();
        }

       
        for(int i = 0;i<events.ingame_events.Count;i++)
        {
            events.ingame_events[i].saveToCSV(file);
            file.WriteLine();
            
        }
        file.Close();
    }

    public static SDVEventContainer LoadCSV(string name,string scene, string data_type)
    {
        SDVEventContainer ret = new SDVEventContainer(name);
        string path = Application.persistentDataPath + "/events/";
        Directory.CreateDirectory(path);
        path += name + '-' + scene + '-'+data_type+".csv";
        Debug.Log("Opening: " + path);
        //path += "Position-TestScene-VECTOR3.csv";
        StreamReader file;
        if (File.Exists(path))
        {
            file = new StreamReader(path);

            ret = readCSVMetadata(file, ret);
            file.ReadLine();
            string line;
            switch (data_type)
            {
                case "NULL":
                    ret.data_type = SDVDataType.NULL;
                    while ((line = file.ReadLine()) != null)
                    {
                        ret.events.Add(new SDVBaseEvent(line, name, ret.use_position, ret.use_target));
                    }
                    break;
                case "BOOL":
                    ret.data_type = SDVDataType.BOOL;
                    while ((line = file.ReadLine()) != null)
                    {
                        ret.events.Add(new SDVBoolEvent(line,name,ret.use_position,ret.use_target));
                     }
                    break;
                case "INT":
                    ret.data_type = SDVDataType.INT;
                    while ((line = file.ReadLine()) != null)
                    {
                        ret.events.Add(new SDVIntEvent(line,name, ret.use_position, ret.use_target));
                    }
                    break;
                case "FLOAT":
                    ret.data_type = SDVDataType.FLOAT;
                    while ((line = file.ReadLine()) != null)
                    {
                        ret.events.Add(new SDVFloatEvent(line,name, ret.use_position, ret.use_target));
                    }
                    break;
                case "STRING":
                    ret.data_type = SDVDataType.STRING;
                    while ((line = file.ReadLine()) != null)
                    {
                        ret.events.Add(new SDVStringEvent(line,name, ret.use_position, ret.use_target));
                    }
                    break;
                case "VECTOR3":
                    ret.data_type = SDVDataType.VECTOR3;
                    while ((line = file.ReadLine()) != null)
                    {
                        ret.events.Add(new SDVVector3Event(line,name, ret.use_position, ret.use_target ));
                    }
                    break;
            }
            if(ret.events.Count >0)
            {
                ret.empty = false;
            }
            file.Close();
        }
        else
        {
            Debug.Log("Unable to open: "+path);
        }
        return ret;
    }

    static SDVEventContainer readCSVMetadata(StreamReader file,SDVEventContainer events)
    {
        SDVEventContainer ret = events;
        string line = file.ReadLine();

        int start = 0;
        int end = line.IndexOf(',');
        ret.use_position = bool.Parse(line.Substring(start, end - start));

        start = end + 1;
         end = line.IndexOf(',', start);
        ret.use_target = bool.Parse(line.Substring(start, end - start));
        return ret;
    }

    public static string dataTypeToString(SDVDataType data_type)
    {
        string ret = "ERROR";
        switch (data_type)
        {
            case SDVDataType.NULL:
                ret = "NULL";
                break;
            case SDVDataType.BOOL:
                ret = "BOOL";
                break;
            case SDVDataType.INT:
                ret = "INT";
                break;
            case SDVDataType.FLOAT:
                ret = "FLOAT";
                break;
            case SDVDataType.STRING:
                ret = "STRING";
                break;
            case SDVDataType.VECTOR3:
                ret = "VECTOR3";
                break;
        }
        return ret;
    }

    public static GameObject getGameObject(string GUID)
    {
        int depth = GUID.Split('-').Length-1;
        int start = 0;
        int end = GUID.IndexOf('-');
        int index;
        bool found = int.TryParse(GUID.Substring(start, end - start),out index);
        GameObject tmp = SceneManager.GetActiveScene().GetRootGameObjects()[index];

        for (int i = 1; i < depth;i++)
        {
            start = end + 1;
            end = GUID.IndexOf('-', start);
             found = int.TryParse(GUID.Substring(start, end - start), out index);
                tmp = tmp.transform.GetChild(index).gameObject;
        }

        return tmp;
    }


}
