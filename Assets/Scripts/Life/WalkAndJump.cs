using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAndJump : MonoBehaviour {

    public float walkSpeed;
    public float verticalJumpImpulse;
    public float horizontalJumpImpulse;
    //bool onGround = false;
    //bool inAirHorizontal = false;

    //private Rigidbody rb;
    private CharacterController ctrl;
    private Vector3 jumpDirection = Vector3.zero;
    private float g = 9.8f;

    void Awake()
    {
        //rb = GetComponent<Rigidbody>();
        ctrl = GetComponent<CharacterController>();
    }

    public void Manuever(Direction direction)
    {
        if (ctrl.isGrounded)
        {
            jumpDirection = Vector3.zero;
            switch (direction)
            {
                case Direction.Left:
                    transform.LookAt(transform.position + Vector3.left);
                    ctrl.SimpleMove(Vector3.left * walkSpeed);
                    break;
                case Direction.Right:
                    transform.LookAt(transform.position + Vector3.right);
                    ctrl.SimpleMove(Vector3.right    * walkSpeed);
                    break;
                case Direction.Up:
                    jumpDirection = new Vector3(0, verticalJumpImpulse, 0);
                    Debug.Log("up");
                    //rb.AddForce(new Vector3(0, verticalJumpImpulse, 0), ForceMode.Impulse);
                    break;
                case Direction.LeftUp:
                    jumpDirection = new Vector3(-horizontalJumpImpulse, verticalJumpImpulse, 0);
                    //rb.AddForce(new Vector3(0, verticalJumpImpulse, 0), ForceMode.Impulse);
                    //inAirHorizontal = true;
                    break;
                case Direction.RightUp:
                    jumpDirection = new Vector3(horizontalJumpImpulse, verticalJumpImpulse, 0);
                    //rb.AddForce(new Vector3(0, verticalJumpImpulse, 0), ForceMode.Impulse);
                    //inAirHorizontal = true;
                    break;
            }
        }
            ctrl.Move(jumpDirection * Time.deltaTime);
            if(!ctrl.isGrounded)
                jumpDirection.y -= g * Time.deltaTime;

        //else if (inAirHorizontal)
        //    transform.Translate(Vector3.forward * horizontalJumpImpulse * Time.deltaTime);
    }
}
