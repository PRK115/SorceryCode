using UnityEngine;

public class Moveable : MonoBehaviour, Attribute
{
    public float XTendency;
    public float YTendency;

    Rigidbody rb;
    ContactDetector cd;
    Changeable changeable;

    public bool gravitated = true;
    float g = 9.8f;

    private void Awake()
    {
        changeable = GetComponent<Changeable>();
        rb = GetComponent<Rigidbody>();
        cd = GetComponent<ContactDetector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.isKinematic)
        {
            if ((YTendency > 0 && (!cd.upBlocked && !changeable.changing) || (YTendency < 0 && (!cd.downBlocked) && !changeable.changing)))
            {
                transform.Translate(Vector3.up * YTendency * Time.deltaTime, Space.World);
                //Debug.Log($"    Y {Time.deltaTime}");
            }
            else
            {
                //Debug.Log("y막힘");
                YTendency = 0;
                transform.position = new Vector3(transform.position.x, Mathf.Round(transform.position.y), 0);
                if (cd.downBlocked)
                    //rb.isKinematic = false;
                    rb.isKinematic = false;
            }

            if (!cd.downBlocked && gravitated)
            {
                YTendency -= g * Time.deltaTime;
            }

            if ((XTendency > 0 && (!cd.rightBlocked || changeable.changing) || (XTendency < 0 && (!cd.leftBlocked) || changeable.changing)))
            {
                transform.Translate(Vector3.right * XTendency * Time.deltaTime, Space.World);
                //Debug.Log($"    X {Time.deltaTime}");
            }
            else
            {
                XTendency = 0;
                transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y, 0);
            }

        }

        //else if (rb.velocity.y < 0 && cd.downBlocked)
        //{
        //    transform.position = new Vector3(transform.position.x, Mathf.Round(transform.position.y), 0);
        //    rb.isKinematic = true;
        //}
    }

    public void Gravitate()
    {
        //rb.isKinematic = false;
        gravitated = true;
    }

    public void Ungravitate()
    {
        rb.isKinematic = true;
        transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);
        gravitated = false;
    }

}