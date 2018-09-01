using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Changeable : MonoBehaviour, Attribute
{
    public bool Resizable;
    public bool big;

    Moveable moveable;
    ContactDetector cd;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cd = GetComponent<ContactDetector>();
        moveable = GetComponent<Moveable>();
    }

    public void BePushed()
    {
        if (cd.rightBlocked)
            moveable.XTendency = -1;
        else if (cd.leftBlocked)
            moveable.XTendency = 1;

        if (cd.upBlocked)
            moveable.YTendency = -1;
        else if (cd.downBlocked)
            moveable.YTendency = 1;

        if (cd.downBlocked || cd.upBlocked || cd.rightBlocked || cd.leftBlocked)
            rb.isKinematic = true;
    }

    public void AdjustPosition()
    {
        moveable.XTendency = moveable.YTendency = 0;
        if (cd.rightBlocked || cd.leftBlocked)
        {
            transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y, 0);
        }
        rb.isKinematic = false;
        //Debug.Log($"{moveable.XTendency} {moveable.YTendency} {rb.isKinematic}");
    }
}