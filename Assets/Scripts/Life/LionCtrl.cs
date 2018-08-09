using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LionCtrl : MonoBehaviour
{
    WalkAndJump walkAndJump;

    public enum eState
    {
        Idle,
        Chase,
    }
    public eState _state;

    public float movePower = 1f;
    int movementFlag = 0;
    bool isWandering = false;

    public Transform playerTr;
    Transform _tr;
    public Vector3 habitat;

    void Awake()
    {
        _tr = gameObject.transform;
        playerTr = GameObject.Find("player").transform;
    }

    void Start()
    {
        _state = eState.Idle;
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
        if ((relHab < 3f) && (relHab > -3f))
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

	void Update()
	{
        var relPos = playerTr.position - transform.position;
        if ((relPos.x < 0.5f) && (relPos.x > -0.5f))
        {
            Debug.Log("Kill you!");
            Destroy(GameObject.Find("player"));
        }

        if (_state == eState.Idle)
        {
            movePower = 1f;
            if (IsInFov(playerTr, 45f, 5f) && (VisionCheck(playerTr, 2f)))
            {
                Debug.Log("Saw you!");
                StopCoroutine("Patrolling");
                isWandering = false;
                _state = eState.Chase;
            }

            if(isWandering == false)
            {
                StartCoroutine("Patrolling");
            }
        }

        if (_state == eState.Chase)
        {
            movePower = 1.5f;
            if (relPos.x < 0f)
            {
                Debug.Log("Chasing_left");
                movementFlag = 1;
            }
            else if (relPos.x > 0f)
            {
                Debug.Log("Chasing_right");
                movementFlag = 2;
            }

            if (!IsInFov(playerTr, 45f, 5f) || !(VisionCheck(playerTr, 2f)))
            {
                Debug.Log("Where you gone?");
                _state = eState.Idle;
            }
        }
	}

    public bool VisionCheck(Transform target, float distance)
    {
        RaycastHit hit;
 
        if(Physics.Raycast(_tr.position, target.position-_tr.position,out hit,distance))
        {
            if(hit.transform == playerTr) return true;
            else return false;
        }
        else return false;
    }   

    public bool IsInFov(Transform target, float angle, float maxHeight)
    {
        var relPos = target.position - _tr.position;
        float height = relPos.y;
        relPos.y = 0;

        if (Mathf.Abs(Vector3.Angle(relPos, transform.forward)) < angle)
        {
            if (Mathf.Abs(height) < maxHeight)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else return false;
    }
}