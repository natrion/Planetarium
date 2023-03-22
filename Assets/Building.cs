using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Building : MonoBehaviour
{
    //builds thing hight
    public float thingsHight=1;
    //sting rotation variables
    private float rotatingX; //= System.Convert.ToInt32(Input.GetKey(KeyCode.Alpha1)) - System.Convert.ToInt32(Input.GetKey(KeyCode.Alpha2)); //setting variables for rotating thing
    private float rotatingY; //= System.Convert.ToInt32(Input.GetKey(KeyCode.Alpha3)) - System.Convert.ToInt32(Input.GetKey(KeyCode.Alpha4));
    private float rotatingZ;
    //seting text that thay will have need list
    public GameObject needList; 
    ///setting parameters for error Massage Text
    public GameObject ErrorMassageText;
    ////////setting parameters for resorses
    public int[] AmountResorsesToBuild;
    public string[] TypeResorsesToBuild;
    ///////////////setting parameters for inventory
    public GameObject inventory;
    ///////////////setting parameters for thing in select mode 
    public GameObject selectModeThing;
    ///////////////setting parameters for thing after buld
    public GameObject Thing;
    /// //////////player 
    public GameObject player;
    //////////////buildmode on bool
    public static bool buildmodeOn = false;


    void Start()
    {
        buildmodeOn = false;
        needList.SetActive(false);
    }

    ///////////////spawning cube after clicking
    public void ButtonClicked()
    {
        
        if (buildmodeOn == false )
        {
            buildmodeOn = true;
            StartCoroutine(PlacingTing());
        }
        

    }

    /////////////function for updating cube position
     IEnumerator PlacingTing()
     {

        yield return new WaitForSeconds(0.1f);

        needList.SetActive(true);
        needList.GetComponent<TextMeshProUGUI>().text = "you need:  ";
        for (int o = 0; o < AmountResorsesToBuild.Length; o++)
        {
            needList.GetComponent<TextMeshProUGUI>().text = needList.GetComponent<TextMeshProUGUI>().text + AmountResorsesToBuild[o].ToString() + " " + TypeResorsesToBuild[o] + "  ";
        }
        

        GameObject copySelectThing = Instantiate(selectModeThing);////copying thing

        Camera camera = player.transform.GetChild(0).GetComponent<Camera>();/////seting camera

        if (player.transform.parent.gameObject.name != "PlayerFolder")
        {
            copySelectThing.transform.position = player.transform.position;
            copySelectThing.transform.LookAt(player.transform.parent);
        }

        while (!Input.GetMouseButton(0) & Player.OnbuildMenu == false)////moving thing
        {
           

            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (!Input.GetKey(KeyCode.LeftShift))/////////not moving object during rotate Mode
            {
                if (Physics.Raycast(ray, out hit, 50f) && player.transform.parent.gameObject.name != "PlayerFolder" | !hit.collider.gameObject.CompareTag("Planet") )///calculating muse 3D position       hit pint postion only on planet or on nothr builded thing
                {
                    copySelectThing.transform.position = hit.point;////seting thing to position

                    //setting hight of buld tjing so it is not in the ground
                    if (hit.collider.gameObject.CompareTag("Planet"))//seting position on planets ground
                    {
                        //calculating local things position and ratio of local things position
                        Vector3 plenetCenter = hit.collider.transform.position;////seting planetCenter, thingCenter
                        Vector3 thingCenter = hit.point;
                        //calculating planet to build thing ratio
                        Vector3 thingPlanetRelativeP = new Vector3(thingCenter.x - plenetCenter.x, thingCenter.y - plenetCenter.y, thingCenter.z - plenetCenter.z);
                        float thingPlanetPfullRatio = Mathf.Abs(thingPlanetRelativeP.x) + Mathf.Abs(thingPlanetRelativeP.y) + Mathf.Abs(thingPlanetRelativeP.z);//calculating ratio of thigs relative position to planet

                        Vector3 thingPlanetPRatio = new Vector3(thingPlanetRelativeP.x / thingPlanetPfullRatio, thingPlanetRelativeP.y / thingPlanetPfullRatio, thingPlanetRelativeP.z / thingPlanetPfullRatio) ;
                        //finding thingsHight
                        copySelectThing.transform.GetChild(0).GetComponent<Collider>().enabled = true;
                        Vector3 lowestPoint = copySelectThing.transform.GetChild(0).GetComponent<Collider>().ClosestPoint(plenetCenter);
                        
                        thingsHight = Vector3.Distance(thingCenter, plenetCenter) - Vector3.Distance(lowestPoint, plenetCenter);
                        //setting final variables
                        thingPlanetPRatio = thingPlanetPRatio * thingsHight;
                        copySelectThing.transform.position = new Vector3(thingCenter.x + thingPlanetPRatio.x, thingCenter.y + thingPlanetPRatio.y, thingCenter.z + thingPlanetPRatio.z); //position by adding thingCenter and thingPlanetPRatio

                        copySelectThing.transform.GetChild(0).GetComponent<Collider>().enabled = false;
                    }

                }
                else if (player.transform.parent.gameObject.name == "PlayerFolder")//////muvin rock in vacum
                {
                    Vector3 mousePos = Input.mousePosition;
                    mousePos.z = Camera.main.nearClipPlane + 10;
                    copySelectThing.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
                }
            }

            rotatingX += System.Convert.ToInt32(Input.GetKey(KeyCode.Alpha1)) * Input.GetAxis("Mouse ScrollWheel") * 20;
            rotatingY += System.Convert.ToInt32(Input.GetKey(KeyCode.Alpha2)) * Input.GetAxis("Mouse ScrollWheel") * 20;
            rotatingZ += System.Convert.ToInt32(Input.GetKey(KeyCode.Alpha3)) * Input.GetAxis("Mouse ScrollWheel") * 20;
            //rotatingX += System.Convert.ToInt32(Input.GetKey(KeyCode.Alpha1)) - System.Convert.ToInt32(Input.GetKey(KeyCode.Alpha2));
            //rotatingY += System.Convert.ToInt32(Input.GetKey(KeyCode.Alpha3)) - System.Convert.ToInt32(Input.GetKey(KeyCode.Alpha4));
            //rotatingZ += System.Convert.ToInt32(Input.GetKey(KeyCode.Alpha5)) - System.Convert.ToInt32(Input.GetKey(KeyCode.Alpha6));

            copySelectThing.transform.localEulerAngles = new Vector3(rotatingX, rotatingY, rotatingZ);
            print(copySelectThing.transform.localEulerAngles);

            copySelectThing.transform.parent = player.transform.parent;////seting thing to parent



            //if (player.transform.parent.gameObject.name != "PlayerFolder")
            
            
            

           yield return null;           
        }
        if (Player.OnbuildMenu == true)//ending function when build menu closed
        {
            buildmodeOn = false;
            Destroy(copySelectThing);
            needList.SetActive(false);
            yield break;
        }

        for (int i = 0; i < AmountResorsesToBuild.Length; i++)////ciling from type of resurse to another type of resurse 
        {
            bool HaveEnoughtResorses = false;////if sript will not say true to this variable nothing will be build

            for (int OnChild = 0; OnChild < inventory.transform.childCount; OnChild++)/////cicling on all resourses an tring to find the corect resorse
            {
                Transform Child = inventory.transform.GetChild(OnChild);
                if (Child.GetComponent<ThingData>().OreType == TypeResorsesToBuild[i])/////finding resorse
                {
                    HaveEnoughtResorses = true;
                    ////substracting number of ores from panel
                    if (Child.GetComponent<ThingData>().AmountOfOres >= AmountResorsesToBuild[i])//////finding if there is big enought number of resorses
                    {
                        Child.GetComponent<ThingData>().AmountOfOres -= AmountResorsesToBuild[i];//taking resorses

                        if (Child.GetComponent<ThingData>().AmountOfOres == 0)/////destroying panel when it hold 0 ores
                        {
                            Destroy(Child.gameObject);
                        }
                        string StringOfAmountOfOres = Child.GetComponent<ThingData>().AmountOfOres.ToString();/////seting text to amount of resorses  
                        Child.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = StringOfAmountOfOres;
                    }
                    else
                    {
                        HaveEnoughtResorses = false;
                    }
                      
                }

            }
            if (HaveEnoughtResorses == false )////////////deleting select thing and ending script when not enought resorses
            {
                buildmodeOn = false;
                Destroy(copySelectThing);
                StartCoroutine(PrintNotEnoughtMaterials());
                needList.SetActive(false);
                yield break;
            }
        }


        

        GameObject CopyThing = Instantiate(Thing);////copying thing
        CopyThing.transform.position = copySelectThing.transform.position;//setting things position
        CopyThing.transform.parent = copySelectThing.transform.parent;//setting things parent
        CopyThing.transform.eulerAngles = copySelectThing.transform.eulerAngles;//setting things rotation

        needList.SetActive(false);
        Destroy(copySelectThing);
        buildmodeOn = false;

     }


    IEnumerator PrintNotEnoughtMaterials()
    {
        ErrorMassageText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "not enough resources";
        ErrorMassageText.SetActive(true);

        yield return new WaitForSeconds(2f);

        ErrorMassageText.SetActive(false);
    }
}
