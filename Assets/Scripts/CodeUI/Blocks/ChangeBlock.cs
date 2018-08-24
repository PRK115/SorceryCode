﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace CodeUI
{
    public class ChangeBlock : StmtBlock
    {
        private ExprSlot argumentSlot;

        public Either<ChangeType, EntityType>? Adjective
        {
            get
            {
                if (argumentSlot.Block is ChangeTypeBlock)
                {
                    return new Either<ChangeType, EntityType>(((ChangeTypeBlock) argumentSlot.Block).ChangeType);
                }
                else if (argumentSlot.Block is EntityBlock)
                {
                    return new Either<ChangeType, EntityType>(((EntityBlock) argumentSlot.Block).EntityType);
                }
                else return null;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            argumentSlot = GetComponentInChildren<ExprSlot>();
        }

        protected override void Start()
        {
            base.Start();
            argumentSlot.CheckBlockValidCallback = block => block is ChangeTypeBlock || block is EntityBlock;
        }
    }
}
