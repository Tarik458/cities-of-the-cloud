using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cities : MonoBehaviour
{
    public CharacterController controller;
    public int InWindZones= 0;
    public Rigidbody rb;
    public GameObject WindZone;
    

    public float speed = 12;


    Vector3 velocity;
    bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "WindArea")
        {
            WindZone = coll.gameObject;
            InWindZones++;
        }
    }
    void OnTriggerExit(Collider coll)
    {

        if (coll.gameObject.tag == "WindArea")
        {
            InWindZones--;
        }

    }


    void FixedUpdate()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 2f;
        }

        float X = Input.GetAxis("Horizontal");
        float Z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * X + transform.forward * Z;

        Vector3 Windforce = Vector3.zero; 

        if (InWindZones>0)
        {
           Windforce += WindZone.GetComponent<WindArea>().Direction * WindZone.GetComponent<WindArea>().Force;
        }

        rb.velocity=(move * speed + Windforce);
    }

}








