using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MouseCtrl : MonoBehaviour
{
    WalkAndJump walkAndJump;
    CharacterController ctrl;

    Animator animator;

    public enum State
    {
        Idle,
        Run,
    }
    public State _state;

    public float movePower = 1f;
    int movementFlag = 0;
    bool isWandering = false;

    bool isInRange = false;

    public List<GameObject> targetlist = new List<GameObject>();
    public GameObject player;
    public GameObject mainTarget;

    Transform _tr;
    public Vector3 habitat;

    void Awake()
    {
        walkAndJump = GetComponent<WalkAndJump>();
        ctrl = GetComponent<CharacterController>();

        _tr = gameObject.transform;
        habitat = transform.position;

        animator = GetComponent<Animator>();

        TargetUpdate();
    }

	void Start ()
    {
        _state = State.Idle;
	}

    void TargetUpdate()
    {
        if (GameObject.FindWithTag("Player"))
        {
            player = GameObject.FindWithTag("Player");
            targetlist.Add(player);
        }
        else
        {
            player = null;
        }

        if (GameObject.FindWithTag("Life"))
        {
            targetlist.AddRange(GameObject.FindGameObjectsWithTag("Life"));
        }
        targetlist.Remove(gameObject);
    }

    void Move()
    {
        if (movementFlag == 1)
        {
            walkAndJump.Manuever(Direction.Left);
        }
        else if (movementFlag == 2)
        {
            walkAndJump.Manuever(Direction.Right);
        }
        else
        {
            walkAndJump.Manuever(Direction.None);
        }
    }

    IEnumerator Patrolling()
    {
        var relHab = habitat.x - transform.position.x;
        if ((relHab <= 3f) && (relHab >= -3f))
        {
            movementFlag = Random.Range(0, 3);
        }
        else if (relHab > 3f)
        {
            //Debug.Log("Too Left" + relHab);
            movementFlag = 2;
        }
        else if (relHab < -3f)
        {
            //Debug.Log("Too Right" + relHab);
            movementFlag = 1;
        }
        isWandering = true;
        yield return new WaitForSeconds(1.5f);
        isWandering = false;
    }

    void Update ()
    {
        TargetUpdate();
        Move();
        //ctrl.isTrigger = true;

        isInRange = false;

        if (_state == State.Idle)
        {
            walkAndJump.SetWalkSpeed(1.5f);
            if (isWandering == false)
            {
                StartCoroutine("Patrolling");
            }
        }

        if (targetlist.Count > 0)
        {
            //RaycastHit[] hits = Physics.RaycastAll(_tr.position, _tr.forward, 2f).OrderBy(h=>h.distance).ToArray();
            var hits = Physics.OverlapSphere(_tr.position, 2.5f);

            foreach (var hit in hits)
            {
                if ((hit.gameObject != gameObject) && (hit.gameObject.layer == 9))
                {
                    if ((hit.gameObject.tag == "Player") || (hit.gameObject.tag == "Life"))
                    {
                        isInRange = true;
                        mainTarget = hit.gameObject;
                        Debug.Log("Run!: " + mainTarget.name);
                        StopCoroutine("Patrolling");
                        isWandering = false;
                        _state = State.Run;
                        animator.SetInteger("State", 1);
                    }
                }
            }

            if ((_state == State.Run) && (mainTarget != null))
            {
                var relPos = mainTarget.transform.position - transform.position;
                walkAndJump.SetWalkSpeed(2f);
                if (relPos.x < 0f)
                {
                    //Debug.Log("Running_right");
                    movementFlag = 2;
                }
                else if (relPos.x > 0f)
                {
                    //Debug.Log("Running_left");
                    movementFlag = 1;
                }

                if ((isInRange == false) && ((relPos.x > 3f) || (relPos.x < -3f)))
                {
                    Debug.Log("Where you gone?(mouse): " + mainTarget.name);
                    mainTarget = null;
                    _state = State.Idle;
                    animator.SetInteger("State", 0);
                }
            }
        }
        else
        {
            _state = State.Idle;
        }
    }
}