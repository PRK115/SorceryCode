using System;
using UnityEngine;

namespace CodeUI
{
    public class BreakBlock : StmtBlock
    {
        protected override void Awake()
        {
            base.Awake();
            IsRune = false;
        }
    }
}
