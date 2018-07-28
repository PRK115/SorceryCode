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
    public override void Manuever(Brain.Direction direction)
    {
        if (onGround)
        {
            switch (direction)
            {
                case Brain.Direction.left:
                    transform.LookAt(transform.position + Vector3.left);
                    transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
                    break;
                case Brain.Direction.right:
                    transform.LookAt(transform.position + Vector3.right);
                    transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
                    break;
                case Brain.Direction.up:
                    GetComponent<Rigidbody>().AddForce(new Vector3(0, verticalJumpImpulse, 0), ForceMode.Impulse);
                    break;
                case Brain.Direction.leftUp:
                    GetComponent<Rigidbody>().AddForce(new Vector3(0, verticalJumpImpulse, 0), ForceMode.Impulse);
                    inAirHorizontal = true;
                    break;
                case Brain.Direction.rightUp:
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
        for(int i = 0; i < collision.contacts.GetLength(0); i++)
        {
            Debug.Log(collision.contacts[i].normal);
        }
        if (collision.contacts[0].normal == Vector3.up)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            inAirHorizontal = false;
        }
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
