using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class FileSystem 
{
    static List<string> getValidCSVs(string scene)
    {
        List<string> paths = new List<string>();
        string path = Application.persistentDataPath + "/events/";
        DirectoryInfo info = new DirectoryInfo(path);
        FileInfo[] fileInfo = info.GetFiles();
        return paths;
    }
    
}
