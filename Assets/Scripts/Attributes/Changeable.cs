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
        bool result = false;
        result = (cd.upBlocked&&cd.downBlocked)||(cd.rightBlocked&&cd.leftBlocked);
        result = result || (cd.checkResult[0]&&cd.checkResult[2]&&cd.checkResult[4]&&cd.checkResult[6]);
        return result;
    }

    public Vector3 BePushed(Vector3 position)
    {
        bool basicBlocked = cd.rightBlocked||cd.leftBlocked||cd.upBlocked||cd.downBlocked;
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
        if(!basicBlocked)
        {
            Vector3 Xmove = new Vector3(0,0,0);
            Vector3 Ymove = new Vector3(0,0,0);
            float XTendency = 0f; // moveable의 XTendency와는 다른 변수임. 여기서만 사용되는 지역변수
            float YTendency = 0f;
            
            if(cd.checkResult[0])
            {
                XTendency += 1f;
                YTendency += -1f;
                Xmove+=Vector3.right;
                Ymove+=Vector3.down;
            }
            else if(cd.checkResult[4])
            {
                XTendency += -1f;
                YTendency += 1f;
                Xmove+=Vector3.left;
                Ymove+=Vector3.up;
            }
            if(cd.checkResult[2])
            {
                XTendency += -1f;
                YTendency += -1f;
                Xmove+=Vector3.left;
                Ymove+=Vector3.down;
            }
            else if(cd.checkResult[6])
            {
                XTendency += 1f;
                YTendency += 1f;
                Xmove+=Vector3.right;
                Ymove+=Vector3.up;
            }
            moveable.XTendency+= XTendency/Math.Abs(XTendency);
            moveable.YTendency+= YTendency/Math.Abs(YTendency);

            return (position+Xmove.normalized+Ymove.normalized);
        }
        else
        {
        return position;
        }
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