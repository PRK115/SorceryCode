using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour {

    private Organism organism;
    private WalkAndJump walkAndJump;
    private GameStateManager manager;

    Camera cam;

    GameObject wand;
    public GameObject projectile;
    //private bool alive = true;

    private void Awake()
    {
        organism = GetComponent<Organism>();
        walkAndJump = GetComponent<WalkAndJump>();
        manager = FindObjectOfType<GameStateManager>();

        cam = FindObjectOfType<Camera>();

        wand = transform.Find("wand").gameObject;
    }

    private void Update()
    {
        if (organism.alive)
        {
            walkAndJump.Manuever(InterpretKey());

            if (Input.GetKeyDown("mouse 0"))
            {
                Vector3 mousePosition = Input.mousePosition;

                mousePosition = cam.ScreenToWorldPoint(mousePosition);

                mousePosition = new Vector3(Mathf.Round(mousePosition.x), Mathf.Round(mousePosition.y), 0);

                transform.LookAt(new Vector3(mousePosition.x, transform.position.y, 0));

                GameObject newProjectile = Instantiate(projectile, wand.transform.position, gameObject.transform.rotation);

                newProjectile.transform.position = wand.transform.position;

                newProjectile.GetComponent<Projectile>().Destination = mousePosition;
            }
        }
        else
        {
            if (manager != null)
                manager.Funeral();
        }
    }

    Direction InterpretKey()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput != 0)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (horizontalInput < 0)
                    return Direction.LeftUp;
                else
                    return Direction.RightUp;
            }
            else if (horizontalInput < 0)
            {
                return Direction.Left;
            }
            else
            {
                return Direction.Right;
            }
        }

        else if (Input.GetKeyDown(KeyCode.Space))
        {
            return Direction.Up;
        }

        else
        {
            return Direction.None;
        }



    }
}
