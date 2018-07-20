using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : Control {

    private void Update()
    {

        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput != 0)
        {
            if (Input.GetKeyDown("space"))
            {
                if (horizontalInput < 0)
                    command = Direction.leftUp;
                else
                    command = Direction.rightUp;
            }
            else if (horizontalInput < 0)
            {
                command = Direction.left;
            }
            else
                command = Direction.right;
        }

        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("jump");
            command = Direction.up;
        }

        else
            command = Direction.none;
    }
}
