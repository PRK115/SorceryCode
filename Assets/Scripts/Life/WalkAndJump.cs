using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAndJump : MonoBehaviour {

    public float walkSpeed;
    public float verticalJumpImpulse;
    public float horizontalJumpImpulse;

    Rigidbody platformRb;
    Vector3 platformOffset;

    private CharacterController ctrl;
    private Vector3 jumpDirection = Vector3.zero;
    private float g = 9.8f;

    void Awake()
    {
        ctrl = GetComponent<CharacterController>();
    }

    public void Manuever(Direction direction)
    {

        Vector3 moveDirection = Vector3.zero;
        if (ctrl.isGrounded)
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
                    break;
                case Direction.LeftUp:
                    jumpDirection = new Vector3(-horizontalJumpImpulse, verticalJumpImpulse, 0);
                    break;
                case Direction.RightUp:
                    jumpDirection = new Vector3(horizontalJumpImpulse, verticalJumpImpulse, 0);
                    break;
            }
            if(platformRb != null)
            {
                Debug.Log(platformRb.velocity);
                moveDirection += platformRb.velocity * 2;
            }
            ctrl.Move(moveDirection * Time.deltaTime);
        }

        else
        {
            jumpDirection.y -= g * Time.deltaTime;
        }
        ctrl.Move(jumpDirection * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.normal == Vector3.up)
        {
            platformRb = hit.gameObject.GetComponent<Rigidbody>();
        }
    }
}
