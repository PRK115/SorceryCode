using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Conductor), typeof(Flammable))]
public class Organism : MonoBehaviour {

    public float deathTime;
    float timeTillDeath;

    public bool alive = true;

    private Conductor conductor;
    private Flammable flammable;

    private void Awake()
    {
        conductor = GetComponent<Conductor>();
        flammable = GetComponent<Flammable>();
    }

    private void Start()
    {
        timeTillDeath = deathTime;
    }

    void Update()
    {
        if (conductor.state == Conductor.State.Electrified || flammable.state == Flammable.State.Burning)
        {
            if (timeTillDeath < 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                timeTillDeath -= Time.deltaTime;
            }
        }
    }
}
