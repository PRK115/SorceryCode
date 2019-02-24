using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingButton : MonoBehaviour {
    public event Action OnAimClicked;
    public static AimingButton inst;

    private void Awake()
    {
        inst = this;
    }

    private bool exprSlotsFull;
    public bool ExprSlotsFull { set { exprSlotsFull = exprSlotsFull && value; } }

	public void CheckExprSlots()
    {
        exprSlotsFull = true;
        if (OnAimClicked != null)
            OnAimClicked();
        if (exprSlotsFull)
        {
            PlayerCtrl.inst.SetState(PlayerCtrl.State.Aiming);
            GameStateManager.instance.SetState(GameStateManager.UIState.Game);
        }
    }
}
