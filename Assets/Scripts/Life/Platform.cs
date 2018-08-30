using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    Vector3 offset;
    GameObject top;
    public GameObject Top
    {
        get
        {
            return top;
        }
        set
        {
            top = value; offset = transform.position - top.transform.position;
        }
    }

    private void Update()
    {
        if(Top != null)
        gameObject.transform.position = Top.transform.position + offset;
    }
}
