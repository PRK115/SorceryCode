using System.Collections.Generic;
using System.Linq;

namespace CodeUI
{
    public class StmtListBlock : ScopedBlock
    {
        public List<StmtBlock> StatementBlocks
        {
            get { return Blocks.Select(block => (StmtBlock) block).ToList(); }
        }

        protected override bool IsBlockValid(Block block) 
            => block is StmtBlock;
    }
}