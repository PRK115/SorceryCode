using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PureEnergy : Substance {

    public float duration;
    float timeLeft;

    public SubstanceState type;
    private void Start()
    {
        timeLeft = duration;
        currentState = type;
    }

    protected override void intactBehaviour()
    {
        Destroy(gameObject);
    }

    protected override void burningBehaviour()
    {
        
    }

    protected override void electrifiedBehaviour()
    {
        
    }

    new void FixedUpdate () {
        if (timeLeft < 0)
            currentState = SubstanceState.intact;
        else
            timeLeft -= Time.deltaTime;
        base.FixedUpdate();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Conductor>() != null && currentState == SubstanceState.electrified)
        {
            other.gameObject.GetComponent<Conductor>().Electrify(gameObject, 0);
        }
    }
}
