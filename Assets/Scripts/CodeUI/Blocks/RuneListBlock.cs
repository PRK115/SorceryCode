namespace CodeUI
{
    public class RuneListBlock : ScopedBlock
    {
        protected override bool IsBlockValid(Block block) => block.IsRune;
    }
}