using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CodeUI
{
    public class MoveDirBlock : RuneBlock, IBeginDragHandler
    {
        private Text text;

        private RuneType.Direction dir;
        public RuneType.Direction Dir
        {
            get { return runeType.direction; }
            set
            {
                dir = value;
                text.text = dir.ToString();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            text = GetComponentInChildren<Text>();
        }

        protected override void SetPrefabRuneType(RuneType value)
        {
            Dir = value.direction;
        }

        //private void OnDestroy()
        //{
        //    if (ContainedSlot != null && !unused)
        //        RuneStock.Inst.AddRune(new RuneType(dir));
        //}
    }
}