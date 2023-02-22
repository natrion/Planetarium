using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityScript : MonoBehaviour
{
    ////////seting strenght of gravity
    public float Gravitystrenght = 0.25f;

    void FixedUpdate()
    {
        Transform parent = transform.parent;
        if (transform.parent.gameObject.name != "PlayerFolder")//finding if object is not on planet
        {           
            float ComplexityOfPlanet = parent.GetComponent<ThingData>().PlanetComplexity * parent.transform.localScale.x;//(mass of the planet or star)
            float distance = Vector3.Distance(parent.transform.position, transform.position);
            Vector3 direction = (parent.position - transform.position).normalized;
            ////calculating gravity
            transform.GetComponent<Rigidbody>().AddForce(direction * ((float)ComplexityOfPlanet / Mathf.Sqrt(distance) * Gravitystrenght * gameObject.GetComponent<Rigidbody>().mass));
        }        
    }
}
