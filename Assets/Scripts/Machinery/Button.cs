using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

    List<Collider> pressingColliders = new List<Collider>();

    private IToggleable[] outputs;

    //AudioSource sound;

    private void Awake()
    {
        //sound = GetComponent<AudioSource>();
    }

    private void Start()
    {
        outputs = GetComponentsInChildren<IToggleable>();
    }

    private void Update()
    {
        List<Collider> futileColliders  = new List<Collider>();
        foreach (Collider c in pressingColliders)
        {
            if (c == null)
            {
                futileColliders.Add(c);
                continue;
            }
            if(c.gameObject.activeInHierarchy == false)
            {
                futileColliders.Add(c);
            }
        }

        foreach(Collider c in futileColliders)
        {
            pressingColliders.Remove(c);
        }

        if(Input.GetKey(KeyCode.K))
        {
            Debug.Log(pressingColliders.Count);
        }
        if (pressingColliders.Count == 0)
        {
            for (int i = 0; i < outputs.GetLength(0); i++)
            {
                outputs[i].Toggle(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        pressingColliders.Add(other);
        for (int i = 0; i < outputs.GetLength(0); i++)
        {
            outputs[i].Toggle(true);
        }
        //sound.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        pressingColliders.Remove(other);
    }
}
