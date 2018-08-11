using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour {

    private Organism organism;
    private WalkAndJump walkAndJump;
    private GameStateManager manager;

    //private bool alive = true;

    private void Awake()
    {
        organism = GetComponent<Organism>();
        walkAndJump = GetComponent<WalkAndJump>();
        manager = FindObjectOfType<GameStateManager>();
    }

    private void Update()
    {
        if (organism.alive)
        {
            walkAndJump.Manuever(InterpretKey());
        }
        else
        {
            if (manager != null)
                manager.Funeral();
        }
    }

    Direction InterpretKey()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (horizontalInput != 0)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (horizontalInput < 0)
                    return Direction.LeftUp;
                else
                    return Direction.RightUp;
            }
            else if (horizontalInput < 0)
            {
                return Direction.Left;
            }
            else
            {
                return Direction.Right;
            }
        }

        else if (Input.GetKeyDown(KeyCode.Space))
        {
            return Direction.Up;
        }

        else
        {
            return Direction.None;
        }



    }
}
