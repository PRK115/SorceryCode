using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CodeUI
{
    public class ChangeTypeBlock : RuneBlock, IBeginDragHandler
    {
        private Text text;

        private ChangeType changeType;
        public ChangeType ChangeType
        {
            get { return runeType.adjective; }
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

        protected override void SetPrefabRuneType(RuneType value)
        {
            ChangeType = value.adjective;
        }

        //private void OnDestroy()
        //{
        //    if (ContainedSlot != null && !unused)
        //        RuneStock.Inst.AddRune(new RuneType(changeType));
        //}
    }
}
