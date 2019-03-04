using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimingButton : MonoBehaviour {
    public event Action OnAimClicked;
    public static AimingButton inst;

    private void Awake()
    {
        inst = this;
        transform.GetChild(0).GetComponent<Text>().text = "Cast\n(Enter)";
    }

    private bool exprSlotsFull;
    public bool ExprSlotsFull { set { exprSlotsFull = exprSlotsFull && value; } }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
            CheckExprSlots();
    }

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
