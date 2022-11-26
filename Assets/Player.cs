using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using TMPro;

public class Player : MonoBehaviour
{
    public float Damage;
    public static bool pouse = true;
    public static bool OnInventory = true;
    public GameObject Camera;
    public float sensitivity;
    public float speed;
    public float Gravitystrenght;
    private bool Onplanet = false;
    public  float planetCamerashiftDistance;
    private GameObject planetGameObject;
    private float YRotation;
    public GameObject PouseMenu;
    public GameObject InventoriMenu;
    public Transform PlayerFolder;

    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int X, int Y);

    void Start()
    {
        pouse = true;
        OnInventory = true;
        //SetCursorPos(-1, -1);
        Cursor.visible = false;
        PouseMenu.SetActive(false);
        InventoriMenu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update()
    {
        if (Input.GetMouseButton(0) & Time.timeScale != 0)
        {
            RaycastHit hit ;
            Ray ray = new Ray(Camera.transform.position, Camera.transform.forward);

            if (Physics.Raycast(ray, out hit , 50f))
            { 
                if (hit.collider.gameObject.CompareTag("Rock")  )
                {
                    hit.collider.GetComponent<ThingData>().HealthToNewOre -= Damage;
                    if (0 > hit.collider.GetComponent<ThingData>().HealthToNewOre)
                    {
                        GameObject ore = Instantiate(hit.collider.GetComponent<ThingData>().Ore);                       
                        ore.transform.position = hit.point;                                  
                        ore.transform.parent = transform.parent;
                        hit.collider.GetComponent<ThingData>().HealthToNewOre = hit.collider.GetComponent<ThingData>().MaxHealthToNewOre;
                    }

                    hit.collider.GetComponent<ThingData>().Health -= Damage;
                    
                    float health = hit.collider.GetComponent<ThingData>().Health;
                    float maxHealth = hit.collider.GetComponent<ThingData>().MaxHealth;
                    float StartRockSize = hit.collider.GetComponent<ThingData>().StartRockSize;

                    hit.collider.transform.localScale = new Vector3(1, 1, 1) * (health / maxHealth * StartRockSize);

                    if (2 > health)
                    {
                        Destroy(hit.collider.gameObject);
                    }

                }
                else if (hit.collider.gameObject.CompareTag("Planet"))
                {
                    /////taking health from planet to spawn ore
                    hit.collider.GetComponent<ThingData>().HealthToNewOre1 -= Damage;
                    hit.collider.GetComponent<ThingData>().HealthToNewOre2 -= Damage;
                    ///////////////////////////////////////////////SpawningOre1
                    if (0 > hit.collider.GetComponent<ThingData>().HealthToNewOre1)
                    {
                        GameObject ore1 = Instantiate(hit.collider.GetComponent<ThingData>().PlanetOre1);

                        Vector3 direction = (  transform.position - transform.parent.position).normalized;
                        ore1.transform.position = new Vector3(hit.point.x + direction.x,
                                                              hit.point.y + direction.y,
                                                              hit.point.z + direction.z);
                        ore1.transform.parent = transform.parent;
                        hit.collider.GetComponent<ThingData>().HealthToNewOre1 = hit.collider.GetComponent<ThingData>().PlanetMaxHealthToNewOre1;
                    }
                    ///////////////////////////////////////////////SpawningOre1
                    if (0 > hit.collider.GetComponent<ThingData>().HealthToNewOre2)
                    {
                        GameObject ore2 = Instantiate(hit.collider.GetComponent<ThingData>().PlanetOre2);

                        Vector3 direction = (transform.position - transform.parent.position).normalized;
                        ore2.transform.position = new Vector3(hit.point.x + direction.x,
                                                              hit.point.y + direction.y,
                                                              hit.point.z + direction.z);
                        ore2.transform.parent = transform.parent;
                        hit.collider.GetComponent<ThingData>().HealthToNewOre2 = hit.collider.GetComponent<ThingData>().PlanetMaxHealthToNewOre2;
                    }
                }
                
            }
        }
        if (Input.GetMouseButtonDown(0) & Time.timeScale != 0)
        {
            RaycastHit hit;
            Ray ray = new Ray(Camera.transform.position, Camera.transform.forward);
            if ( Physics.Raycast(ray, out hit, 50f))
            {
                if (hit.collider.gameObject.CompareTag("Ore"))
                {
                    Destroy(hit.collider.gameObject);

                    bool FoundSamePanel = false;
                    for (int OnChild = 0; OnChild < InventoriMenu.transform.childCount; OnChild++)
                    {
                        Transform Child = InventoriMenu.transform.GetChild(OnChild);
                        if (Child.GetComponent<ThingData>().OreType == hit.collider.GetComponent<ThingData>().OreUI.GetComponent<ThingData>().OreType)
                        {
                            FoundSamePanel = true;
                            Child.GetComponent<ThingData>().AmountOfOres += hit.collider.GetComponent<ThingData>().OreAmountInrock;
                            string StringOfAmountOfOres = Child.GetComponent<ThingData>().AmountOfOres.ToString();
                            Child.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = StringOfAmountOfOres;
                        }    

                    }

                    if (FoundSamePanel == false)
                    {
                        
                        GameObject OreUI = Instantiate(hit.collider.GetComponent<ThingData>().OreUI);
                        OreUI.transform.SetParent(InventoriMenu.transform);

                        OreUI.GetComponent<ThingData>().AmountOfOres = hit.collider.GetComponent<ThingData>().OreAmountInrock;
                        string StringOfAmountOfOres = OreUI.GetComponent<ThingData>().AmountOfOres.ToString();
                        OreUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = StringOfAmountOfOres;
                    }
                }
            }
            
        }

        
        if (Input.GetKeyDown(KeyCode.Escape) & OnInventory == true)
        {
            if (pouse == false)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                pouse = true;
                PouseMenu.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                pouse = false;
                PouseMenu.SetActive(true);
                Time.timeScale = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab) & pouse == true)
        {
            if (OnInventory == false)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                OnInventory = true;
                InventoriMenu.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                OnInventory = false;
                InventoriMenu.SetActive(true);
                Time.timeScale = 0;
            }
        }

    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Planet"))
        {
            
            //Camera.transform.position = transform.position;
            
            float ComplexityOfPlanet = other.GetComponent<ThingData>().PlanetComplexity * other.transform.localScale.x;
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
            

            if (distance < (ComplexityOfPlanet + (other.GetComponent<ThingData>().Intensity / 200)) / 1.5f)
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
