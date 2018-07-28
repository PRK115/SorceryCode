using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : Brain {

    GameStateManager manager;

    private void Start()
    {
        if(GameObject.Find("StageCanvas") != null)
        manager = GameObject.Find("StageCanvas").GetComponent<GameStateManager>();
    }

    private void Update()
    {
        if (alive)
        {
            InterpretKey();
        }
        else if(manager != null)
            manager.Funeral();
    }

    void InterpretKey()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput != 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
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
            command = Direction.up;
        }

        else
            command = Direction.none;
    }
}
