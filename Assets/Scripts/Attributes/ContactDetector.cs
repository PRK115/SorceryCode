using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDetector : MonoBehaviour {

    //Rigidbody rb;

    public bool rightBlocked;
    public bool leftBlocked;

    public bool upBlocked;
    public bool downBlocked;

    //private void Awake()
    //{
    //    rb = GetComponent<Rigidbody>();
    //}

    private void OnCollisionEnter(Collision collision)
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
        if (rightSum >= 1)
            rightBlocked = true;
        else
            rightBlocked = false;

        if (downSum >= 1)
            downBlocked = true;
        else
            downBlocked = false;

        if (leftSum >= 1)
            leftBlocked = true;
        else
            leftBlocked = false;

        if (upSum >= 1)
            upBlocked = true;
        else
            upBlocked = false;
    }

    public void CheckSurroundingObstacles()
    {
        int layerMask = ~ (9 << 9);
        upBlocked = Physics.CheckBox(transform.position + Vector3.up * 0.8f, new Vector3(0.3f, 0.1f, 0.5f), Quaternion.identity, layerMask);
        downBlocked = Physics.CheckBox(transform.position + Vector3.down * 0.8f, new Vector3(0.3f, 0.1f, 0.5f), Quaternion.identity, layerMask);
        leftBlocked = Physics.CheckBox(transform.position + Vector3.left * 0.8f, new Vector3(0.1f, 0.3f, 0.5f), Quaternion.identity, layerMask);
        rightBlocked = Physics.CheckBox(transform.position + Vector3.right * 0.8f, new Vector3(0.1f, 0.3f, 0.5f), Quaternion.identity, layerMask);
        Collider[] c = Physics.OverlapBox(transform.position + Vector3.up * 0.8f, new Vector3(0.3f, 0.1f, 0.5f), Quaternion.identity, layerMask);
        for (int i = 0; i < c.Length; i++)
        {
            Debug.Log(c[i].name);
        }
        //Debug.Log($"{upBlocked} {downBlocked} {leftBlocked} {rightBlocked}");
    }

    private void OnCollisionExit(Collision collision)
    {
        downBlocked = upBlocked = leftBlocked = rightBlocked = false;
    }

    public void IndicateBlocked()
    {
        Debug.Log($"{upBlocked} {downBlocked} {leftBlocked} {rightBlocked}");
    }
}
