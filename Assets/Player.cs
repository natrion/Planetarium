using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class Player : MonoBehaviour
{
    public static bool pouse = false;
    public GameObject Camera;
    public float sensitivity;
    public float speed;
    public float Gravitystrenght;
    private bool Onplanet = false;
    public  float planetCamerashiftDistance;
    private GameObject planetGameObject;
    private float YRotation;
    public GameObject PouseMenu;
    public Transform PlayerFolder;

    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int X, int Y);

    void Start()
    {
        pouse = false;
        SetCursorPos(-1, -1);
        PouseMenu.SetActive(false);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pouse == false)
            {
                pouse = true;
                PouseMenu.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                pouse = false;
                PouseMenu.SetActive(true);
                Time.timeScale = 0;
            }
        }
        if (pouse == true)
        {
            SetCursorPos(-1, -1);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Planet"))
        {
            
            //Camera.transform.position = transform.position;
            
            float ComplexityOfPlanet = other.GetComponent<Planet>().PlanetComplexity * other.transform.localScale.x;
            float distance = Mathf.Abs(other.transform.position.x - transform.position.x) + Mathf.Abs(other.transform.position.y - transform.position.y) + Mathf.Abs(other.transform.position.z - transform.position.z);
            Vector3 direction = (other.transform.position - transform.position).normalized;

            if (other.transform.localScale.x == 0.7f)
            {
                transform.GetComponent<Rigidbody>().AddForce(direction * ((float)ComplexityOfPlanet / Mathf.Sqrt(distance) * Gravitystrenght));
            }
            else
            {
                if (distance < ComplexityOfPlanet)
                {
                    transform.GetComponent<Rigidbody>().AddForce(direction * ((float)ComplexityOfPlanet / Mathf.Sqrt(distance) * Gravitystrenght));
                }
                
            }
            

            if (distance < ComplexityOfPlanet + (other.GetComponent<Planet>().Intensity / 100) / 2.5f)
            {
                if (Onplanet == false)
                {
                    Camera.transform.localEulerAngles = Camera.transform.up;
                    transform.parent = other.transform;
                    for (int i = 0; i < other.transform.childCount; i++)
                    {
                        if (!other.transform.GetChild(i).gameObject.CompareTag("Planet"))
                        {
                            other.transform.GetChild(i).gameObject.SetActive(true);
                        }
                    }
                    // RenderSettings.fog = true;
                }
                transform.rotation = Quaternion.FromToRotation(-transform.up, direction) * transform.rotation;
                Onplanet = true;
                planetGameObject = other.gameObject;

                // float atmosphereDensityOfPlanet = other.transform.parent.GetComponent<GeneratePlanet>().atmosphereDensityOfPlanets[other.transform.GetSiblingIndex()];
                //  RenderSettings.fogDensity = atmosphereDensityOfPlanet * ( 1 - (distance / (ComplexityOfPlanet / 1.5f)) ) ;
            }
            else
            {

                if (planetGameObject== other.gameObject)
                {
                    if (Onplanet == true)
                    {
                        transform.eulerAngles = new Vector3(0, 0, 0);
                        Camera.transform.eulerAngles = new Vector3(0, 0, 0);
                        transform.parent = PlayerFolder;
                        for (int i = 0; i < other.transform.childCount; i++)
                        {
                            if (!other.transform.GetChild(i).gameObject.CompareTag("Planet"))
                            {
                                other.transform.GetChild(i).gameObject.SetActive(false);
                            }
                           
                        }
                        // RenderSettings.fog = false;
                    }
                    Onplanet = false;
                }
            }
        }
    }

    void FixedUpdate()
    {
        speed += speed * Input.GetAxis("Mouse ScrollWheel") / 2;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float up = System.Convert.ToInt32(Input.GetKey(KeyCode.Space)) + System.Convert.ToInt32(Input.GetKey(KeyCode.LeftShift)) * -1;

        transform.GetComponent<Rigidbody>().AddForce(Camera.transform.right * speed * h );
        transform.GetComponent<Rigidbody>().AddForce(Camera.transform.forward * speed * v);
        transform.GetComponent<Rigidbody>().AddForce(Camera.transform.up * speed * up);

        //Camera.transform.position = transform.position;

        float mY = Input.GetAxis("Mouse Y");
        float mX = Input.GetAxis("Mouse X");

        if (Onplanet == false)
        {
            transform.eulerAngles += new Vector3(0, mX * sensitivity / 50, 0);
            YRotation += mY * Time.deltaTime * sensitivity;
            YRotation = Mathf.Clamp(YRotation, -90, 90);
            
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
            YRotation = Mathf.Clamp(YRotation, -80, 80);
            Camera.transform.localEulerAngles = Vector3.left * YRotation;
           // print("Near Planet");
        }        
    }
}
