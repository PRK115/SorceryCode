namespace CodeUI
{
    public class RuneListBlock : ScopedBlock
    {
        protected override bool IsBlockValid(Block block)
            => block is ChangeTypeBlock || block is EntityBlock;
    }
}