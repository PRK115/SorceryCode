using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpManager : MonoBehaviour {

    public GameObject Help;
	
	private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            Help.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Help.SetActive(false);
        }
    }
}
