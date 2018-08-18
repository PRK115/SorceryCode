using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour {

    private Organism organism;
    private WalkAndJump walkAndJump;
    private GameStateManager manager;

    public enum State { Walking, Weaving, Casting, Cleared }
    private State currentState;

    Camera cam;

    GameObject wand;
    public GameObject projectile;
    //private bool alive = true;

    Animator animator;
    float exitTime = 1;

    bool cleared;

    private void Awake()
    {
        organism = GetComponent<Organism>();
        walkAndJump = GetComponent<WalkAndJump>();
        manager = FindObjectOfType<GameStateManager>();

        cam = FindObjectOfType<Camera>();

        wand = transform.Find("wand").gameObject;

        animator = transform.Find("witch").GetComponent<Animator>();
    }

    private void Update()
    {
        if (organism.alive)
        {
            switch(currentState)
            {
                case State.Walking:
                    walkAndJump.Manuever(InterpretKey());
                    if (Input.GetKeyDown("mouse 0"))
                    {
                        SetState(State.Casting);
                    }
                    break;

                case State.Casting:
                    StartCoroutine(Cast());
                    break;

                case State.Cleared:
                    transform.LookAt(transform.position + new Vector3(0, 0, 1));
                    if(exitTime <= 0)
                    {
                        cleared = true;
                    }
                    else
                    {
                        transform.Translate(Vector3.forward * Time.deltaTime);
                        exitTime -= Time.deltaTime;
                    }    
                    break;
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

    IEnumerator Cast()
    {
        Vector3 mousePosition = Input.mousePosition;

        mousePosition = cam.ScreenToWorldPoint(mousePosition);

        mousePosition = new Vector3(Mathf.Round(mousePosition.x), Mathf.Round(mousePosition.y), 0);

        transform.LookAt(new Vector3(mousePosition.x, transform.position.y, 0));

        GameObject newProjectile = Instantiate(projectile, wand.transform.position, gameObject.transform.rotation);

        newProjectile.transform.position = wand.transform.position;

        newProjectile.GetComponent<Projectile>().Destination = mousePosition;

        yield return new WaitForSeconds(0.2f);

        SetState(State.Walking);

        yield return null;
    }

    public void SetState(State state)
    {
        currentState = state;
        animator.SetInteger("State", (int)state);
    }
}
