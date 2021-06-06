using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    float speed = 10f;
    [SerializeField]
    int hp = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.forward * Time.deltaTime * speed);

        if (Input.GetKey(KeyCode.S))
            transform.Translate(-1 * Vector3.forward * Time.deltaTime * speed);

        if (Input.GetKey(KeyCode.A))
            transform.Rotate(0, -1, 0);

        if (Input.GetKey(KeyCode.D))
            transform.Rotate(0, 1, 0);
        if (Input.GetKey(KeyCode.Space))
            hp -= 1;
        if(hp<=0)
        {
            EventHandler.StoreEventStatic("TST_death","Uded");
        }
    }
}
