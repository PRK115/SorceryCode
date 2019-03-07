using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAndJump : MonoBehaviour {

    public float walkSpeed;
    public float verticalJumpImpulse;
    public float horizontalJumpImpulse;

    private CharacterController ctrl;
    //private Vector3 moveDirection = Vector3.zero;
    [SerializeField]
    Vector3 moveDirection = Vector3.zero;
    Vector3 MoveDirection
    {
        get
        {
            return moveDirection;
        }
        set
        {
            //점프하자마자 속도가 0이 되는 현상 방지
            if (!(moveDirection.y == verticalJumpImpulse && value.y == 0))
            {
                moveDirection = value;
            }
        }
    }

    private float g = 9.8f;

    float halfHeight;
    float radius;
    [SerializeField]
    Moveable platform;
    //Vector3 originalScale;

    public bool isGrounded_delayed;
    int airborneDelay = 5;

    AudioSource sound;

    void Awake()
    {
        //originalScale = transform.localScale;
        ctrl = GetComponent<CharacterController>();
        sound = GetComponent<AudioSource>();
        halfHeight = ctrl.height / 2 *transform.localScale.x + 0.01f;
        radius = ctrl.radius * transform.localScale.x;
    }

    public void Manuever(Direction direction)
    {
        //if (ctrl.isGrounded)
        //{
        //    isGrounded_delayed = true;
        //    airborneDelay = 5;
        //}
        //else
        //{
        //    if (airborneDelay == 0)
        //        isGrounded_delayed = false;
        //    else
        //        airborneDelay--;
        //}
        TtookbaegiBreak();
        isGrounded_delayed = CheckUnderFoot();

        if (isGrounded_delayed)
        {
            SqueezeCheck();
            switch (direction)
            {
                case Direction.None:
                    //Debug.Log("none");
                    MoveDirection = Vector3.zero;
                    break;

                case Direction.Up:
                    MoveDirection = new Vector3(0, verticalJumpImpulse, 0);
                    isGrounded_delayed = false;
                    sound.Play();
                    if(platform != null)
                        platform.rider = null;
                    platform = null;
                    break;
                case Direction.LeftUp:
                    transform.LookAt(transform.position + Vector3.left);
                    MoveDirection = new Vector3(-horizontalJumpImpulse, verticalJumpImpulse, 0);
                    sound.Play();
                    isGrounded_delayed = false;
                    if (platform != null)
                        platform.rider = null;
                    platform = null;
                    break;
                case Direction.RightUp:
                    transform.LookAt(transform.position + Vector3.right);
                    MoveDirection = new Vector3(horizontalJumpImpulse, verticalJumpImpulse, 0);
                    sound.Play();
                    isGrounded_delayed = false;
                    if (platform != null)
                        platform.rider = null;
                    platform = null;
                    break;
            }
            if (platform != null)
            {
                //moveDirection += new Vector3(platform.XTendency, platform.YTendency, 0);
                Vector3 platformMove = new Vector3(platform.XTendency, platform.YTendency > MoveDirection.y ? platform.YTendency : MoveDirection.y, 0);
                ctrl.Move(platformMove * Time.deltaTime);
            }
        }

        else
        {
            //Debug.Log("fall");
            //Debug.Log(moveDirection.y);
            MoveDirection -= g * Time.deltaTime * Vector3.up;
        }

        switch (direction)
        {
            case Direction.None:
                moveDirection = new Vector3(0, moveDirection.y, 0);
                break;

            case Direction.Left:
                transform.LookAt(transform.position + Vector3.left);
                MoveDirection = new Vector3(-walkSpeed, MoveDirection.y, 0);
                break;

            case Direction.Right:
                transform.LookAt(transform.position + Vector3.right);
                MoveDirection = new Vector3(walkSpeed, MoveDirection.y, 0);
                break;
        }
    }

    public void SetWalkSpeed(float speed)
    {
        walkSpeed = speed;
    }

    private bool CheckUnderFoot()
    {
        Ray ray = new Ray(transform.position + ctrl.center, Vector3.down);
        if (Physics.SphereCast(ray, radius, halfHeight - radius, (1 << 11)))
        {
            RaycastHit[] Moveables = new RaycastHit[1];
            Physics.SphereCastNonAlloc(ray, radius, Moveables, halfHeight - radius, 1 << 11);
            if (Moveables.Length != 0)
            {
                platform = Moveables[0].collider.GetComponent<Moveable>();
                platform.rider = this;
                //Debug.Log("플랫폼");
                return true;
            }
        }

        //Debug.Log("플랫폼 이탈");
        if(platform!=null)
            platform.rider = null;
        platform = null;

        RaycastHit[] Below = new RaycastHit[1];
        Physics.SphereCastNonAlloc(ray, radius, Below, halfHeight - radius, 1 + (1 << 8));
        if (Below[0].collider != null)
        {
            if (!Below[0].collider.isTrigger)
            {
                return true;
            }
        }

        return false || ctrl.isGrounded;
    }

    void SqueezeCheck()
    {
        RaycastHit[] TtookbaegiBreaker = new RaycastHit[1];

        Ray ray = new Ray(transform.position + ctrl.center + Vector3.up * halfHeight, Vector3.up);

        Physics.RaycastNonAlloc(ray, TtookbaegiBreaker, radius, 1 + (1 << 8) +(1 << 11));

        if (TtookbaegiBreaker[0].collider != null)
        {
            if (!TtookbaegiBreaker[0].collider.isTrigger && TtookbaegiBreaker[0].point.y > transform.position.y)
            {
                GetComponent<Organism>().PhysicalDamage();
            }
        }
    }

    void TtookbaegiBreak()
    {
        Collider[] Moveables = Physics.OverlapSphere(transform.position + ctrl.center, radius, 1 << 11);
        if (Moveables.Length != 0)
        {
            var m = Moveables[0].GetComponent<Moveable>();
            if(m.gravitated)
            {
                m.YTendency = 0;
                GetComponent<Organism>().PhysicalDamage();
            }
            return;
        }
    }

    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    if (hit.normal == Vector3.up)
    //    {
    //        platform = hit.gameObject.GetComponent<Moveable>();
    //        //transform.parent = hit.transform;
    //        //transform.localScale = new Vector3 (originalScale.x/transform.parent.localScale.x, originalScale.y / transform.parent.localScale.y, originalScale.z / transform.parent.localScale.z);
    //    }

    //    //if(hit.normal == Vector3.right || hit.normal == Vector3.left)
    //    //{
    //    //    jumpDirection.x = 0;
    //    //}
    //}

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if ((hit.normal == Vector3.up && moveDirection.y < 0) || (hit.normal == Vector3.down && moveDirection.y >0))
        {
            //Debug.Log(moveDirection.y);
            moveDirection.y = 0;
        }
    }

    void Update()
    {
        //Debug.Log(moveDirection.y);
        if(ctrl.enabled)
            ctrl.Move(moveDirection * Time.deltaTime);
    }
}
