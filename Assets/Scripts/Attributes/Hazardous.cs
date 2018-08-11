using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazardous : MonoBehaviour, Attribute
{
    private void OnTriggerEnter(Collider other)
    {
        Organism flesh;
        flesh = other.GetComponent<Organism>();
        if(flesh != null)
        {
            flesh.PhysicalDamage();
        }
    }
}
