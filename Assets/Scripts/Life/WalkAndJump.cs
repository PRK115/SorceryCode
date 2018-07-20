using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAndJump : ManueverType {

    public float walkSpeed;
    public float verticalJumpImpulse;
    public float horizontalJumpImpulse;
    bool onGround = false;
    bool inAirHorizontal = false;
    //bool slowDown = false;

    public override void Manuever(Control.Direction direction)
    {
        Debug.Log(direction +" " + inAirHorizontal);
        if (onGround)
        {
            switch (direction)
            {
                case Control.Direction.left:
                    transform.LookAt(transform.position + Vector3.left);
                    transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
                    break;
                case Control.Direction.right:
                    transform.LookAt(transform.position + Vector3.right);
                    transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
                    break;
                case Control.Direction.up:
                    GetComponent<Rigidbody>().AddForce(new Vector3(0, verticalJumpImpulse, 0), ForceMode.Impulse);
                    break;
                case Control.Direction.leftUp:
                    GetComponent<Rigidbody>().AddForce(new Vector3(0, verticalJumpImpulse, 0), ForceMode.Impulse);
                    inAirHorizontal = true;
                    break;
                case Control.Direction.rightUp:
                    GetComponent<Rigidbody>().AddForce(new Vector3(0, verticalJumpImpulse, 0), ForceMode.Impulse);
                    inAirHorizontal = true;
                    break;
            }
        }

        else if (inAirHorizontal)
            transform.Translate(Vector3.forward * horizontalJumpImpulse * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].normal == Vector3.up)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            //slowDown = false;
        }
        inAirHorizontal = false;
    }

    private void OnCollisionStay (Collision collision)
    {
        if (collision.contacts[0].normal == Vector3.up)
            onGround = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        onGround = false;
    }
}
