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
    public float burningTimeLeft;

    private IfDetector id;

    private GameObject fire;
    private MeshRenderer meshRenderer;

    public Action OnBurn;

    AudioSource sound;

    public void SetBurnCallback(Action action)
    {
        OnBurn = action;
    }

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        fire = transform.Find("fire").gameObject;
        sound = GetComponent<AudioSource>();
        id = GetComponent<IfDetector>();
    }

    private void Start()
    {
        timeTillIgnition = burnDelay;
        burningTimeLeft = burningDuration;
    }

    void Update()
    {
        if(state == State.Intact)
        {
            List<Collider> colliders = new List<Collider>();
            if(id != null)
                colliders = id.CheckSelf(1 << 12);
            if (colliders.Exists(collider => collider.gameObject.name == "fire"))
                Ignite();
        }
        else if (state == State.BurnReady)
        {
            if (timeTillIgnition <= 0)
            {
                if(sound != null)
                {
                    sound.Play();
                }
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

                if (burningTimeLeft < 1 && meshRenderer != null)
                {
                    meshRenderer.enabled = false;
                }
            }
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if(state == State.Burning)
    //    {
    //        Flammable flammable = other.GetComponent<Flammable>();
    //        if (flammable != null)
    //        {
    //            if (flammable.state == State.Intact)
    //            {
    //                flammable.Ignite();
    //            }
    //        }

    //    }
    //}

    public void Ignite()
    {
        state = State.BurnReady;
    }
}
