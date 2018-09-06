using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace CodeUI
{
    public class ChangeTypeBlock : Block
    {
        private Text text;

        private ChangeType changeType;
        public ChangeType ChangeType
        {
            get { return changeType; }
            set
            {
                changeType = value;
                text.text = changeType.ToString();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            text = GetComponentInChildren<Text>();
        }
    }
}
