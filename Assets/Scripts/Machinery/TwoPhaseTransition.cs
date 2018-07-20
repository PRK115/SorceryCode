using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoPhaseTransition : TwoPhaseMachine
{

    Vector3 originalPosition;

    public Vector3 transition;
    private Vector3 currentTransition;

    void Start()
    {
        originalPosition = transform.position;
    }


    protected override void PhaseChange()
    {
        currentTransition = transform.position - originalPosition;
        if (currentTransition.magnitude >= transition.magnitude)
            currentPhase = Phase.second;

        else
            transform.Translate(transition * Time.deltaTime / phaseChangeTime);
    }

    protected override void Return()
    {
        currentTransition = transform.position - originalPosition;
        if (Vector3.Dot(currentTransition, transition) <= 0)
        {
            transform.position = originalPosition;
            currentPhase = Phase.original;
        }

        else
            transform.Translate(-transition * Time.deltaTime / returnTime);
    }
}
