using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeUI
{
    public class ConjureBlock : StmtBlock
    {
        private ExprSlot argumentSlot;

        public EntityType? EntityToConjure
        {
            get {
                return (argumentSlot.Block as EntityBlock)?.EntityType;  }
        }

        protected override void Awake()
        {
            base.Awake();
            argumentSlot = GetComponentInChildren<ExprSlot>();
            IsRune = false;
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
                    if (commandMgr.IsConjurable(entityBlock.EntityType))
                    {
                        return true;
                    }
                }

                return false;
            };
        }
    }
}
