namespace CodeUI
{
    public class MoveBlock : StmtBlock
    {
        private ExprSlot dirSlot;

        public MoveDirection Dir;
        public int? Distance
        {
            get { return (dirSlot.Block as MoveDistanceBlock)?.Distance; }
        }

        protected override void Awake()
        {
            base.Awake();
            dirSlot = GetComponentInChildren<ExprSlot>();
        }

        protected override void Start()
        {
            base.Start();
            dirSlot.CheckBlockValidCallback = block =>
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