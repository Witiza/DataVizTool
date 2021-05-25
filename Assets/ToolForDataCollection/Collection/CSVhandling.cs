using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public static  class CSVhandling 
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
            //First column in metadata indicates if the event uses position;
            if (events.save_position)
            {
                file.WriteLine("true,");
                file.WriteLine("PlayerID,SessionID,Timestamp,X,Y,Z,Data");
            }
            else {
                file.WriteLine("false,");
                file.WriteLine("PlayerID,SessionID,Timestamp,Data");
            }
        }

       
        for(int i = 0;i<events.ingame_events.Count;i++)
        {
            events.ingame_events[i].saveToCSV(file);
            if (events.data_type == DataType.NULL)
            {
                file.WriteLine();
            }
        }
        file.Close();
    }

    public static EventContainer LoadCSV(string name,string scene, string data_type)
    {
        EventContainer ret = new EventContainer(name);
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
                    ret.type = DataType.NULL;
                    while ((line = file.ReadLine()) != null)
                    {
                        ret.events.Add(new BaseEvent(line, name, ret.use_position));
                    }
                    break;
                case "BOOL":
                    ret.type = DataType.BOOL;
                    while ((line = file.ReadLine()) != null)
                    {
                        ret.events.Add(new BoolEvent(line,name,ret.use_position));
                     }
                    break;
                case "INT":
                    ret.type = DataType.INT;
                    while ((line = file.ReadLine()) != null)
                    {
                        ret.events.Add(new IntEvent(line,name, ret.use_position));
                    }
                    break;
                case "FLOAT":
                    ret.type = DataType.FLOAT;
                    while ((line = file.ReadLine()) != null)
                    {
                        ret.events.Add(new FloatEvent(line,name, ret.use_position));
                    }
                    break;
                case "CHAR":
                    ret.type = DataType.CHAR;
                    while ((line = file.ReadLine()) != null)
                    {
                        ret.events.Add(new CharEvent(line,name, ret.use_position));
                    }
                    break;
                case "STRING":
                    ret.type = DataType.STRING;
                    while ((line = file.ReadLine()) != null)
                    {
                        ret.events.Add(new StringEvent(line,name, ret.use_position));
                    }
                    break;
                case "VECTOR3":
                    ret.type = DataType.VECTOR3;
                    while ((line = file.ReadLine()) != null)
                    {
                        ret.events.Add(new Vector3Event(line,name, ret.use_position));
                    }
                    break;
            }
            file.Close();
        }
        else
        {
            Debug.Log("Unable to open: "+path);
        }
        return ret;
    }

    static EventContainer readCSVMetadata(StreamReader file,EventContainer events)
    {
        EventContainer ret = events;
        string line = file.ReadLine();

        int start = 0;
        int end = line.IndexOf(',');

        events.use_position = bool.Parse(line.Substring(start, end - start));
        return ret;
    }

    public static string dataTypeToString(DataType data_type)
    {
        string ret = "ERROR";
        switch (data_type)
        {
            case DataType.NULL:
                ret = "NULL";
                break;
            case DataType.BOOL:
                ret = "BOOL";
                break;
            case DataType.INT:
                ret = "INT";
                break;
            case DataType.FLOAT:
                ret = "FLOAT";
                break;
            case DataType.CHAR:
                ret = "CHAR";
                break;
            case DataType.STRING:
                ret = "STRING";
                break;
            case DataType.VECTOR3:
                ret = "VECTOR3";
                break;
        }
        return ret;
    }
}
