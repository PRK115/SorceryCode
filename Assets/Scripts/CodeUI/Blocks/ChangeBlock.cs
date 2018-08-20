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

        public ChangeType? ChangeType
        {
            get { return (argumentSlot.Block as ChangeTypeBlock)?.ChangeType; }
        }

        protected override void Awake()
        {
            base.Awake();
            argumentSlot = GetComponentInChildren<ExprSlot>();
        }

        protected override void Start()
        {
            base.Start();
            argumentSlot.CheckBlockValidCallback = block => block is ChangeTypeBlock;
        }
    }
}
