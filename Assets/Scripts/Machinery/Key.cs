using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Consumable))]
public class Key : MonoBehaviour, IConsumable {

    private IToggleable lockedDoor;

    private void Awake()
    {
        lockedDoor = GetComponentInParent<IToggleable>();
    }

    public void ConsumedBehaviour()
    {
        lockedDoor.Toggle(true);
        Destroy(gameObject);
    }
}