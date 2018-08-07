using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour {

    GameObject consumedEffect;
    Action ConsumedBehaviour;
    bool consumed;

    IConsumable consumableComponent;

    private void Awake()
    {
        consumableComponent = GetComponent<IConsumable>();
        consumedEffect = transform.Find("Consumed").gameObject as GameObject;
        this.ConsumedBehaviour = consumableComponent.ConsumedBehaviour;
    }

    private void Update()
    {
        if(consumed)
        {
            if (ConsumedBehaviour != null)
                ConsumedBehaviour();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            consumed = true;
            if (consumedEffect != null)
                consumedEffect.SetActive(true);
        }
    }
}
