namespace CodeUI
{
    public class MoveBlock : StmtBlock
    {
        public ExprSlot dirSlot;
        public UnityEngine.UI.Dropdown dropdown;

        public MoveDirection? Dir
        {
            get { return (dirSlot.Block as MoveDirBlock)?.Dir; }
        }
        public int? Distance
        {
            get { return dropdown.value + 1; }
        }

        protected override void Start()
        {
            base.Start();
            dirSlot.CheckBlockValidCallback = block => block is MoveDirBlock;
        }
    }
}