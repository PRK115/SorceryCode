using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour {

    public enum Direction { none, left, right, up, down, leftUp, rightUp };
    protected Direction command;
	public Direction Command{ get { return command; } }
}
