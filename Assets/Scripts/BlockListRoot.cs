using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlockListRoot : MonoBehaviour {

    public static BlockListRoot inst;
    public event Action OnUse;
    public event Action SetAllUnused;

    private void Awake()
    {
        inst = this;
    }

    public void Clear()
    {
        if(OnUse != null)
            OnUse();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ResetUsed()
    {
        SetAllUnused();
    }
}
