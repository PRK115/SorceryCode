using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour, ISubstance
{
    public enum State
    {
        Intact, Electrified
    }

    public State state;

    public bool source;

    public float electricityDuration;
    float electricityTimeLeft = 0;

    int distanceFromSupply = 0;
    Conductor predecessor;
    bool supplied = false;

    private GameObject spark;

    private Action OnElectrify;

    //public void SetElectrifyCallback(Action action)
    //{
    //    action = OnElectrify;
    //}
    void Awake()
    {
        spark = transform.Find("spark").gameObject;
    }

    private void Start()
    {
        if(source)
        {
            Electrify(this, -1);
        }
    }

    void Update()
    {
        if (state == State.Electrified)
        {
            if (electricityTimeLeft <= 0)
            {
                distanceFromSupply = 0;
                spark.SetActive(false);
                state = State.Intact;
            }
            else
            {
                if (!supplied)
                {
                    electricityTimeLeft -= Time.deltaTime;
                }
                if (predecessor == null || predecessor.enabled == false)
                {
                    supplied = false;
                }
            }
        }
        
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "PowerSupply")
    //        Electrify(0);
    //}

    protected void OnTriggerStay(Collider other)
    {
        if (state == State.Electrified)
        {
            Conductor conductor = other.GetComponent<Conductor>();
            if (conductor != null)
            {
                if (!supplied)
                    conductor.Disconnect(distanceFromSupply);

                else if (conductor.state == State.Intact || conductor.distanceFromSupply > distanceFromSupply + 1)
                    conductor.Electrify(GetComponent<Conductor>(), distanceFromSupply);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Conductor conductor = other.GetComponent<Conductor>();
        if (conductor == predecessor)
        {
            predecessor = null;
        }
        //else if (conductor != null && conductor.distanceFromSupply < distanceFromSupply)
        //{
        //    supplied = false;
        //}
    }

    public void Electrify(Conductor predecessor, int previousDistanceFromSupply)
    {
        this.predecessor = predecessor;
        supplied = true;
        electricityTimeLeft = electricityDuration;
        state = State.Electrified;
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
