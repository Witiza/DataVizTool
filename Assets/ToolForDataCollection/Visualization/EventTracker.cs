using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EventTracker : MonoBehaviour
{
    // Start is called before the first frame update

    //Look into transform.has changed, guids and hierarchy index

    //https://answers.unity.com/questions/1780694/how-to-get-some-unique-identifier-for-gameobjects.html
    //https://docs.unity3d.com/ScriptReference/Transform.SetSiblingIndex.html
    //https://stackoverflow.com/questions/63482810/detect-hierarchy-change-during-runtime

    string GUID = "";
    public List<BaseEvent> events = new List<BaseEvent>();

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("HENLO");
        if(events.Count > 0)
        {
            Debug.Log("Events: " + events.Count);
        }
    }

    public string getGUID()
    {
        if(GUID == "")
        {
            generateUID();
        }
        return GUID;
    }
    void generateUID()
    {
        recursiveParent(transform);
        //GUID += transform.GetSiblingIndex();
    }
    
    void recursiveParent(Transform trns)
    {
        if(trns != null)
        {
            recursiveParent(trns.parent);
            GUID += trns.GetSiblingIndex() + "-";
        }
      
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(gameObject.transform.position, new Vector3(10 * events.Count, 10 * events.Count, 10 * events.Count));
    }

}
