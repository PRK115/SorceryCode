using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDetector : MonoBehaviour {

    //Rigidbody rb;
    private Vector3[] direction = {new Vector3(-1,1,0),new Vector3(0,1,0),new Vector3(1,1,0),new Vector3(1,0,0)
                                  ,new Vector3(1,-1,0),new Vector3(0,-1,0),new Vector3(-1,-1,0),new Vector3(-1,0,0)};

                                  //막힘체크시 사용할 방향을 저장, 시계방향으로8방향을 체크하며 첫번째 방향은 11시
                                  //11시->12->1->3->5->6->7->9시 방향 순서로 저장되어 있음
    public bool[] checkResult = new bool[8];
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
        Debug.Log("check");
        int layerMask = (1 + (1<<8) + (1<< 11));
        for(int i=0;i<8;i++)
        {
            checkResult[i] = Physics.CheckBox(transform.position + direction[i] * 0.8f, new Vector3(0.3f, 0.1f, 0.5f), Quaternion.identity, layerMask);
            Debug.Log(i +" " +checkResult[i]);
        }
        upBlocked = checkResult[1];
        rightBlocked = checkResult[3];
        downBlocked = checkResult[5];
        leftBlocked = checkResult[7];
        //Collider[] c = Physics.OverlapBox(transform.position + Vector3.up * 0.8f, new Vector3(0.3f, 0.1f, 0.5f), Quaternion.identity, layerMask);
        //for (int i = 0; i < c.Length; i++)
        //{
        //    Debug.Log(c[i].name);
        //}
        Debug.Log($"{upBlocked} {downBlocked} {leftBlocked} {rightBlocked}");
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
