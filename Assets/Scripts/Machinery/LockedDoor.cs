using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : Output {

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if(other.gameObject.name == "player" && activated)
        {
            Destroy(gameObject);
        }
    }

}
