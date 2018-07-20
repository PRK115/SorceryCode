using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Substance : MonoBehaviour {
    public enum SubstanceState { intact, burnDelay, burning, electrified };
    protected SubstanceState currentState;
    public SubstanceState CurrentState { get { return currentState; } }

    //protected ArrayList powerSupplies = new ArrayList();

    protected void FixedUpdate()
    {
        substanceStateBehaviour(currentState);
    }
    protected void substanceStateBehaviour(SubstanceState state)
    {
        switch(currentState)
        {
            case SubstanceState.intact:
                intactBehaviour();
                break;

            case SubstanceState.burnDelay:
                burnDelayBehaviour();
                break;

            case SubstanceState.burning:
                burningBehaviour();
                break;

            case SubstanceState.electrified:
                electrifiedBehaviour();
                break;
        }
    }
    protected virtual void intactBehaviour() { }
    protected virtual void burnDelayBehaviour() { }
    protected virtual void burningBehaviour() { }
    protected virtual void electrifiedBehaviour() { }

}
