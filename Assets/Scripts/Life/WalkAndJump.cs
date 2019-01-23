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
    Vector3 moveDirection = Vector3.zero;

    private float g = 9.8f;

    float halfHeight;
    Moveable platform;
    //Vector3 originalScale;

    bool isGrounded_delayed;
    int airborneDelay = 5;

    AudioSource sound;

    void Awake()
    {
        //originalScale = transform.localScale;
        ctrl = GetComponent<CharacterController>();
        sound = GetComponent<AudioSource>();
        halfHeight = ctrl.height / 2 *transform.localScale.x + 0.02f;
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

        isGrounded_delayed = CheckUnderFoot();

        if (isGrounded_delayed)
        {
            switch (direction)
            {
                case Direction.None:
                    //Debug.Log("none");
                    moveDirection = Vector3.zero;
                    break;

                case Direction.Up:
                    moveDirection = new Vector3(0, verticalJumpImpulse, 0);
                    isGrounded_delayed = false;
                    sound.Play();
                    transform.parent = null;
                    break;
                case Direction.LeftUp:
                    transform.LookAt(transform.position + Vector3.left);
                    moveDirection = new Vector3(-horizontalJumpImpulse, verticalJumpImpulse, 0);
                    sound.Play();
                    isGrounded_delayed = false;
                    transform.parent = null;

                    break;
                case Direction.RightUp:
                    transform.LookAt(transform.position + Vector3.right);
                    moveDirection = new Vector3(horizontalJumpImpulse, verticalJumpImpulse, 0);
                    sound.Play();
                    isGrounded_delayed = false;
                    transform.parent = null;

                    break;
            }
            if (platform != null)
            {
                moveDirection += new Vector3(platform.XTendency, platform.YTendency, 0);
            }
            ctrl.Move(moveDirection * Time.deltaTime);
        }

        else
        {
            moveDirection.y -= g * Time.deltaTime;
            platform = null;
        }

        switch (direction)
        {
            case Direction.None:
                moveDirection = new Vector3(0, moveDirection.y, 0);
                break;

            case Direction.Left:
                transform.LookAt(transform.position + Vector3.left);
                moveDirection = new Vector3(-walkSpeed, moveDirection.y, 0);
                break;

            case Direction.Right:
                transform.LookAt(transform.position + Vector3.right);
                moveDirection = new Vector3(walkSpeed, moveDirection.y, 0);
                break;
        }
        ctrl.Move(moveDirection * Time.deltaTime);
    }

    public void SetWalkSpeed(float speed)
    {
        walkSpeed = speed;
    }

    private bool CheckUnderFoot()
    {
        if (Physics.Raycast(transform.position + ctrl.center, Vector3.down, halfHeight, (1 << 11)))
        {
            RaycastHit[] Moveables = new RaycastHit[1];
            Physics.RaycastNonAlloc(transform.position + ctrl.center, Vector3.down, Moveables, halfHeight, 1 << 11);
            if (Moveables.Length != 0)
            {
                platform = Moveables[0].collider.GetComponent<Moveable>();
                return true;
            }
        }
        
        if (Physics.Raycast(transform.position + ctrl.center, Vector3.down, halfHeight, 1 + (1 << 11) +(1 << 8)))
        {
           
            return true;
        }

        return false;
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
}
