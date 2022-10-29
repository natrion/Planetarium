using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public float PlanetComplexity;
    void FixedUpdate()
    {
        if (Player.pouse == true)
        {          
            transform.eulerAngles += new Vector3(0, 0.01f, 0);           
        }
    }
}
