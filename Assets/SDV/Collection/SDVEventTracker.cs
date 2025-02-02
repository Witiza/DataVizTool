﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class SDVEventTracker : MonoBehaviour
{

    string GUID = "";
    public List<SDVBaseEvent> events = new List<SDVBaseEvent>();

    [HideInInspector]
    public SDVGameObjects parent =null;
    float alpha;
    Color color;
    public float yoffset;

    public Dictionary<string, SDVPair<Color, int>> sepparated_events = new Dictionary<string, SDVPair<Color, int>>();
 

    void Start()
    {
        getParent();
    }

    // Update is called once per frame
    void Update()
    {
        //if(!Application.isPlaying)
        //    Debug.Log(gameObject.name + " Stored events: " + events.Count);
    }

    void getParent()
    {
        if(parent == null)
        {
            parent = EditorWindow.GetWindow<SDVGameObjects>();

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
            foreach (SDVBaseEvent ev in events)
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

    public void sepparateEvents()
    {
        foreach(SDVBaseEvent ev in events)
        {
            if(sepparated_events.ContainsKey(ev.name))
            {
                sepparated_events[ev.name].Second++;
            }
            else
            {
                Color color = parent.getEventColor(ev.name);
                sepparated_events.Add(ev.name, new SDVPair<Color, int>(color, 1));
            }
        }
        Debug.Log(sepparated_events);
    }
    private void OnDrawGizmos()
    {
        drawGizmos(false);
    }

    private void OnDrawGizmosSelected()
    {
        drawGizmos(true);
    }

    void drawGizmos(bool on_selected)
    {
        if (parent != null)
        {
            if (parent.selection == on_selected)
            {

                Gizmos.color = color;
                Vector3 pos = gameObject.transform.position;
                pos.y += yoffset + parent.yoffset + (events.Count*parent.size_multiplier)/2;
                Vector3 scale = Vector3.one * parent.size_multiplier;
                if (!parent.sepparated)
                {
                    scale *= events.Count;
                    Gizmos.DrawCube(pos, scale);
                    Gizmos.DrawCube(Vector3.zero, Vector3.one * 10);
                }
                else
                {
                    Quaternion rotation = Quaternion.Euler(0, Camera.current.transform.eulerAngles.y, 0);
                    Matrix4x4 matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, Vector3.one)*Matrix4x4.Rotate(rotation);                  
                    Gizmos.matrix = matrix;
                    int i = 0;
                    foreach(var pair in sepparated_events.Values)
                    {
                        i++;
                        scale.y = pair.Second*parent.y_multiplier;
                        pos = Vector3.zero;
                        pos.y = yoffset + parent.yoffset + (pair.Second*parent.y_multiplier * parent.size_multiplier) / 2;
                        pos.x = (-1 * parent.size_multiplier * sepparated_events.Count) / 2 + i * parent.size_multiplier;

                        Gizmos.color = pair.First;
                        Gizmos.DrawCube(pos, scale);

                    }
                    
                }
            }
        }
        else
        {
            getParent();
        }
    }
}

//https://stackoverflow.com/questions/569903/multi-value-dictionary
public class SDVPair<TFirst, TSecond>
{
    public SDVPair(TFirst first, TSecond second)
    {
        this.First = first;
        this.Second = second;
    }


    public TFirst First { get; set; }

    public TSecond Second { get; set; }

    public bool Equals(SDVPair<TFirst, TSecond> other)
    {
        if (other == null)
        {
            return false;
        }
        return EqualityComparer<TFirst>.Default.Equals(this.First, other.First) &&
               EqualityComparer<TSecond>.Default.Equals(this.Second, other.Second);
    }

    public override bool Equals(object o)
    {
        return Equals(o as SDVPair<TFirst, TSecond>);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<TFirst>.Default.GetHashCode(First) * 37 +
               EqualityComparer<TSecond>.Default.GetHashCode(Second);
    }
}
