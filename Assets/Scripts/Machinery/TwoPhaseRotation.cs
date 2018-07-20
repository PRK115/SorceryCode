using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoPhaseRotation : TwoPhaseMachine {

    Vector3 originalDirection;
    Quaternion phase2Rotation;

    public float angle;
    float timeTillPhase2;

    private void Start()
    {
        timeTillPhase2 = phaseChangeTime;
        originalDirection = gameObject.transform.up;
        phase2Rotation = Quaternion.AngleAxis(angle,transform.forward);
    }

    protected override void PhaseChange()
    {
        if (timeTillPhase2 <= 0)
        {
            transform.rotation = phase2Rotation;
            timeTillPhase2 = phaseChangeTime;
            currentPhase = Phase.second;
        }
        else
        {
            transform.Rotate(gameObject.transform.forward, angle / phaseChangeTime * Time.deltaTime);
            timeTillPhase2 -= Time.deltaTime;
        }
    }

    protected override void Return()
    {
        if (Vector3.Angle(gameObject.transform.up.normalized, originalDirection.normalized) <= 0)
        {
            currentPhase = Phase.original;
        }
        else
            transform.Rotate(gameObject.transform.forward, -angle / phaseChangeTime * Time.deltaTime);
    }
}
