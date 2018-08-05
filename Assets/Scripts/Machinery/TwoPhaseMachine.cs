using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TwoPhaseMachine {

    public enum Phase { Original, Changing, Second, Returning }
    public Phase currentPhase;

    public float phaseChangeTime;
    public float returnTime;

    public bool activated = false;

    private Action PhaseChange;
    private Action Return;

    public void SetCallbacks(Action PhaseChange, Action Return)
    {
        this.PhaseChange = PhaseChange;
        this.Return = Return;
    }
    
    public void Activate(bool activated)
    {
        this.activated = activated;
    }

    public void Update()
    {
        switch (currentPhase)
        {
            case Phase.Original:
                if (activated)
                    currentPhase = Phase.Changing;
                break;
            case Phase.Changing:
                PhaseChange();
                break;
            case Phase.Second:
                if (!activated)
                    currentPhase = Phase.Returning;
                break;
            case Phase.Returning:
                Return();
                break;
        }
    }
}
