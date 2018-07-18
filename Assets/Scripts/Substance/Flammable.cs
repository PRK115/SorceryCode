using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flammable : Substance {
    public float burnDelay;
    float timeTillIgnition;
    public float burningDuration;
    float burningTimeLeft;

    public GameObject fire;

    private void Start()
    {
        timeTillIgnition = burnDelay;
        burningTimeLeft = burningDuration;
        currentState = SubstanceState.intact;
    }

    protected override void burnDelayBehaviour()
    {
        if (timeTillIgnition <= 0)
            currentState = SubstanceState.burning;
        else
            timeTillIgnition -= Time.deltaTime;
    }

    override protected void burningBehaviour()
    {
        if (burningTimeLeft <= 0)
            Destroy(gameObject);
        else
        {
            burningTimeLeft -= Time.deltaTime;
        }
        if (burningTimeLeft < 1)
            GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.GetComponent<Substance>() != null)
            if (other.gameObject.GetComponent<Substance>().CurrentState == SubstanceState.burning && currentState == SubstanceState.intact)
            {
                currentState = SubstanceState.burnDelay;
                fire.SetActive(true);
            }
    }

    new private void Update()
    {
        base.Update();
        Debug.Log("wood " + currentState);
        Debug.Log(burningTimeLeft);
    }
}
