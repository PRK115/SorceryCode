using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

    IToggleable[] outputs;

    private void Start()
    {
        outputs = GetComponentsInChildren<IToggleable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        for(int i = 0; i < outputs.GetLength(0); i++)
        {
            outputs[i].Toggle(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < outputs.GetLength(0); i++)
        {
            outputs[i].Toggle(false);
        }
    }
}
