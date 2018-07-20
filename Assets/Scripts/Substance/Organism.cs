using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Organism : Conductor {

    Control control;
    ManueverType manueverType;

    public GameObject fire;

    public float deathTime;
    float timeTillDeath;

    private void Start()
    {
        control = GetComponent<Control>();
        manueverType = GetComponent<ManueverType>();
        timeTillDeath = deathTime;
    }

    protected override void intactBehaviour()
    {
        manueverType.Manuever(control.Command);
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
        {
            timeTillDeath -= Time.deltaTime;
            if (timeTillDeath > 0.03 && timeTillDeath < 0.05)
                GetComponent<BoxCollider>().transform.Translate(new Vector3(0, 0, -10));
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
