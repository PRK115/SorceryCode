using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyOrb : Substance {

    public float duration;
    float timeLeft;
    BoxCollider elementalCollider;

    public SubstanceState type;
    private void Start()
    {
        timeLeft = duration;
        currentState = type;
        elementalCollider = GetComponent<BoxCollider>();
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
        if (timeLeft > 0.03 && timeLeft < 0.05)
            elementalCollider.center = new Vector3(elementalCollider.center.x, elementalCollider.center.y, elementalCollider.center.z - 10);
    }

    new void Update () {
        if (timeLeft < 0)
            currentState = SubstanceState.intact;
        else
            timeLeft -= Time.deltaTime;
        base.Update();
	}
}
