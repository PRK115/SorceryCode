using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PureEnergy : MonoBehaviour
{
    private ISubstance substance;

    public float duration;
    float timeLeft;

    private void Awake()
    {
        substance = GetComponent<ISubstance>();
        if (substance == null)
        {
            Debug.LogError("PureEnergy must have a substance as a component!");
        }
    }

    private void Start()
    {
        timeLeft = duration;

        Conductor conductor = substance as Conductor;
        if (conductor)
        {
            conductor.SetAsElectricitySource();
        }
    }

    void Update() {
        if (timeLeft < 0)
            Destroy(gameObject);
        else
            timeLeft -= Time.deltaTime;
	}
}
