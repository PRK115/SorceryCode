using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour, IToggleable
{
    public bool activated;

    public void Toggle(bool on)
    {
        activated = on;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if(collision.gameObject.tag == "Player" && activated)
        {
            Destroy(gameObject);
        }
    }

}