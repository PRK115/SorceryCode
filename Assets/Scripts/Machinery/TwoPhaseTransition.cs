using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoPhaseTransition : MonoBehaviour, IToggleable
{
    public TwoPhaseMachine machine = new TwoPhaseMachine();

    Vector3 originalPosition;

    public Vector3 transition;

    private void Awake()
    {
        machine.SetCallbacks(PhaseChange, Return);
    }

    void Start()
    {
        originalPosition = transform.position;
        machine.TimeTillNextPhase = machine.phaseChangeTime;
    }

    private void Update()
    {
        machine.Update();
    }

    public void Toggle(bool on)
    {
        machine.Activate(on);
    }

    public void PhaseChange()
    {
        if (machine.TimeTillNextPhase <= 0)
        {
            machine.TimeTillNextPhase = machine.returnTime;
            //transform.position = originalPosition;
            machine.currentPhase = TwoPhaseMachine.Phase.Second;
        }

        else
        {
            transform.Translate(transition * Time.deltaTime / machine.phaseChangeTime);
            machine.TimeTillNextPhase -= Time.deltaTime;
        }
    }

    public void Return()
    {
        if (machine.TimeTillNextPhase <= 0)
        {
            machine.TimeTillNextPhase = machine.phaseChangeTime;
            transform.position = originalPosition;
            machine.currentPhase = TwoPhaseMachine.Phase.Original;
        }

        else
        {
            transform.Translate(-transition * Time.deltaTime / machine.returnTime);
            machine.TimeTillNextPhase -= Time.deltaTime;
        }
    }
}
