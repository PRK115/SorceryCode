using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : Key, IConsumable {

    GameStateManager gameStateManager;
    PlayerCtrl playerCtrl;
    private void Start()
    {
        playerCtrl = FindObjectOfType<PlayerCtrl>();
        gameStateManager = FindObjectOfType<GameStateManager>();
    }

    public override void ConsumedBehaviour()
    {
        base.ConsumedBehaviour();
        playerCtrl.GoalX = transform.position.x;
        playerCtrl.SetState(PlayerCtrl.State.Cleared);
    }
}
