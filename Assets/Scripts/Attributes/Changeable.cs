﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Changeable : MonoBehaviour, Attribute
{
    public bool Resizable;
    public bool big;
    public bool changing;

    Moveable moveable;
    ContactDetector cd;
    //Rigidbody rb;

    private void Awake()
    {
        //rb = GetComponent<Rigidbody>();
        cd = GetComponent<ContactDetector>();
        moveable = GetComponent<Moveable>();
    }

    public bool IsConfined()
    {
        return (cd.rightBlocked && cd.leftBlocked) || (cd.upBlocked && cd.downBlocked);
    }

    public Vector3 BePushed(Vector3 position)
    {
        if (cd.rightBlocked)
        {
            moveable.XTendency = -1f;
            position += Vector3.left;
        }
        else if (cd.leftBlocked)
        {
            moveable.XTendency = 1f;
            position += Vector3.right;
        }

        if (cd.upBlocked)
        {
            moveable.YTendency = -1f;
            position += Vector3.down;
        }
        else if (cd.downBlocked)
        {
            moveable.YTendency = 1f;
            position += Vector3.up;
        }

        return position;
    }

    public void AdjustPosition()
    {
        moveable.XTendency = moveable.YTendency = 0;
        transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);
        moveable.Gravitate();
        //Debug.Log($"{moveable.XTendency} {moveable.YTendency} {rb.isKinematic}");
    }
}