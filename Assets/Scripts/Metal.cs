using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metal : Substance {

    public float electricityDuration;
    float electricityTimeLeft = 0;

    int distanceFromSupply = 0;
    bool supplied = false;  

    public GameObject spark;


    protected override void electrifiedBehaviour()
    {
        if (electricityTimeLeft < 0)
        {
            distanceFromSupply = 0;
            spark.SetActive(false);
            currentState = SubstanceState.intact;
        }
        else if(!supplied)
            electricityTimeLeft -= Time.deltaTime;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PowerSupply")
            Electrify(0);
    }

    private void OnTriggerStay(Collider other)
    {
        switch (currentState)
        {
            case SubstanceState.intact:
                break;
            case SubstanceState.electrified:
                if(other.gameObject.tag == "Conductor")
                {
                    if (other.gameObject.GetComponent<Metal>().distanceFromSupply == 0 || other.gameObject.GetComponent<Metal>().distanceFromSupply > distanceFromSupply)
                        other.gameObject.GetComponent<Metal>().Electrify(distanceFromSupply);
                }
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit");
        if (other.gameObject.tag == "PowerSupply")
        {
            distanceFromSupply = 0;
            supplied = false;
        }
    }

    public void Electrify(int previousDistanceFromSupply)
    {
        supplied = true;
        electricityTimeLeft = electricityDuration;
        currentState = SubstanceState.electrified;
        distanceFromSupply = previousDistanceFromSupply + 1;
        spark.SetActive(true);
    }
}
