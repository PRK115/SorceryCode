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
    public bool changing;

    BoxCollider boxCollider;

    Moveable moveable;
    ContactDetector cd;
    //Rigidbody rb;

    AudioSource sound;
    public AudioClip shrink;
    public AudioClip swell;

    private void Awake()
    {
        //rb = GetComponent<Rigidbody>();
        cd = GetComponent<ContactDetector>();
        moveable = GetComponent<Moveable>();
        sound = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider>();
        if(boxCollider != null)
            boxCollider.size = big ? new Vector3(0.97f, 0.97f, 1f) : new Vector3(0.9f, 0.9f, 1f);
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

    public void ShrinkSound()
    {
        sound.clip = shrink;
        sound.Play();
    }

    public void SwellSound()
    {
        sound.clip = swell;
        sound.Play();
    }
}