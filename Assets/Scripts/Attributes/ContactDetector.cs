using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDetector : MonoBehaviour {

    Rigidbody rb;

    public bool rightBlocked;
    public bool leftBlocked;

    public bool upBlocked;
    public bool downBlocked;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(true)
        {
            ContactPoint[] c = collision.contacts;

            int rightSum = 0;
            int leftSum = 0;
            int upSum = 0;
            int downSum = 0;
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i].normal == Vector3.right)
                    leftSum++;
                else if (c[i].normal == -Vector3.right)
                    rightSum++;
                else if (c[i].normal == Vector3.up)
                    downSum++;
                else if (c[i].normal == -Vector3.up)
                    upSum++;
            }
            if (rightSum >= 3)
                rightBlocked = true;
            else
                rightBlocked = false;

            if (downSum >= 3)
                downBlocked = true;
            else
                downBlocked = false;

            if (leftSum >= 3)
                leftBlocked = true;
            else
                leftBlocked = false;

            if (upSum >= 3)
                upBlocked = true;
            else
                upBlocked = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        downBlocked = upBlocked = leftBlocked = rightBlocked = false;
    }
}
