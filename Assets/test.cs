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
        EventHandler.StoreEventStatic("Attack",true, gameObject.transform.position, gameObject);
        if (obj)
        {
            s_obj = new SerializedObject(obj.GetComponent("test2"));
           // s_property = s_obj.FindProperty("test2").FindPropertyRelative("integer");

            //   s_obj = new SerializedObject(obj);

             s_property = s_obj.FindProperty("integer");

            
            //SerializedProperty prop = s_obj.GetIterator();
            //{
            //    if (prop.NextVisible(true))
            //    {
            //        do
            //        {
            //            Debug.Log(prop.name);
            //        }
            //        while (prop.NextVisible(true));
            //    }
            //}
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (obj)
        {
            s_obj.Update();
            Debug.Log("Property: " + s_property.intValue);
        }
    }
}
