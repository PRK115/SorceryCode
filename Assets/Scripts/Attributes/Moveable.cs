using UnityEngine;

public class Moveable : MonoBehaviour, Attribute
{
    public float XTendency { get; set; }
    public float YTendency { get; set; }

    Rigidbody rb;
    ContactDetector cd;

    //public bool Gravitated { private get; set; } = true;
    //float g = 9.8f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cd = GetComponent<ContactDetector>();
    }

    // Update is called once per frame
    void Update()
    {

        if (rb.isKinematic)
        {
            Debug.Log($"{XTendency}");
            if ((XTendency > 0 && !cd.rightBlocked) || (XTendency < 0 && !cd.leftBlocked))
            {
                transform.Translate(Vector3.right * XTendency * Time.deltaTime, Space.World);
            }
            else
            {
                XTendency = 0;
                transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y, 0);
            }

            if ((YTendency > 0 && !cd.upBlocked) || (YTendency < 0 && !cd.downBlocked))
            {
                transform.Translate(Vector3.up * YTendency * Time.deltaTime, Space.World);
            }
            else
            {
                YTendency = 0;
                transform.position = new Vector3(transform.position.x, Mathf.Round(transform.position.y), 0);
            }
        }

        else if(rb.velocity == Vector3.zero)
        {
            transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);
            rb.isKinematic = true;
        }

        else if (rb.velocity.y < 0 && cd.downBlocked)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Round(transform.position.y), 0);
            rb.isKinematic = true;
        }
    }

}