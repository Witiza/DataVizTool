using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[ExecuteInEditMode]
public class EventTracker : MonoBehaviour
{
    // Start is called before the first frame update

    string GUID = "";
    public List<BaseEvent> events = new List<BaseEvent>();
    SceneDataViewer parent =null;
    float alpha;
    Color color;

    void Start()
    {
        getParent();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void getParent()
    {
        if(parent == null)
        {
            parent = EditorWindow.GetWindow<SceneDataViewer>();

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

    public void generateColor()
    {
        if (parent)
        {
            int count = 0;
            foreach (BaseEvent ev in events)
            {
                if (parent.checkIfUsingEvent(ev.name))
                {
                    count++;
                }
            }
            alpha = count / (float)parent.max_events;
            color = parent.gradient.Evaluate(count / (float)parent.max_events);
        }
        else
        {
            getParent();
        }
    }
    void recursiveParent(Transform trns)
    {

        if (trns != null)
        {
            recursiveParent(trns.parent);
            GUID += trns.GetSiblingIndex() + "-";
        }

        else
        {
            getParent();
        }
    }
    

    private void OnDrawGizmos()
    {
        if (parent != null)
        {
            Gizmos.color = color;
            Gizmos.DrawCube(gameObject.transform.position, new Vector3(10 * events.Count, 10 * events.Count, 10 * events.Count));
        }
        else
        {
            getParent();
        }
    }

}
