﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : Substance
{

    public float electricityDuration;
    float electricityTimeLeft = 0;

    int distanceFromSupply = 0;
    GameObject predecessor;
    bool supplied = false;

    public GameObject spark;


    protected override void electrifiedBehaviour()
    {
        if (electricityTimeLeft <= 0)
        {
            distanceFromSupply = 0;
            spark.SetActive(false);
            currentState = SubstanceState.intact;
        }

        else
        {
            if(!supplied)
                electricityTimeLeft -= Time.deltaTime;
            if (predecessor == null)
                supplied = false;
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "PowerSupply")
    //        Electrify(0);
    //}

    protected void OnTriggerStay(Collider other)
    {
        switch (currentState)
        {
            case SubstanceState.electrified:
                if (other.gameObject.GetComponent<Conductor>() != null)
                {
                    if (!supplied)
                        other.gameObject.GetComponent<Conductor>().Disconnect(distanceFromSupply);

                    else if (other.gameObject.GetComponent<Conductor>().distanceFromSupply == 0 || other.gameObject.GetComponent<Conductor>().distanceFromSupply > distanceFromSupply + 1)
                        other.gameObject.GetComponent<Conductor>().Electrify(gameObject, distanceFromSupply);
                }
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == predecessor)
        {
            predecessor = null;
        }

        else if (other.gameObject.GetComponent<Conductor>() != null && other.gameObject.GetComponent<Conductor>().distanceFromSupply < distanceFromSupply)
            supplied = false;
            
    }

    public void Electrify(GameObject predecessor, int previousDistanceFromSupply)
    {
        this.predecessor = predecessor;
        supplied = true;
        electricityTimeLeft = electricityDuration;
        currentState = SubstanceState.electrified;
        distanceFromSupply = previousDistanceFromSupply + 1;
        spark.SetActive(true);
    }

    public void Disconnect(int previousDistanceFromSupply)
    {
        if(distanceFromSupply > previousDistanceFromSupply)
        {
            supplied = false;
        }
    }
}
