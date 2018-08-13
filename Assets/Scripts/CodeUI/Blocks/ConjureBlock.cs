using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeUI
{
    public class ConjureBlock : StmtBlock
    {
        private ExprSlot argumentSlot;

        public EntityType EntityToConjure { get; private set; }

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
                    if (commandMgr.IsConjurable(entityBlock.EntityType))
                    {
                        return true;
                    }
                }

                return false;
            };
            argumentSlot.OnBlockDropCallback = droppedBlock =>
            {
                EntityBlock entityBlock = (EntityBlock) droppedBlock;
                EntityToConjure = entityBlock.EntityType;
            };
        }
    }
}
