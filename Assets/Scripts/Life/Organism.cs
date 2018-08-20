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
    private bool physicallyDamaged;
    private GameObject blood;

    private void Awake()
    {
        conductor = GetComponent<Conductor>();
        flammable = GetComponent<Flammable>();
        blood = transform.Find("blood").gameObject;
    }

    private void Start()
    {
        timeTillDeath = deathTime;
    }

    void Update()
    {
        if (conductor.state == Conductor.State.Electrified || flammable.state == Flammable.State.Burning || physicallyDamaged)
        {
            alive = false;
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

    public void PhysicalDamage()
    {
        blood.SetActive(true);
        physicallyDamaged = true;
    }
}
