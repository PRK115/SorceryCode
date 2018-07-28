using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    GameStateManager gameStateManager;

    private void Start()
    {
        gameStateManager = GameObject.Find("StageCanvas").GetComponent<GameStateManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            gameStateManager.StageClear();
    }
}
