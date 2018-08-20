using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeUI
{
    class RepeatBlock : StmtListBlock
    {
        protected override void Start()
        {
            base.Start();
            DynamicHeight = true;
        }
    }
}
