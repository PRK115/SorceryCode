using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CodeUI
{
    public class StmtBlock : Block
    {
        protected void Update()
        {
            if (transform.parent == null)
                Destroy(gameObject);
        }

    }
}
