using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CodeUI
{
    public class EntityBlock : Block
    {
        private Text text; 

        private EntityType entityType;
        public EntityType EntityType
        {
            get { return entityType; }
            set
            {
                entityType = value;
                text.text = entityType.ToString();
            }

        }

        protected override void Awake()
        {
            base.Awake();
            text = GetComponentInChildren<Text>();
        }
    }
}

