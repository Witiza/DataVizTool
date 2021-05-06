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
        path+=events.name+'-'+scene+'-'+events.dataTypeToString()+".csv";
        StreamWriter file;
        if(File.Exists(path))
        {
           file = new StreamWriter(path, true);
        }
        else
        {
            file = new StreamWriter(path,false);
            file.WriteLine("EventName,PlayerID,SessionID,Timestamp,Data");
        }

       
        for(int i = 0;i<events.ingame_events.Count;i++)
        {
            events.ingame_events[i].saveToCSV(file);
        }
        file.Close();
    }

    public static EventContainer LoadCSV(string name,string scene, string data_type)
    {
        EventContainer ret = new EventContainer(name);
        string path = Application.persistentDataPath + "/events/";
        Directory.CreateDirectory(path);
        path += name + '-' + scene + '-'+data_type+".csv";
        //path += "Position-TestScene-VECTOR3.csv";
        StreamReader file;
        if (File.Exists(path))
        {
            file = new StreamReader(path);
            file.ReadLine();
            string line;
            switch (data_type)
            {
                case "BOOL":
                    ret.type = DataType.BOOL;
                    while ((line = file.ReadLine()) != null)
                    {
                        ret.events.Add(new BoolEvent(line,name));
                     }
                    break;
                case "INT":
                    ret.type = DataType.INT;
                    while ((line = file.ReadLine()) != null)
                    {
                        ret.events.Add(new IntEvent(line,name));
                    }
                    break;
                case "FLOAT":
                    ret.type = DataType.FLOAT;
                    while ((line = file.ReadLine()) != null)
                    {
                        ret.events.Add(new FloatEvent(line,name));
                    }
                    break;
                case "CHAR":
                    ret.type = DataType.CHAR;
                    while ((line = file.ReadLine()) != null)
                    {
                        ret.events.Add(new CharEvent(line,name));
                    }
                    break;
                case "STRING":
                    ret.type = DataType.STRING;
                    while ((line = file.ReadLine()) != null)
                    {
                        ret.events.Add(new StringEvent(line,name));
                    }
                    break;
                case "VECTOR3":
                    ret.type = DataType.VECTOR3;
                    while ((line = file.ReadLine()) != null)
                    {
                        ret.events.Add(new Vector3Event(line,name));
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
}
