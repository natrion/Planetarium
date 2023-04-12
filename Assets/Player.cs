using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using TMPro;

public class Player : MonoBehaviour
{

    /// //////////////////////////////////////////////////////
    /// <Seting Variables>
    public bool techTreeOn;
    public GameObject techTreeOnUI;
    public GameObject ErrorMassageText;
    public float Damage;
    public static bool pouse = true;
    public static bool OnbuildMenu = true;
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
    public GameObject buildMenu;
    public Transform PlayerFolder;

    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int X, int Y);






     IEnumerator BuildMenuturning()///////////////seting building menu not in update function must be deley between closing build menu
     {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.F) & pouse == true & OnInventory == true)
            {
                if (OnbuildMenu == false)
                {
                    OnbuildMenu = true;
                    yield return null;
                    buildMenu.SetActive(false);
                }
                else
                {
                    OnbuildMenu = false;
                    yield return null;
                    buildMenu.SetActive(true);
                    ErrorMassageText.SetActive(false);
                }
            }
            yield return null;
        }
        
     }


    void Start()
    {
        techTreeOnUI.SetActive(false);
        ErrorMassageText.SetActive(false);

        StartCoroutine(BuildMenuturning());

        /// //////////////////////////////////////////////////////
        /// <Seting Variables On Start>
        ///  bool pouse = true;

        pouse = true;
        OnInventory = true;
        OnbuildMenu = true;
        //SetCursorPos(-1, -1);

        Cursor.visible = false;

        PouseMenu.SetActive(false);
        InventoriMenu.SetActive(false);
        buildMenu.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update()
    {////mining
        if (Input.GetMouseButton(0) & Time.timeScale != 0)////stops mining during pause
        {
            ////calculating thing in front of the player
            RaycastHit hit ;
            Ray ray = new Ray(Camera.transform.position, Camera.transform.forward);

            if (Physics.Raycast(ray, out hit , 50f))////stops script runing when nothing is in front of the player
            { 
                if (hit.collider.gameObject.CompareTag("Rock")  )/////rock mining
                {
                    hit.collider.GetComponent<ThingData>().HealthToNewOre -= Damage;////making less time to another ore

                    if (0 > hit.collider.GetComponent<ThingData>().HealthToNewOre)/////creating ore
                    {
                        GameObject ore = Instantiate(hit.collider.GetComponent<ThingData>().Ore);                       
                        ore.transform.position = hit.point;                                  
                        ore.transform.parent = transform.parent;
                        hit.collider.GetComponent<ThingData>().HealthToNewOre = hit.collider.GetComponent<ThingData>().MaxHealthToNewOre;
                    }

                    hit.collider.GetComponent<ThingData>().Health -= Damage;////taking damage from rock

                    float health = hit.collider.GetComponent<ThingData>().Health;
                    float maxHealth = hit.collider.GetComponent<ThingData>().MaxHealth;
                    float StartRockSize = hit.collider.GetComponent<ThingData>().StartRockSize;

                    hit.collider.transform.localScale = new Vector3(1, 1, 1) * (health / maxHealth * StartRockSize);////making rock smaller from mining

                    if (2 > health)////destroyng rock when low health (from 2 health -- for not making rock too small)
                    {
                        Destroy(hit.collider.gameObject);
                    }

                }
                else if (hit.collider.gameObject.CompareTag("Planet")) //planet mining
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
                    ///////////////////////////////////////////////SpawningOre2
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
        ////colecting ore
        if (Input.GetMouseButtonDown(0) & Time.timeScale != 0)
        {
            ////calculating thing in front of the player
            RaycastHit hit;
            Ray ray = new Ray(Camera.transform.position, Camera.transform.forward);

            if ( Physics.Raycast(ray, out hit, 50f))////stops script runing when nothing is in front of the player
            {
                if (hit.collider.gameObject.CompareTag("Ore"))////object must be ore
                {
                    Destroy(hit.collider.gameObject);////destroing ore

                    /////adding ore to inventory
                    
                    ////finding existing same type ore panel
                    bool FoundSamePanel = false;
                    for (int OnChild = 0; OnChild < InventoriMenu.transform.childCount; OnChild++)
                    {
                        Transform Child = InventoriMenu.transform.GetChild(OnChild);
                        if (Child.GetComponent<ThingData>().OreType == hit.collider.GetComponent<ThingData>().OreUI.GetComponent<ThingData>().OreType)
                        {
                            FoundSamePanel = true;
                            ////adding number of ores to panel
                            Child.GetComponent<ThingData>().AmountOfOres += hit.collider.GetComponent<ThingData>().OreAmountInrock;
                            string StringOfAmountOfOres = Child.GetComponent<ThingData>().AmountOfOres.ToString();
                            Child.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = StringOfAmountOfOres;
                        }    

                    }
                    /////when same panel is not found
                    if (FoundSamePanel == false)
                    {
                        //////creting new panel
                        GameObject OreUI = Instantiate(hit.collider.GetComponent<ThingData>().OreUI);
                        OreUI.transform.SetParent(InventoriMenu.transform);
                        /////seting number of ores to panel
                        OreUI.GetComponent<ThingData>().AmountOfOres = hit.collider.GetComponent<ThingData>().OreAmountInrock;
                        string StringOfAmountOfOres = OreUI.GetComponent<ThingData>().AmountOfOres.ToString();
                        OreUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = StringOfAmountOfOres;
                    }
                }
            }
            
        }
        if (pouse == true & OnInventory == true & techTreeOn == false)
        {
            if (OnbuildMenu == true || OnbuildMenu == false & Input.GetMouseButton(1))/////making mouse not visible when biuld mode holding not right mouse button or playing normaly
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else/////making mouse visible when biuld mode holding right mouse button
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        
        /// //////////////////////////////////////////////////////
        /// <Opening Escape Menu>
        if (Input.GetKeyDown(KeyCode.Escape) & OnInventory == true & techTreeOn == false)
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
        /// //////////////////////////////////////////////////////
        /// <Opening Inventori>
        if (Input.GetKeyDown(KeyCode.Tab) & pouse == true & techTreeOn == false )
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
        /// <Opening tech menu>
        if (Input.GetKeyDown(KeyCode.Q) & pouse == true & OnInventory == true )
        {
            if (techTreeOn == true)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                techTreeOn = false;
                techTreeOnUI.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                techTreeOn = true;
                techTreeOnUI.SetActive(true);
                Time.timeScale = 0;
            }
        }

    }

    void OnTriggerStay(Collider other)
    {
        /// //////////////////////////////////////////////////////
        /// <Gravity Simulation>
        if (other.gameObject.CompareTag("Planet"))
        {
            
            //Camera.transform.position = transform.position;
            
            /////////////seting variables from script storage for caculating gravity
            float ComplexityOfPlanet = other.GetComponent<ThingData>().PlanetComplexity * other.transform.localScale.x;
            float distance = Vector3.Distance(other.transform.position , transform.position);
            Vector3 direction = (other.transform.position - transform.position).normalized;
            /////////////sun Gravity
            if (other.transform.localScale.x == 0.7f)
            {
                transform.GetComponent<Rigidbody>().AddForce(direction * ((float)ComplexityOfPlanet / Mathf.Sqrt(distance) * Gravitystrenght));
            }
            /////////////planet gravity
            else
            {
                if (distance < ComplexityOfPlanet)////gravity works for some distance
                {
                    transform.GetComponent<Rigidbody>().AddForce(direction * ((float)ComplexityOfPlanet* ComplexityOfPlanet / Mathf.Sqrt(distance) * Gravitystrenght));
                }
                
            }
            
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////going off on planet
            if (distance < (ComplexityOfPlanet + (other.GetComponent<ThingData>().Intensity / 200)) / 1.5f)
            {
                /////////////////////////going on planet
                if (Onplanet == false)
                {
                    ////player camera chanig to planet mode
                    Camera.transform.localEulerAngles = Camera.transform.up;
                    transform.parent = other.transform;
                    ////showing rocks
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
                /////////////////////////going of planet
                if (planetGameObject== other.gameObject)
                {
                    if (Onplanet == true)
                    {
                        ///player camera chanig space mode
                        transform.eulerAngles = new Vector3(0, 0, 0);
                        Camera.transform.eulerAngles = new Vector3(0, 0, 0);
                        transform.parent = PlayerFolder;
                        ////hiding rocks
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
        ///////////seting speed of player
        if (OnbuildMenu == true)
        {
            speed += speed * Input.GetAxis("Mouse ScrollWheel") / 2;
        }
        

        ///////getting keyboard movemant
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float up = System.Convert.ToInt32(Input.GetKey(KeyCode.Space)) + System.Convert.ToInt32(Input.GetKey(KeyCode.LeftShift)) * -1;

        /////////////moving player
        transform.GetComponent<Rigidbody>().AddForce(Camera.transform.right * speed * h );
        transform.GetComponent<Rigidbody>().AddForce(Camera.transform.forward * speed * v);
        transform.GetComponent<Rigidbody>().AddForce(Camera.transform.up * speed * up);

        //Camera.transform.position = transform.position;

        ///////getting mouse movemant
        float mY = Input.GetAxis("Mouse Y");
        float mX = Input.GetAxis("Mouse X");
        if (OnbuildMenu == true || OnbuildMenu == false & Input.GetMouseButton(1))////not rotating camera during build mode          and not pressing right mouse button
        {
            //////////////////////////////rotating camera in space
            if (Onplanet == false)
            {
                transform.eulerAngles += new Vector3(0, mX * sensitivity / 50, 0);
                YRotation += mY * Time.deltaTime * sensitivity;
                YRotation = Mathf.Clamp(YRotation, -90, 90);

                Camera.transform.localEulerAngles = Vector3.left * YRotation;


            }
            else//////////////////////////////rotating camera on planet
            {
                //Vector3 planetPositionDiference = new Vector3(planetPosition.x - transform.position.x, planetPosition.y - transform.position.y, planetPosition.z - transform.position.z);
                //float floatplanetPositionDiference = planetPositionDiference.x + planetPositionDiference.y + planetPositionDiference.z;

                // planetPositionDiference = new Vector3(planetPositionDiference.x / floatplanetPositionDiference, 
                //                                       planetPositionDiference.y / floatplanetPositionDiference, 
                //                                       planetPositionDiference.z / floatplanetPositionDiference);
                //Camera.transform.localEulerAngles += Vector3.left * mY * sensitivity;
                //Camera.transform.localEulerAngles += Vector3.up * mX * sensitivity;
                transform.Rotate((Vector3.up * mX) * Time.deltaTime * sensitivity);

                YRotation += mY * Time.deltaTime * sensitivity;
                YRotation = Mathf.Clamp(YRotation, -80, 80);
                Camera.transform.localEulerAngles = Vector3.left * YRotation;
                // print("Near Planet");
            }
        }
        
    }
}
