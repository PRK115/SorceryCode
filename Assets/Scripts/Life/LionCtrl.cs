using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LionCtrl : MonoBehaviour
{
    WalkAndJump walkAndJump;
    Organism org;
    //CharacterController ctrl;

    Animator animator;
    AudioSource sound;

    public enum State
    {
        Idle,
        Chase,
    }
    public State _state;

    public float runSpeed;
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
        org = GetComponent<Organism>();
        sound = GetComponent<AudioSource>();

        //ctrl = GetComponent<CharacterController>();

        _tr = gameObject.transform;
        habitat = transform.position;

        TargetUpdate();

        animator = GetComponent<Animator>();
    }

    void Start()
    {
        SetState(State.Idle);
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

        for(int i = 0; i < targetlist.Count; i++)
        {
            if (!targetlist[i].GetComponent<Organism>().alive)
            {
                targetlist.Remove(targetlist[i]);
            }
        }

        //foreach(GameObject animal in targetlist)
        //{
        //    if(!animal.GetComponent<Organism>().alive)
        //    {
        //        targetlist.Remove(animal);
        //    }
        //}
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
        //isWandering = true;
        var relHab = habitat.x - transform.position.x;
        //if ((relHab <= 3f) && (relHab >= -3f))
        //{
        //    movementFlag = Random.Range(0, 3);
        //}
        //else if(relHab > 3f)
        if(relHab>1f)
        {
            movementFlag = 2;
        }
        else if(relHab < -1f)
        {
            movementFlag = 1;
        }
        yield return new WaitForSeconds(1.5f);
        isWandering = false;
        //Debug.Log("끝");
    }

    void Killing(float distance)
    {
        var hits = Physics.OverlapSphere(_tr.position, distance);

        foreach (var hit in hits)
        {
            if ((hit.gameObject != gameObject) && (hit.gameObject.layer == 9))
            {
                Organism org = hit.GetComponent<Organism>();
                sound.Play();
                org.PhysicalDamage();
                targetlist.Remove(hit.gameObject);
                SetState(State.Idle);
                mainTarget = null;
            }
        }
    }

    void Update()
	{
        if(!org.alive)
        {
            return;
        }
        TargetUpdate();
        Move();
        //ctrl.enabled = false;

        isInRange = false;

        if (_state == State.Idle)
        {
            if (isWandering == false)
            {
                //Debug.Log($"{isWandering}  start");
                isWandering = true;

                StartCoroutine(Patrolling());
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
                            StopCoroutine(Patrolling());
                            if(mainTarget.GetComponent<Organism>().alive)
                                SetState(State.Chase);
                            //isWandering = false;
                        }
                    }
                }
            }

            if ((_state == State.Chase) && (mainTarget != null))
            {
                var relPos = mainTarget.transform.position - transform.position;

                Killing(0.5f);

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
                    mainTarget = null;
                    Debug.Log("시야 내에 없다");
                    SetState(State.Idle);
                }
            }
        }
        else
        {
            Debug.Log("타겟 없음");
            SetState(State.Idle);
        }

	}

    private void SetState(State state)
    {
        if(_state != state)
        {
            _state = state;
            switch (state)
            {
                case State.Idle:
                    walkAndJump.SetWalkSpeed(0.8f);
                    break;

                case State.Chase:
                    walkAndJump.SetWalkSpeed(runSpeed);
                    isWandering = false;
                    //Debug.Log("false");
                    break;
            }
            animator.SetInteger("State", (int)state);
        }
    }
}