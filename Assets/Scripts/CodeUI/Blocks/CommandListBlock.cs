namespace CodeUI
{
    public class CommandListBlock : ScopedBlock
    {
        protected override bool IsBlockValid(Block block) => !block.IsRune;
    }
}