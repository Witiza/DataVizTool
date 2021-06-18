using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    float speed = 50f;
    [SerializeField]
    int hp = 10;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
       rb= GetComponent<Rigidbody>();
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        // rb.velocity = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            rb.AddForce(transform.forward * Time.deltaTime * speed);
            //transform.Translate(Vector3.forward * Time.deltaTime * speed);

        if (Input.GetKey(KeyCode.S))
            rb.AddForce(transform.forward * Time.deltaTime * speed);


        // transform.Translate(-1 * Vector3.forward * Time.deltaTime * speed);

        if (Input.GetKey(KeyCode.A))
            rb.rotation = transform.rotation * Quaternion.Euler(0, -3, 0);

        if (Input.GetKey(KeyCode.D))
            rb.rotation = transform.rotation * Quaternion.Euler(0, 3, 0);

        if (Input.GetKey(KeyCode.Space))
            hp -= 1;
        if(hp<=0)
        {
            SDVEventHandler.StoreEventStatic("TST_death","Uded");
        }
    }
}
