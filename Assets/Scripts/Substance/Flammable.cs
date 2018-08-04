using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flammable : MonoBehaviour, ISubstance {
    public enum State
    {
        Intact, BurnReady, Burning
    }

    public State state;

    public float burnDelay;
    float timeTillIgnition;
    public float burningDuration;
    float burningTimeLeft;

    private GameObject fire;
    private MeshRenderer renderer;

    public Action OnBurn;

    public void SetBurnCallback(Action action)
    {
        OnBurn = action;
    }

    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        fire = transform.Find("fire").gameObject;
    }

    private void Start()
    {
        timeTillIgnition = burnDelay;
        burningTimeLeft = burningDuration;
        state = State.Intact;
    }

    void Update()
    {
        if (state == State.BurnReady)
        {
            if (timeTillIgnition <= 0)
                state = State.Burning;
            else
                timeTillIgnition -= Time.deltaTime;
        }
        else if (state == State.Burning)
        {
            OnBurn();

            if (burningTimeLeft <= 0)
                gameObject.SetActive(false);
            else
                burningTimeLeft -= Time.deltaTime;

            // TODO: 잿더미 이펙트 넣기
            if (burningTimeLeft < 1)
                renderer.enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Flammable flammable = other.GetComponent<Flammable>();
        if (flammable != null)
        {
            if (flammable.state == State.Burning && flammable.state == State.Intact)
            {
                state = State.BurnReady;
                fire.SetActive(true);
            }
        }
    }
}
