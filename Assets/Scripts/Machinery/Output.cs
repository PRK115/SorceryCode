using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Output : MonoBehaviour {

    //public bool finishOnceActivated;
    protected bool activated = false;

    public void Activate()
    {
        activated = true;
    }

    public void Deactivate()
    {
        activated = false;
    }
}
