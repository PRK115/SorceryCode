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

    public bool indestructable;
    public float burnDelay;
    float timeTillIgnition;
    public float burningDuration;
    float burningTimeLeft;

    private GameObject fire;
    private MeshRenderer meshRenderer;

    public Action OnBurn;

    public void SetBurnCallback(Action action)
    {
        OnBurn = action;
    }

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        fire = transform.Find("fire").gameObject;
    }

    private void Start()
    {
        timeTillIgnition = burnDelay;
        burningTimeLeft = burningDuration;
    }

    void Update()
    {
        if (state == State.BurnReady)
        {
            if (timeTillIgnition <= 0)
            {
                state = State.Burning;
                fire.SetActive(true);
            }
            else
                timeTillIgnition -= Time.deltaTime;
        }
        else if (state == State.Burning)
        {
            if(OnBurn != null)
                OnBurn();

            if (!indestructable)
            {
                if (burningTimeLeft <= 0)
                    gameObject.SetActive(false);
                else
                    burningTimeLeft -= Time.deltaTime;

                // TODO: 잿더미 이펙트 넣기
                if (burningTimeLeft < 1 && meshRenderer != null)
                {
                    meshRenderer.enabled = false;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Flammable flammable = other.GetComponent<Flammable>();
        if (flammable != null)
        {
            if (flammable.state == State.Burning)
            {
                state = State.BurnReady;
                fire.SetActive(true);
            }
        }
    }
}
