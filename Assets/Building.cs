using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Building : MonoBehaviour
{
    private GameObject hitedThing;

    public float rotatingNumber = 20;

    public float gridSize;
    //builds thing hight
    private float thingsHight=1;
    //sting rotation variables
    public float rotatingX; //= System.Convert.ToInt32(Input.GetKey(KeyCode.Alpha1)) - System.Convert.ToInt32(Input.GetKey(KeyCode.Alpha2)); //setting variables for rotating thing
    public float rotatingY; //= System.Convert.ToInt32(Input.GetKey(KeyCode.Alpha3)) - System.Convert.ToInt32(Input.GetKey(KeyCode.Alpha4));
    public float rotatingZ;
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
            StartCoroutine(PlacingThing());
        }
        

    }

    /////////////function for updating cube position
     IEnumerator PlacingThing()
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
            /////////not moving object during rotate Mode
            


                ///calculating muse 3D position       hit point postion only on planet or on nother builded thing
                if (Physics.Raycast(ray, out hit, 50f) && player.transform.parent.gameObject.name != "Space" | !hit.collider.gameObject.CompareTag("Planet"))
                {
                    copySelectThing.transform.position = hit.point;////seting thing to position
                    //setting postion on nother build thing
                    if (hit.collider.gameObject.CompareTag("Builded thing"))
                    {
                         hitedThing = hit.collider.gameObject;

                        Vector3 SlectPosOnBuildedThing = (hit.point - hitedThing.transform.position);
                        float SlectPOnBTFullRatio = Mathf.Abs(SlectPosOnBuildedThing.x) + Mathf.Abs(SlectPosOnBuildedThing.y) + Mathf.Abs(SlectPosOnBuildedThing.z);
                        Vector3 SlectPOnBTRatio = SlectPosOnBuildedThing / SlectPOnBTFullRatio;
                        //if (Input.GetKey(KeyCode.LeftShift))//GridMode
                        //{
                        //    SlectPOnBTRatio =new Vector3(Mathf.Round(SlectPOnBTRatio.x), Mathf.Round(SlectPOnBTRatio.y), Mathf.Round(SlectPOnBTRatio.z));
                        //}
                        print(SlectPOnBTRatio);
                        copySelectThing.transform.GetChild(0).GetComponent<Collider>().enabled = true;
                        Vector3 ClosestPoint = copySelectThing.transform.GetChild(0).GetComponent<Collider>().ClosestPoint(hitedThing.transform.position);

                        float howClose = Vector3.Distance(copySelectThing.transform.position, ClosestPoint);
                        print(howClose);
                        copySelectThing.transform.position = SlectPOnBTRatio * howClose + hit.point; //position by adding thingCenter and thingPlanetPRatio

                        copySelectThing.transform.GetChild(0).GetComponent<Collider>().enabled = false;

                        if (Input.GetKey(KeyCode.LeftControl))//GridMode
                        {
                            //Vector3 buildedPos = hitedThing.transform.localPosition / gridSize;

                            //Vector3 buildedPosRound =new Vector3( Mathf.Round(buildedPos.x)* gridSize,
                                                               //   Mathf.Round(buildedPos.y)* gridSize,
                                                                //  Mathf.Round(buildedPos.z)* gridSize);

                        //Vector3 buildedPosRoundDifference = buildedPos * gridSize - buildedPosRound;

                            copySelectThing.transform.parent = hitedThing.transform;
                            Vector3 SlectPos = copySelectThing.transform.localPosition  / gridSize;

                            Vector3 SelectPosRound = new Vector3(Mathf.Round(SlectPos.x)* gridSize,
                                                                 Mathf.Round(SlectPos.y)* gridSize,
                                                                Mathf.Round(SlectPos.z)* gridSize);

                            copySelectThing.transform.localPosition = SelectPosRound  ;
                            copySelectThing.transform.parent = hitedThing.transform.parent;
                        }

                    }
                    //setting hight of buld tjing so it is not in the ground or Rock
                    if (player.transform.parent.gameObject.name != "Space" && hit.collider.gameObject.CompareTag("Planet") | hit.collider.gameObject.CompareTag("Rock"))//seting position on planets ground
                    {
                        //calculating local things position and ratio of local things position
                        Vector3 plenetCenter = player.transform.parent.position;////seting planetCenter, thingCenter
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
                else if (player.transform.parent.gameObject.name == "Space")//////muvin rock in vacum
                {
                    Vector3 mousePos = Input.mousePosition;
                    mousePos.z = Camera.main.nearClipPlane + 10;
                    copySelectThing.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
                }

            copySelectThing.transform.parent = player.transform.parent;////seting thing to parent

            //rotating
            copySelectThing.transform.localEulerAngles = new Vector3(0, 0, 0);
            if (copySelectThing.transform.parent.gameObject.name != "Space" & !Input.GetKey(KeyCode.LeftControl))//finfing direction facing to the planet
            {
                copySelectThing.transform.LookAt(copySelectThing.transform.parent.position);
            }else if(Input.GetKey(KeyCode.LeftControl) & hitedThing)
            {
                copySelectThing.transform.eulerAngles = hitedThing.transform.eulerAngles;
            }
            //grid mode sensitivity change
            rotatingNumber = 20;
            if (Input.GetKey(KeyCode.LeftControl))
            {
                rotatingNumber = 45;
            }

            rotatingX += System.Convert.ToInt32(Input.GetKey(KeyCode.Alpha1)) * Input.GetAxis("Mouse ScrollWheel") * rotatingNumber;//rotatting numbers
            rotatingY += System.Convert.ToInt32(Input.GetKey(KeyCode.Alpha2)) * Input.GetAxis("Mouse ScrollWheel") * rotatingNumber;
            rotatingZ += System.Convert.ToInt32(Input.GetKey(KeyCode.Alpha3)) * Input.GetAxis("Mouse ScrollWheel") * rotatingNumber;

            if (Input.GetKey(KeyCode.LeftControl))
            {
                //grid rotating
                copySelectThing.transform.localEulerAngles += new Vector3(Mathf.Round(rotatingX / 45) * 45, Mathf.Round(rotatingY / 45) * 45, Mathf.Round(rotatingZ / 45) * 45) ;
            }
            else
            {
                //normal rotating
                copySelectThing.transform.localEulerAngles += new Vector3(rotatingX, rotatingY, rotatingZ);
            }


           



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
