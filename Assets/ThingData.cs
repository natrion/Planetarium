using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingData : MonoBehaviour
{
    private float  star_light_itensity;
    public Transform PlayerCamera ;
    public GameObject ObjectPlayer;
    /// <RandomData>
    /// ////////////////////////////////////////////////
    /// <PlanetData>
    public float PlanetComplexity;
    public float Intensity;

    public GameObject PlanetOre1;
    public GameObject PlanetOre2;
    public float PlanetMaxHealthToNewOre1;
    public float PlanetMaxHealthToNewOre2;
    public float HealthToNewOre1;
    public float HealthToNewOre2;
    /// <PlanetData>
    /// ////////////////////////////////////////////////
    /// <RockData>
    public GameObject Ore;
    public float Health;
    public float MaxHealth;
    public float StartRockSize;
    public float MaxHealthToNewOre;
    public float HealthToNewOre;
    /// <RockData>
    /// /////////////////////
    /// <OreData> 
    public int OreAmountInrock;
    public GameObject OreUI;
    /// <OreData>
    /// /////////////////////
    /// <OreUI>
    public int AmountOfOresToDelete;
    public int AmountOfOres;
    public string OreType;
    public int OreNumber;
    /// <OreUI>
    /// /////////////////////
    /// <PlanetRotating>
 
    void Start()
    {
        PlayerCamera = transform.parent.parent.GetChild(0).GetChild(0);

        //finding star istensity
        if (transform.childCount != 0)//looking if object is star
        {
            GameObject star_light = transform.GetChild(0).gameObject;
            if (star_light.CompareTag("star light"))//looking if object is star
            {
                star_light_itensity = star_light.transform.GetComponent<Light>().intensity;//sting star start light intensity
            }           
        }
    }

    void FixedUpdate()
    {
        if (Player.pouse == true & Player.OnInventory == true)////if game is not pouse
        {
            if (PlanetComplexity != 0)/////finding  if it is not a rock
            {
                transform.eulerAngles += new Vector3(0, 0.01f, 0);
            }
            /// <PlanetRotating>
            /// /////////////////////
            /// <star light>     
            if (transform.childCount != 0)//looking if object is star
            {
                GameObject star_light = transform.GetChild(0).gameObject;
                if (star_light.CompareTag("star light"))//looking if object is star
                {
                    Vector3 PlayerCameraRounded = new Vector3(Mathf.Round(PlayerCamera.position.x / 100) * 100,    //Rounding camera postion to 100
                                                              Mathf.Round(PlayerCamera.position.y / 100) * 100,
                                                              Mathf.Round(PlayerCamera.position.z / 100) * 100);
                    star_light.transform.LookAt(PlayerCameraRounded); //turning light to player

                    float starDistance = Vector3.Distance(PlayerCamera.position, star_light.transform.position);//making Light less efective far away

                    star_light.transform.GetComponent<Light>().intensity = star_light_itensity / (starDistance / 2000);
                }
            }
        }

    }
}
