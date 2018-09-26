using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockListRoot : MonoBehaviour {

    public static BlockListRoot inst;

    private void Awake()
    {
        inst = this;
    }

    public void Clear()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

    }
}
