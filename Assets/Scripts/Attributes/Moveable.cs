using UnityEngine;

public class Moveable : MonoBehaviour, Attribute
{
    public float XTendency { private get; set; }

    bool rightBlocked;
    bool leftBlocked;

    public float YTendency { private get; set; }

    bool upBlocked;
    bool downBlocked;

    public bool Gravitated { private get; set; } = true;
    float g = 9.8f;

    // Update is called once per frame
    void Update()
    {
        if ((XTendency > 0 && !rightBlocked) || (XTendency < 0 && !leftBlocked))
        {
            transform.Translate(Vector3.right * XTendency * Time.deltaTime, Space.World);
        }

        if (Gravitated)
        {
            if (!downBlocked)
            {
                YTendency -= g * Time.deltaTime;
            }
            else
            {
                YTendency = 0;
            }
        }

        if ((YTendency > 0 && !upBlocked) || (YTendency < 0 && !downBlocked))
        {
            transform.Translate(Vector3.up * YTendency * Time.deltaTime, Space.World);
        }

        if (Input.GetKey(KeyCode.B))
        {
            Gravitated = false;
            XTendency = 2;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] c = collision.contacts;
        int rightSum = 0;
        int leftSum = 0;
        int upSum = 0;
        int downSum = 0;
        for(int i = 0; i < c.Length; i++)
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
        if (rightSum >= 4)
            rightBlocked = true;
        if (downSum >= 4)
            downBlocked = true;
        if (leftSum >= 4)
            leftBlocked = true;
        if (upSum >= 4)
            upBlocked = true;

        if(leftBlocked || rightBlocked)
        {
            transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y, 0);
        }

        if (upBlocked || downBlocked)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Round(transform.position.y), 0);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        downBlocked = upBlocked = leftBlocked = rightBlocked = false;
    }
}