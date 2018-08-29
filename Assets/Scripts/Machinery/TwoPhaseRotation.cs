using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoPhaseRotation : MonoBehaviour, IToggleable
{
    public TwoPhaseMachine machine;

    Quaternion originalRotation;
    public float phase2angle;
    Quaternion phase2Rotation;

    float timeTillNextPhase;

    private void Awake()
    {
        machine.SetCallbacks(PhaseChange, Return);
    }

    private void Start()
    {
        originalRotation = transform.rotation;
        phase2Rotation = Quaternion.AngleAxis(phase2angle, transform.forward);
        
        timeTillNextPhase = machine.phaseChangeTime;
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
        if (timeTillNextPhase <= 0)
        {
            transform.rotation = phase2Rotation;
            timeTillNextPhase = machine.returnTime;
            machine.currentPhase = TwoPhaseMachine.Phase.Second;
        }
        else
        {
            transform.rotation = Quaternion.Lerp(phase2Rotation, originalRotation, timeTillNextPhase/machine.phaseChangeTime);
            timeTillNextPhase -= Time.deltaTime;

        }
    }

    private void Return()
    {
        if (timeTillNextPhase <= 0)
        {
            transform.rotation = originalRotation;
            timeTillNextPhase = machine.phaseChangeTime;
            machine.currentPhase = TwoPhaseMachine.Phase.Original;
        }
        else
        {
            transform.rotation = Quaternion.Lerp(originalRotation, phase2Rotation, timeTillNextPhase / machine.returnTime);
            timeTillNextPhase -= Time.deltaTime;
        }
    }
}
