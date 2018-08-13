using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeUI
{
    public class ChangeBlock : StmtBlock
    {
        private ExprSlot argumentSlot;

        public ChangeType changeType;

        protected override void Awake()
        {
            base.Awake();
            argumentSlot = GetComponentInChildren<ExprSlot>();
        }

        protected override void Start()
        {
            base.Start();

            argumentSlot.CheckBlockValidCallback = block => block is ChangeTypeBlock;
            argumentSlot.OnBlockDropCallback = droppedBlock =>
            {
                var changeTypeBlock = (ChangeTypeBlock) droppedBlock;
                changeType = changeTypeBlock.ChangeType;
            };
        }
    }
}
