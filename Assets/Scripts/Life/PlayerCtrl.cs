using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCtrl : MonoBehaviour {

    public static PlayerCtrl inst;

    private Organism organism;
    private WalkAndJump walkAndJump;
    private GameStateManager manager;

    //public Vector3 mp;

    public enum State { Walking, Weaving, Aiming, Casting, Cleared }
    private State currentState;

    Camera cam;

    GameObject wand;
    public GameObject projectile;
    Vector3 mousePosition;
    public GameObject aim;
    GameObject X;
    GameObject mouse;

    bool touchingUI;

    Animator animator;
    float exitTime = 1;

    //bool cleared;
    float goalX;
    public float GoalX { set { goalX = value; } }

    //public AudioClip runeGot;

    private void Awake()
    {
        inst = this;
        organism = GetComponent<Organism>();
        walkAndJump = GetComponent<WalkAndJump>();
        manager = FindObjectOfType<GameStateManager>();

        cam = FindObjectOfType<Camera>();

        wand = transform.Find("wand").gameObject;

        aim = GameObject.Find("Aim");
        X = aim.transform.Find("X").gameObject;
        mouse = aim.transform.Find("mouse").gameObject;

        animator = transform.Find("witch").GetComponent<Animator>();
    }

    private void Start()
    {
        SetState(State.Walking);
    }

    private void Update()
    {
        if (organism.alive)
        {
            switch(currentState)
            {
                case State.Walking:
                    walkAndJump.Manuever(InterpretKey());
                    break;

                case State.Aiming:
                    //mousePosition = Input.mousePosition;

                    //mousePosition = cam.ScreenToWorldPoint(mousePosition);

                    //Ray r = new Ray();
                    //r.origin = mousePosition;
                    //r.direction = cam.transform.forward;

                    //    mousePosition = r.GetPoint(-r.origin.z / Mathf.Cos(Vector3.Angle(r.direction, Vector3.forward * Mathf.PI / 180)));

                    var mousePos = Input.mousePosition;
                    mousePos.z = -cam.transform.position.z;
                    mousePosition = (cam.ScreenToWorldPoint(mousePos));

                    mousePosition = new Vector3(Mathf.Round(mousePosition.x), Mathf.Round(mousePosition.y), 0);

                    transform.LookAt(new Vector3(mousePosition.x, transform.position.y, 0));

                    aim.GetComponent<RectTransform>().position = cam.WorldToScreenPoint(mousePosition);

                    RaycastHit[] hits = Physics.SphereCastAll(wand.transform.position, 0.2f, mousePosition - wand.transform.position, Vector3.Distance(wand.transform.position, mousePosition), 257, QueryTriggerInteraction.Collide);
                    //Debug.DrawLine(wand.transform.position, mousePosition);

                    bool blocked = false;
                    for(int i = 0; i< hits.Length; i++)
                    {
                        Entity e = hits[i].collider.GetComponent<Entity>();
                        if (e != null && e.blockProjectiles == true)
                        {
                            blocked = true;
                            //Debug.Log(e.name);
                            break;
                        }
                    }

                    if(blocked)
                    {
                        mouse.SetActive(false);
                        X.SetActive(true);
                    }

                    else
                    {
                        mouse.SetActive(true);
                        X.SetActive(false);
                    }

                    if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
                    {
                        StartCoroutine(Cast());
                        SetState(State.Casting);
                    }

                    if (Input.GetKeyDown(KeyCode.Mouse1) && !EventSystem.current.IsPointerOverGameObject())
                    {
                        SetState(State.Walking);
                    }
                    break;

                case State.Casting:
                    break;

                case State.Cleared:
                    StartCoroutine(Clear());
                    break;
            }
        }
        else
        {
            walkAndJump.Manuever(Direction.None);
            if (manager != null)
                manager.SetState(GameStateManager.UIState.GameOver);
        }
    }

    Direction InterpretKey()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput != 0)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
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

        else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
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
        GameObject newProjectile = Instantiate(projectile, wand.transform.position, Quaternion.identity);

        Projectile p = newProjectile.GetComponent<Projectile>();

        p.Destination = mousePosition;
        if (Physics.CheckSphere(mousePosition, 0.1f))
        {
            Collider[] g = Physics.OverlapSphere(mousePosition, 0.1f);
            if (g.Length != 0)
                for (int i = 0; i < g.Length; i++)
                {
                    if (g[i].GetComponent<Entity>() != null)
                    {
                        p.Target = g[i].gameObject;
                        p.TargetOffset = mousePosition - g[i].gameObject.transform.position;
                    }
                }
        }
        yield return new WaitForSeconds(0.2f);

        SetState(State.Walking);

        yield return null;
    }

    IEnumerator Clear()
    {
        transform.LookAt(transform.position + new Vector3(0, 0, 1));
        transform.position = new Vector3(goalX, transform.position.y, transform.position.z);

        yield return new WaitForSeconds(0.5f);

        if (exitTime <= 0)
        {
            //cleared = true;
            manager.SetState(GameStateManager.UIState.StageClear);
        }
        else
        {
            transform.Translate(Vector3.forward * Time.deltaTime);
            exitTime -= Time.deltaTime;
        }
        yield return null;
    }


    public void SetState(State state)
    {
        currentState = state;
        animator.SetInteger("State", (int)state);
        if (state == State.Aiming)
        {
            aim.SetActive(true);
        }
        else
            aim.SetActive(false);
    }

    public void ToWeavingState()
    {
        SetState(State.Weaving);
    }

    public void ToWalkingState()
    {
        SetState(State.Walking);
    }
}
