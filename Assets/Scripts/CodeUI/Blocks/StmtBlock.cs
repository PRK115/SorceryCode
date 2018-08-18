using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CodeUI
{
    public class StmtBlock : Block
    {
        [NonSerialized]
        public ScopedBlock ParentScope;

        protected override void Awake()
        {
            base.Awake();
            ParentScope = GetComponentInParent<ScopedBlock>();
        }

        protected override void Start()
        {
            base.Start();
        }
    }
}
