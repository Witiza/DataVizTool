using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    string name;
    int playerID;
    int sessionID;
    string timestamp;
    int value;
    Vector3 data;
    [SerializeField]
    public GameObject obj;
    SerializedObject s_obj;
   SerializedProperty s_property;
    void Start()
    {
        //string helo = "5-3-6-3-";
        //Debug.Log("Last character: "+helo[helo.Length-1]);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
