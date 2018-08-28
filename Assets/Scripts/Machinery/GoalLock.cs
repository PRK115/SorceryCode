using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalLock : MonoBehaviour, IToggleable {

    Goal goal;
    bool activated;

    private void Awake()
    {
        goal = FindObjectOfType<Goal>();
        goal.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && activated)
        {
            Open();
        }
    }

    void Open()
    {
        goal.gameObject.SetActive(true);
    }

    public void Toggle(bool on)
    {
        activated = on;
    }
}
