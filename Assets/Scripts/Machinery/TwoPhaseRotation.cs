using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoPhaseRotation : MonoBehaviour, IToggleable
{
    public TwoPhaseMachine machine;

    Vector3 originalDirection;
    Quaternion phase2Rotation;

    public float angle;
    float timeTillPhase2;

    private void Awake()
    {
        machine.SetCallbacks(PhaseChange, Return);
    }

    private void Start()
    {
        timeTillPhase2 = machine.phaseChangeTime;
        originalDirection = gameObject.transform.up;
        phase2Rotation = Quaternion.AngleAxis(angle,transform.forward);
    }

    private void Update()
    {
        machine.Update();
    }

    public void Toggle(bool on)
    {
        machine.Activate(on);
    }

    private void PhaseChange()
    {
        if (timeTillPhase2 <= 0)
        {
            transform.rotation = phase2Rotation;
            timeTillPhase2 = machine.phaseChangeTime;
            machine.currentPhase = TwoPhaseMachine.Phase.Second;
        }
        else
        {
            transform.Rotate(gameObject.transform.forward, angle / machine.phaseChangeTime * Time.deltaTime);
            timeTillPhase2 -= Time.deltaTime;
        }
    }

    private void Return()
    {
        if (Vector3.Angle(gameObject.transform.up.normalized, originalDirection.normalized) <= 0)
        {
            machine.currentPhase = TwoPhaseMachine.Phase.Original;
        }
        else
        {
            transform.Rotate(gameObject.transform.forward, -angle / machine.phaseChangeTime * Time.deltaTime);
        }
    }
}
