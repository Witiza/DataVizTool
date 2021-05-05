using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    string name;
    int playerID;
    int sessionID;
    string timestamp;
    int value;
    Vector3 data;
    void Start()
    {
        string line = "Position,0,0,04//30//2021 11:57:18.256,69,700000000,2";

        int start = 0;
        int end = 0;

        start = 0;
        end = line.IndexOf(',');
        name = line.Substring(start, end-start);
       
        start = end+1;
        end = line.IndexOf(',', start);
        playerID = int.Parse(line.Substring(start, end-start));

        start = end+1;
        end = line.IndexOf(',', start);
        sessionID = int.Parse(line.Substring(start, end-start));

        start = end+1;
        end = line.IndexOf(',', start);
        timestamp = line.Substring(start, end-start);

       

     


        Debug.Log(name);
        Debug.Log(data);
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
