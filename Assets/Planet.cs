using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    
    public float PlanetComplexity;
    public float Intensity;

    public GameObject Ore;
    public float Health;
    public float MaxHealth;
    public float StartRockSize;
    public float MaxHealthToNewOre;
    public float HealthToNewOre;

    void FixedUpdate()
    {
        if (Player.pouse == true)
        {
            if (PlanetComplexity != 0)
            {
                transform.eulerAngles += new Vector3(0, 0.01f, 0);
            }                
        }
    }
}
