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
        string path = "Position-TestScene-VECTOR3.csv";

        int start = 0;
        int end = 0;

        string data_type = path.Substring(path.LastIndexOf("-")+1, path.LastIndexOf(".") - path.LastIndexOf("-")-1);

        Debug.Log("Sibling "+transform.GetSiblingIndex());

        string tst ="0-2-3-";

        Debug.Log(CSVhandling.getGameObject(tst).name);

        EventHandler.StoreEventStatic("Attack",true, null, gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
