using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : Key, IConsumable {

    GameStateManager gameStateManager;

    private void Start()
    {
        gameStateManager = GameObject.Find("StageCanvas").GetComponent<GameStateManager>();
    }

    public new void ConsumedBehaviour()
    {
        gameStateManager.StageClear();
        base.ConsumedBehaviour();
    }
}
