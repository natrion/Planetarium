using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject Camera;
    public float sensitivity;
    public float speed;
    public float Gravitystrenght;
    private Transform OnplanetDirection;
    private float planetCamerashiftDistance;

    void OnTriggerStay(Collider other)
    {
        Camera.transform.position = transform.position;
        int ComplexityOfPlanet = other.transform.parent.GetComponent<GeneratePlanet>().CompelxityOfPlanets[other.transform.GetSiblingIndex() ] ;
        float distance = Mathf.Abs(other.transform.position.x - transform.position.x) + Mathf.Abs(other.transform.position.y - transform.position.y) + Mathf.Abs(other.transform.position.z - transform.position.z);
        transform.GetChild(0).LookAt(other.transform.position);
        transform.GetComponent<Rigidbody>().AddForce(transform.GetChild(0).forward * ((float)ComplexityOfPlanet / distance) / Gravitystrenght);

        if(distance < planetCamerashiftDistance & OnplanetDirection != null)
        {
            OnplanetDirection = transform.GetChild(0); 
        }else
        {
            OnplanetDirection = null;
        }
    }

    void FixedUpdate()
    {
        
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        transform.GetComponent<Rigidbody>().AddForce(Camera.transform.right * speed * h );
        transform.GetComponent<Rigidbody>().AddForce(Camera.transform.forward * speed * v);

        Camera.transform.position = transform.position;

        float mY = Input.GetAxis("Mouse Y");
        float mX = Input.GetAxis("Mouse X");

        if (OnplanetDirection == null)
        {
            Camera.transform.eulerAngles += new Vector3(-mY * sensitivity, mX * sensitivity, 0);
        }else
        {
            Camera.transform.eulerAngles += new Vector3(-mY * sensitivity, mX * sensitivity, 0);
            print("Near Planet");
        }        
    }
}
