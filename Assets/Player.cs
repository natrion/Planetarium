using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject Camera;
    public float sensitivity;
    public float speed;
    public float Gravitystrenght;
    private bool Onplanet = false;
    public  float planetCamerashiftDistance;
    private Vector3 planetPosition;
    private float YRotation;

    void OnTriggerStay(Collider other)
    {
        //Camera.transform.position = transform.position;
        int ComplexityOfPlanet = other.transform.parent.GetComponent<GeneratePlanet>().CompelxityOfPlanets[other.transform.GetSiblingIndex() ] ;
        float distance = Mathf.Abs(other.transform.position.x - transform.position.x) + Mathf.Abs(other.transform.position.y - transform.position.y) + Mathf.Abs(other.transform.position.z - transform.position.z);
        Vector3 direction = (other.transform.position - transform.position).normalized;
        
        transform.GetComponent<Rigidbody>().AddForce(direction * ((float)ComplexityOfPlanet / distance) / Gravitystrenght);

        if(distance < planetCamerashiftDistance )
        {
            if (Onplanet == false)
            {
                Camera.transform.localEulerAngles = Camera.transform.up;
            }
            transform.rotation = Quaternion.FromToRotation(-transform.up, direction) * transform.rotation;
            Onplanet = true;
            planetPosition = other.transform.position;
        }
        else
        {

            if (planetPosition == other.transform.position)
            {
                if (Onplanet == true)
                {
                    Camera.transform.eulerAngles = new Vector3(0, 0, 0);
                }
                Onplanet = false;
            }            
        }
    }

    void FixedUpdate()
    {
        
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        transform.GetComponent<Rigidbody>().AddForce(Camera.transform.right * speed * h );
        transform.GetComponent<Rigidbody>().AddForce(Camera.transform.forward * speed * v);

        //Camera.transform.position = transform.position;

        float mY = Input.GetAxis("Mouse Y");
        float mX = Input.GetAxis("Mouse X");

        if (Onplanet == false)
        {
            transform.eulerAngles += new Vector3(0, mX * sensitivity / 50, 0);
            YRotation += mY * Time.deltaTime * sensitivity;
            YRotation = Mathf.Clamp(YRotation, -60, 60);
            
            Camera.transform.localEulerAngles = Vector3.left * YRotation;


        }
        else
        {
            //Vector3 planetPositionDiference = new Vector3(planetPosition.x - transform.position.x, planetPosition.y - transform.position.y, planetPosition.z - transform.position.z);
            //float floatplanetPositionDiference = planetPositionDiference.x + planetPositionDiference.y + planetPositionDiference.z;

           // planetPositionDiference = new Vector3(planetPositionDiference.x / floatplanetPositionDiference, 
           //                                       planetPositionDiference.y / floatplanetPositionDiference, 
           //                                       planetPositionDiference.z / floatplanetPositionDiference);
            //Camera.transform.localEulerAngles += Vector3.left * mY * sensitivity;
            //Camera.transform.localEulerAngles += Vector3.up * mX * sensitivity;
            transform.Rotate((Vector3.up * mX) * Time.deltaTime * sensitivity);

            YRotation +=  mY * Time.deltaTime * sensitivity;
            YRotation = Mathf.Clamp(YRotation, -60, 60);
            Camera.transform.localEulerAngles = Vector3.left * YRotation;
            print("Near Planet");
        }        
    }
}
