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
        EventHandler.StoreEventStatic("Attack",true, null, gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
