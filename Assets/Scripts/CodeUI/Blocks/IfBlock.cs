namespace CodeUI
{
    public class IfBlock : StmtListBlock
    {
        private ExprSlot conditionSlot;

        //public bool? Condition
        //{
        //    get { return (conditionSlot.Block as ConditionBlock)?.condition; }
        //}

        public EntityType? EntityToCheck
        {
            get { return (conditionSlot.Block as EntityBlock)?.EntityType; }
        }

        protected override void Awake()
        {
            base.Awake();
            IsRune = false;
            conditionSlot = GetComponentInChildren<ExprSlot>();
            DynamicHeight = true;
        }

        protected override void Start()
        {
            base.Start();
            conditionSlot.CheckBlockValidCallback = 
                block => block is EntityBlock;
        }
    }
}