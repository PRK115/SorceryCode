using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoPhaseTransition : MonoBehaviour, IToggleable
{
    public TwoPhaseMachine machine;

    Vector3 originalPosition;

    public Vector3 transition;
    private Vector3 currentTransition;

    void Start()
    {
        originalPosition = transform.position;
    }

    public void Toggle(bool on)
    {
        machine.Activate(on);
    }

    public void PhaseChange()
    {
        currentTransition = transform.position - originalPosition;
        if (currentTransition.magnitude >= transition.magnitude)
            machine.currentPhase = TwoPhaseMachine.Phase.Second;

        else
            transform.Translate(transition * Time.deltaTime / machine.phaseChangeTime);
    }

    public void Return()
    {
        currentTransition = transform.position - originalPosition;
        if (Vector3.Dot(currentTransition, transition) <= 0)
        {
            transform.position = originalPosition;
            machine.currentPhase = TwoPhaseMachine.Phase.Original;
        }

        else
            transform.Translate(-transition * Time.deltaTime / machine.returnTime);
    }
}
