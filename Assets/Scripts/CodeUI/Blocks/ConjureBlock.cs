using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeUI
{
    public class ConjureBlock : Block
    {
        public string FunName { get; set; }
        public Func<object, object> Fun { get; set; }

        private Slot argumentSlot;
        private Conjurable conjurable;

        public void Awake()
        {
            argumentSlot = GetComponentInChildren<Slot>();
        }

        public void Start()
        {
            argumentSlot.CheckBlockValidCallback = block =>
            {
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
