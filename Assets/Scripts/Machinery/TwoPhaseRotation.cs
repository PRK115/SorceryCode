﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoPhaseRotation : MonoBehaviour, IToggleable
{
    public TwoPhaseMachine machine;

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
            transform.Rotate(gameObject.transform.up, angle / machine.phaseChangeTime * Time.deltaTime);
            timeTillPhase2 -= Time.deltaTime;
        }
    }

    private void Return()
    {
       
    }
}
