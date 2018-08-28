using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class blink : MonoBehaviour {

    bool black = true;

	// Use this for initialization
	void Start () {
        StartCoroutine(ColorChange());
	}

    IEnumerator ColorChange()
    {
        while (true)
        {
            if (black)
            {
                gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                black = false;
            }
            else
            {
                gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 255);
                black = true;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
