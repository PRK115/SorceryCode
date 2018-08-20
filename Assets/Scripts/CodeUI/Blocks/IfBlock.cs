namespace CodeUI
{
    public class IfBlock : StmtListBlock
    {
        private ExprSlot conditionSlot;

        public bool? Condition
        {
            get { return (conditionSlot.Block as ConditionBlock)?.condition; }
        }

        protected override void Awake()
        {
            base.Awake();
            conditionSlot = GetComponentInChildren<ExprSlot>();
            DynamicHeight = true;
        }

        protected override void Start()
        {
            base.Start();
            conditionSlot.CheckBlockValidCallback = 
                block => block is ConditionBlock;
        }
    }
}