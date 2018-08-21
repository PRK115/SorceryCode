using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LionCtrl : MonoBehaviour
{
    WalkAndJump walkAndJump;
    CharacterController ctrl;

    Animator animator;

    public enum State
    {
        Idle,
        Chase,
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

        TargetUpdate();

        animator = GetComponent<Animator>();
    }

    void Start()
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
        else if(relHab > 3f)
        {
            Debug.Log("Too Left" + relHab);
            movementFlag = 2;
        }
        else if(relHab < -3f)
        {
            Debug.Log("Too Right" + relHab);
            movementFlag = 1;
        }
        isWandering = true;
        yield return new WaitForSeconds(1.5f);
        isWandering = false;
    }

    void Killing(float distance)
    {
        var hits = Physics.OverlapSphere(_tr.position, distance);

        foreach (var hit in hits)
        {
            if ((hit.gameObject != gameObject) && (hit.gameObject.layer == 9))
            {
                if ((hit.gameObject.tag == "Player") || (hit.gameObject.tag == "Life"))
                {
                    Debug.Log("Kill you!: " + hit.gameObject.name);
                    Destroy(hit.gameObject);
                    targetlist.Remove(hit.gameObject);
                    _state = State.Idle;
                    animator.SetInteger("State", 0);
                    mainTarget = null;
                }
            }
        }
    }

    void Update()
	{
        TargetUpdate();
        Move();
        //ctrl.enabled = false;

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
            //RaycastHit[] hits = Physics.RaycastAll(_tr.position, _tr.forward, 2f).OrderBy(h => h.distance).ToArray();
            var hits = Physics.OverlapSphere(_tr.position, 3f);

            foreach (var hit in hits)
            {
                if((hit.gameObject != gameObject) && (hit.gameObject.layer == 9))
                {
                    if ((hit.gameObject.tag == "Player") || (hit.gameObject.tag == "Life"))
                    {
                        isInRange = true;
                        if ((mainTarget != player) || (mainTarget == null))
                        {
                            mainTarget = hit.gameObject;
                            StopCoroutine("Patrolling");
                            isWandering = false;
                            _state = State.Chase;
                            animator.SetInteger("State", 1);
                        }
                    }
                }
            }

            if ((_state == State.Chase) && (mainTarget != null))
            {
                var relPos = mainTarget.transform.position - transform.position;
                walkAndJump.SetWalkSpeed(2f);

                if (relPos.x < 0f)
                {
                    //Debug.Log("Chasing_left");
                    movementFlag = 1;
                }
                else if (relPos.x > 0f)
                {
                    //Debug.Log("Chasing_right");
                    movementFlag = 2;
                }

                if (isInRange == false)
                {
                    Debug.Log("Where you gone?(lion): " + mainTarget.name);
                    mainTarget = null;
                    _state = State.Idle;
                    animator.SetInteger("State", 0);
                }
            }
        }
        else
        {
            _state = State.Idle;
            animator.SetInteger("State", 0);
        }

        Killing(0.5f);
	}

}