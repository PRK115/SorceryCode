using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CodeUI
{
    public class StmtBlock : Block
    {
        [NonSerialized]
        public ScopedBlock ParentScope;

        private StmtSlot slot;

        protected override void Awake()
        {
            base.Awake();
            ParentScope = GetComponentInParent<ScopedBlock>();
            slot = GetComponentInChildren<StmtSlot>();
        }

        protected override void Start()
        {
            base.Start();
        }
    }
}
