using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public static  class CSVhandling 
{
    public static void SaveToCSV(StandardEvent events)
    {
        string path = Application.persistentDataPath + "/events/";
        Directory.CreateDirectory(path);
        path+=events.name+".csv";
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
}
