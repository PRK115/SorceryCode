using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour, IConsumable {

    GameStateManager gameStateManager;

    private void Awake()
    {
    }

    private void Start()
    {
        gameStateManager = GameObject.Find("StageCanvas").GetComponent<GameStateManager>();
    }

    public void ConsumedBehaviour()
    {
        gameStateManager.StageClear();
    }
}
