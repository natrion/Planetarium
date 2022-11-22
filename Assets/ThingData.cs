using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingData : MonoBehaviour
{
    
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
    public float AmountOfOres;
    public string OreType;
    /// <OreUI>
    /// /////////////////////
    /// <PlanetRotating>
    /// 
    void FixedUpdate()
    {
        if (Player.pouse == true & Player.OnInventory == true)
        {
            if (PlanetComplexity != 0)
            {
                transform.eulerAngles += new Vector3(0, 0.01f, 0);
            }                
        }
    }
}
