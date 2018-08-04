using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour {

    private WalkAndJump walkAndJump;
    private GameStateManager manager;

    private bool alive = false;

    private void Awake()
    {
        walkAndJump = GetComponent<WalkAndJump>();
        manager = FindObjectOfType<GameStateManager>();
    }

    private void Update()
    {
        InterpretKey();
        if (alive)
        {
            if (manager != null)
                manager.Funeral();
        }
    }

    void InterpretKey()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput != 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (horizontalInput < 0)
                    walkAndJump.Manuever(Direction.LeftUp);
                else
                    walkAndJump.Manuever(Direction.RightUp);
            }
            else if (horizontalInput < 0)
            {
                walkAndJump.Manuever(Direction.Left);
            }
            else
            {
                walkAndJump.Manuever(Direction.Right);
            }
        }

        else if (Input.GetKeyDown(KeyCode.Space))
        {
            walkAndJump.Manuever(Direction.Up);
        }
    }
}
