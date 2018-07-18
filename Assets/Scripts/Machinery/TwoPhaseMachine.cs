using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TwoPhaseMachine : Output {

    protected enum Phase { original, changing, second, returning }
    protected Phase currentPhase;

    protected void Update()
    {
        switch (currentPhase)
        {
            case Phase.original:
                if (activated)
                    currentPhase = Phase.changing;
                break;
            case Phase.changing:
                PhaseChange();
                break;
            case Phase.second:
                if (!activated)
                    currentPhase = Phase.returning;
                break;
            case Phase.returning:
                Return();
                break;
        }
    }

    public float phaseChangeTime;
    public float returnTime;

    protected abstract void PhaseChange();
    protected abstract void Return();

}
