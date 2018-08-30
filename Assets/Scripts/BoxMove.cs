using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMove : MonoBehaviour {

    Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("b"))
        {
            rb.useGravity = false;
            rb.velocity = Vector3.up * 5;
        }

        if(Input.GetKeyUp("b"))
        {
            rb.velocity = Vector3.zero;
        }

        //Debug.Log(GetComponent<Rigidbody>().velocity);
	}
}
