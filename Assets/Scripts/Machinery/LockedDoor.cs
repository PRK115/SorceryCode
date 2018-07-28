using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : Output {

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if(collision.gameObject.tag == "Player" && activated)
        {
            Destroy(gameObject);
        }
    }

}
