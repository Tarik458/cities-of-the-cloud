using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cities : MonoBehaviour
{
    public int InWindZones;
    public Rigidbody rb;
    public GameObject WindZone;

    public float forwardForce;
    public float sideForce;

    public float citySpeed;

    bool m_movementPaused = false;


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

    void Update()
    {
        if (!m_movementPaused)
        {
            if (Input.GetKey("a"))
            {
                rb.AddForce(-sideForce, 0, 0);
            }

            if (Input.GetKey("d"))
            {
                rb.AddForce(sideForce, 0, 0);
            }

            if (Input.GetKey("w"))
            {
                rb.AddForce(0, 0, forwardForce);
            }

            if (Input.GetKey("s"))
            {
                rb.AddForce(0, 0, -forwardForce);
            }
        }
    }

    public void pauseMovement(bool pause = true)
    {
        m_movementPaused = pause;
        rb.velocity = Vector3.zero;
    }

    void FixedUpdate()
    {
        if (!m_movementPaused)
        {
            if (InWindZones > 0)
            {
                rb.AddForce(WindZone.GetComponent<WindArea>().Direction * WindZone.GetComponent<WindArea>().Force);
            }

            else
            {
                rb.AddForce(0, 0, 0);
            }
        }
    }

}