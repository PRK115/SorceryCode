using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAndJump : MonoBehaviour {

    public float walkSpeed;
    public float verticalJumpImpulse;
    public float horizontalJumpImpulse;

    private CharacterController ctrl;
    private Vector3 jumpDirection = Vector3.zero;
    private float g = 9.8f;

    Moveable platform;
    //Vector3 originalScale;

    bool isGrounded_delayed;
    int airborneDelay = 4;

    void Awake()
    {
        //originalScale = transform.localScale;
        ctrl = GetComponent<CharacterController>();
        //ctrl.detectCollisions = false;
        //platform = GetComponentInParent<Platform>();
    }

    public void Manuever(Direction direction)
    {
        if (ctrl.isGrounded)
        {
            isGrounded_delayed = true;
            airborneDelay = 4;
        }
        else
        {
            if (airborneDelay == 0)
                isGrounded_delayed = false;
            else
                airborneDelay--;
        }


        Vector3 moveDirection = Vector3.zero;
        if (isGrounded_delayed)
        {
            jumpDirection = Vector3.zero;
            switch (direction)
            {
                case Direction.None:
                    moveDirection = Vector3.zero;
                    break;
                case Direction.Left:
                    transform.LookAt(transform.position + Vector3.left);
                    moveDirection = Vector3.left * walkSpeed;
                    break;
                case Direction.Right:
                    transform.LookAt(transform.position + Vector3.right);
                    moveDirection = Vector3.right * walkSpeed;
                    break;
                case Direction.Up:
                    jumpDirection = new Vector3(0, verticalJumpImpulse, 0);
                    isGrounded_delayed = false;
                    transform.parent = null;
                    break;
                case Direction.LeftUp:
                    transform.LookAt(transform.position + Vector3.left);
                    jumpDirection = new Vector3(-horizontalJumpImpulse, verticalJumpImpulse, 0);
                    isGrounded_delayed = false;
                    transform.parent = null;

                    break;
                case Direction.RightUp:
                    transform.LookAt(transform.position + Vector3.right);
                    jumpDirection = new Vector3(horizontalJumpImpulse, verticalJumpImpulse, 0);
                    isGrounded_delayed = false;
                    transform.parent = null;

                    break;
            }
            if (platform != null)
            {
                moveDirection += new Vector3(platform.XTendency, platform.YTendency, 0) * 1.4f;
            }
            ctrl.Move(moveDirection * Time.deltaTime);
        }

        else
        {
            if(platform == null)
            {
                jumpDirection.y -= g * Time.deltaTime;  
            }
            else
            {
                platform = null;
            }
        }
        ctrl.Move(jumpDirection * Time.deltaTime);
    }

    public void SetWalkSpeed(float speed)
    {
        walkSpeed = speed;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.normal == Vector3.up)
        {
            platform = hit.gameObject.GetComponent<Moveable>();
            //transform.parent = hit.transform;
            //transform.localScale = new Vector3 (originalScale.x/transform.parent.localScale.x, originalScale.y / transform.parent.localScale.y, originalScale.z / transform.parent.localScale.z);
        }

        //if(hit.normal == Vector3.right || hit.normal == Vector3.left)
        //{
        //    jumpDirection.x = 0;
        //}
    }
}
