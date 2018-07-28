using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Organism : Conductor {

    Brain brain;
    ManueverType manueverType;

    public GameObject fire;

    public float deathTime;
    float timeTillDeath;

    private void Start()
    {
        brain = GetComponent<Brain>();
        manueverType = GetComponent<ManueverType>();
        timeTillDeath = deathTime;
    }

    protected override void intactBehaviour()
    {
        manueverType.Manuever(brain.Command);
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
        brain.Death();
        if (timeTillDeath < 0)
            Destroy(gameObject);
        else
        {
            timeTillDeath -= Time.deltaTime;
        }
    }

    private new void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
        if(other.gameObject.GetComponent<Substance>() != null)
            if (other.gameObject.GetComponent<Substance>().CurrentState == SubstanceState.burning)
            {
                currentState = SubstanceState.burning;
            }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
    }
}
