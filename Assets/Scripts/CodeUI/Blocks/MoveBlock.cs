namespace CodeUI
{
    public class MoveBlock : StmtBlock
    {
        public ExprSlot dirSlot;
        public ExprSlot distanceSlot;

        public MoveDirection? Dir
        {
            get { return (dirSlot.Block as MoveDirBlock)?.Dir; }
        }
        public int? Distance
        {
            get { return (distanceSlot.Block as MoveDistanceBlock)?.Distance; }
        }

        protected override void Start()
        {
            base.Start();
            dirSlot.CheckBlockValidCallback = block => block is MoveDirBlock;
            distanceSlot.CheckBlockValidCallback = block =>
            {
                if (block is MoveDistanceBlock)
                {
                    int dist = ((MoveDistanceBlock) block).Distance;
                    return dist > 0 && dist <= 3;
                }
                return false;
            };
        }
    }
}