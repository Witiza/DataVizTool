using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DataViewer : EditorWindow
{
   public EventHandler event_handler = null;
    public List<EventContainer> events = new List<EventContainer>();
    public int max_events = 0;
    public Gradient gradient = new Gradient();

    public bool modify_size = false;
    public float size_multiplier = 0;
    public float yoffset = 10;

    public GUIStyle inspector_title = new GUIStyle();
    public GUIStyle text = new GUIStyle();

    public bool getEventHandler()
    {
        if (event_handler == null)
        {
            if (!(event_handler = GameObject.FindObjectOfType<EventHandler>()))
            {
                Debug.LogError("There is no EventHandler component in the scene");
                return false;
            }
        }
        return true;
    }

  

    public void setStyles()
    {
        inspector_title.fontSize = 20;
        inspector_title.normal.textColor = Color.white;
        inspector_title.alignment = TextAnchor.MiddleCenter;
    }

    public bool checkIfUsingEvent(string name)
    {

        foreach (EventContainer tmp in events)
        {
            if (tmp.name == name)
            {
                if (tmp.in_use)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public Color getEventColor(string name)
    {
        Color ret = Color.black;

        foreach(EventContainer tmp in events)
        {
            if(tmp.name == name)
             {
                Debug.Log("Name + Returnint : " + name+"   "+tmp.color);
                return tmp.color;
            }
        }
        return ret;
    }

    public static void DrawUILine(Color color, int thickness = 2, int padding = 10)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += padding / 2;
        r.x -= 2;
        r.width += 6;
        EditorGUI.DrawRect(r, color);
    }
}
