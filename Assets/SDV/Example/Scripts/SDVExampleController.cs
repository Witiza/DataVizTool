using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDVExampleController : MonoBehaviour
{
    float speed = 1000f;
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
         rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            rb.velocity = transform.forward * Time.deltaTime * speed;
            //transform.Translate(Vector3.forward * Time.deltaTime * speed);

        if (Input.GetKey(KeyCode.S))
            rb.velocity = transform.forward * Time.deltaTime * speed;


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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.name == "Pole")
        {
            SDVEventHandler.StoreEventStatic("SDVMultiTargeted", collision.collider.transform.position, collision.collider.gameObject);
        }
    }
    private void OnCollisionStay(Collision collision)
    {

            if (collision.collider.name == "Pole")
            {
               SDVEventHandler.StoreEventStatic("SDVMultiTargetedTwo", collision.collider.transform.position, collision.collider.gameObject);
            }
        
    }

}
