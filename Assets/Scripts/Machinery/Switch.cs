using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {

    Output[] outputs;

    private void Start()
    {
        outputs = GetComponentsInChildren<Output>();
    }

    private void OnTriggerEnter(Collider other)
    {
        for(int i = 0; i < outputs.GetLength(0); i++)
        {
            outputs[i].Activate();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < outputs.GetLength(0); i++)
        {
            outputs[i].Deactivate();
        }
    }
}
