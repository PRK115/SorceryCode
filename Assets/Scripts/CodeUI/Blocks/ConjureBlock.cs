using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeUI
{
    public class ConjureBlock : StmtBlock
    {
        public string FunName { get; set; }
        public Func<object, object> Fun { get; set; }

        private ExprSlot argumentSlot;
        private Conjurable conjurable;

        protected override void Awake()
        {
            base.Awake();
            argumentSlot = GetComponentInChildren<ExprSlot>();
        }

        protected override void Start()
        {
            base.Start();
            argumentSlot.CheckBlockValidCallback = block =>
            {
                if (block == this) return false;
                if (block is EntityBlock)
                {
                    var entityBlock = (EntityBlock) block;
                    if (entityBlock.Entity.GetComponent<Conjurable>() != null)
                    {
                        return true;
                    }
                }

                return false;
            };
            argumentSlot.OnBlockDropCallback = droppedBlock =>
            {
                EntityBlock entityBlock = (EntityBlock) droppedBlock;
                conjurable = entityBlock.GetComponent<Conjurable>();
            };
        }
    }
}
