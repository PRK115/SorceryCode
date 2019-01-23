using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CodeUI
{
    public class EntityBlock : RuneBlock, IBeginDragHandler
    {
        private Text text; 
        [SerializeField]
        private EntityType entityType;
        public EntityType EntityType
        {
            get { return runeType.Entity; }
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

        protected override void SetPrefabRuneType(RuneType value)
        {
            EntityType = value.Entity;
        }

        //private void OnDestroy()
        //{
        //    //if (ContainedSlot != null && !unused)
        //    //    RuneStock.Inst.AddRune(new RuneType(entityType));
        //}
    }
}

