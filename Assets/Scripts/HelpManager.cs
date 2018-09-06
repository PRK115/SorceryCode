using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpManager : MonoBehaviour {

    private GameStateManager gsm;

    public GameObject tutorialPanel;

    //public string title;
    //public string contents;

    private void Awake()
    {
        gsm = FindObjectOfType<GameStateManager>();
        tutorialPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            gsm.OpenTutorialPanel(tutorialPanel);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            gsm.CloseTutorialPanel(tutorialPanel);
        }
    }
}
