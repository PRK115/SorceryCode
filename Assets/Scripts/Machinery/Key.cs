using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    Output lockedDoor;

    private void Start()
    {
        lockedDoor = GetComponentInParent<Output>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            lockedDoor.Activate();
            Destroy(gameObject);
        }
    }
}
