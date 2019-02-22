using UnityEngine.UI;
using UnityEngine;

namespace CodeUI
{
    public class IfBlock : StmtListBlock
    {
        private ExprSlot conditionSlot;
        public bool toggleable;
        private bool not;
        public bool Not => toggleable ? not : false;
        private GameObject trueText;
        private GameObject falseText;

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
            if(toggleable)
            {
                Toggle toggle = transform.Find("Toggle").GetComponent<Toggle>();
                Transform background = toggle.transform.Find("Background");
                falseText = background.Find("false").gameObject;
                trueText = background.Find("true").gameObject;

                toggle.onValueChanged.AddListener((bool boolean) =>
                {
                    not = boolean;
                    falseText.SetActive(not);
                    trueText.SetActive(!not);
                }
                );

            }
        }

        protected override void Start()
        {
            base.Start();
            conditionSlot.CheckBlockValidCallback = 
                block => block is EntityBlock;
        }
    }
}