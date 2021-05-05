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
        }

        file.WriteLine("EventName,PlayerID,SessionID,Timestamp,Data");
        for(int i = 0;i<events.ingame_events.Count;i++)
        {
            events.ingame_events[i].saveToCSV(file);
        }
        file.Close();
    }

    public static StandardEvent LoadCSV()
    {
        StandardEvent ret = new StandardEvent();
        string path = Application.persistentDataPath + "/events/";
        Directory.CreateDirectory(path);
        // path += events.name + '-' + scene + ".csv";
        path += "Position.csv";
        StreamReader file;
        if (File.Exists(path))
        {
            file = new StreamReader(path);
            file.ReadLine();
            
            string line;
            string data_type = path.Substring(path.LastIndexOf("-"), path.LastIndexOf(".") - path.LastIndexOf("-"));
            switch(data_type)
            {
                case "BOOL":
                    while ((line = file.ReadLine()) != null)
                    {
                        
                        ret.ingame_events.Add(new BoolEvent(line));
                     }
                    break;
                case "INT":
                    while ((line = file.ReadLine()) != null)
                    {

                        ret.ingame_events.Add(new IntEvent(line));
                    }
                    break;
                case "FLOAT":
                    while ((line = file.ReadLine()) != null)
                    {

                        ret.ingame_events.Add(new FloatEvent(line));
                    }
                    break;
                case "CHAR":
                    while ((line = file.ReadLine()) != null)
                    {
                        ret.ingame_events.Add(new CharEvent(line));
                    }
                    break;
                case "STRING":
                    while ((line = file.ReadLine()) != null)
                    {
                        ret.ingame_events.Add(new StringEvent(line));
                    }
                    break;
                case "VECTOR3":
                    while ((line = file.ReadLine()) != null)
                    {
                        ret.ingame_events.Add(new Vector3Event(line));
                    }
                    break;
            }
          

        }
        else
        {
            Debug.Log("homie");
        }
        return new StandardEvent();
    }
}
