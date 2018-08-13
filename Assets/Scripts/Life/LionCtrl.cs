﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LionCtrl : MonoBehaviour
{
    WalkAndJump walkAndJump;

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
        _tr = gameObject.transform;
        habitat = transform.position;

        if (GameObject.FindWithTag("Player"))
        {
            player = GameObject.FindWithTag("Player");
            targetlist.Add(player);
        }
        if (GameObject.FindWithTag("Life"))
        {
            targetlist.AddRange(GameObject.FindGameObjectsWithTag("Life"));
        }
    }

    void Start()
    {
        _state = State.Idle;
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;

        if (movementFlag == 1)
        {
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3(1, 1, 1);
            transform.rotation = Quaternion.Euler(0, 270, 0);
        }
        else if (movementFlag == 2)
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(-1, 1, 1);
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        transform.position += moveVelocity * movePower * Time.deltaTime;
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
            //Debug.Log("Too Left" + relHab);
            movementFlag = 2;
        }
        else if(relHab < -3f)
        {
            //Debug.Log("Too Right" + relHab);
            movementFlag = 1;
        }
        isWandering = true;
        yield return new WaitForSeconds(1.5f);
        isWandering = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != gameObject)
        {
            if ((other.gameObject.tag == "Player") || (other.gameObject.tag == "Life"))
            {
                Debug.Log("Kill you!: " + other.gameObject.name);
                Destroy(other.gameObject);
                targetlist.Remove(other.gameObject);
                _state = State.Idle;
            }
        }
    }

    void Update()
	{
        isInRange = false;

        if (_state == State.Idle)
        {
            movePower = 1f;
            if (isWandering == false)
            {
                StartCoroutine("Patrolling");
            }
        }

        if (targetlist.Count > 0)
        {
            RaycastHit[] hits = Physics.RaycastAll(_tr.position, _tr.forward, 2f).OrderBy(h => h.distance).ToArray();
            foreach (var hit in hits)
            {
                if ((hit.collider.gameObject.tag == "Player") || (hit.collider.gameObject.tag == "Life"))
                {
                    isInRange = true;
                    if (mainTarget != player)
                    {
                        mainTarget = hit.collider.gameObject;
                        Debug.Log("Saw you!: " + mainTarget.name);
                        StopCoroutine("Patrolling");
                        isWandering = false;
                        _state = State.Chase;
                    }
                }
            }

            if ((_state == State.Chase) && (mainTarget != null))
            {
                var relPos = mainTarget.transform.position - transform.position;
                movePower = 1.5f;
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
                }
            }
        }
        else
        {
            _state = State.Idle;
        }
	}

}