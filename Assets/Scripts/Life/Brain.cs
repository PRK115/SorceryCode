using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {

    public enum Direction { none, left, right, up, down, leftUp, rightUp }
    protected Direction command;
    public Direction Command { get { return command; } }

    protected bool alive = true;

    public void Death()
    {
        alive = false;
    }
}
