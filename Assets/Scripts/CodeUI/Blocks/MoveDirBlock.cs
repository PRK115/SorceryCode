using UnityEngine.UI;

namespace CodeUI
{
    public class MoveDirBlock : Block
    {
        private Text text;

        private RuneType.Direction dir;
        public RuneType.Direction Dir
        {
            get { return dir; }
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
    }
}