using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    IToggleable lockedDoor;

    private void Awake()
    {
        lockedDoor = GetComponentInParent<IToggleable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            lockedDoor.Toggle(true);
            Destroy(gameObject);
        }
    }
}