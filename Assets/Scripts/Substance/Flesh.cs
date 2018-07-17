using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flesh : Substance {

    public GameObject fire;
    public GameObject spark;

    public float deathTime;
    float timeTillDeath;

    private void Start()
    {
        timeTillDeath = deathTime;
    }

    protected override void intactBehaviour()
    {
        //AliveUpdate();
    }

    protected override void burningBehaviour()
    {
        fire.SetActive(true);
        CountDown();
    }

    protected override void electrifiedBehaviour()
    {
        spark.SetActive(true);
        CountDown();
    }

    void CountDown()
    {
        if (timeTillDeath < 0)
            Destroy(gameObject);
        else
            timeTillDeath -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.GetComponent<Substance>().CurrentState == SubstanceState.burning)
        {
            currentState = SubstanceState.burning;
        }
        if (other.gameObject.GetComponent<Substance>().CurrentState == SubstanceState.electrified)
        {
            currentState = SubstanceState.electrified;
        }
    }
}
