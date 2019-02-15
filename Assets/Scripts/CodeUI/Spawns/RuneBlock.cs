using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeUI
{
    public class RuneBlock : Block, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private bool used = false;
        private new void Awake()
        {
            base.Awake();
        }
        public RuneBlock blockPrefab;

        private void Update()
        {
            if (transform.parent == null)
            {
                Destroy(gameObject);
            }
        }

        public RuneType Type
        {
            set
            {
                SetPrefabRuneType(value);
            }
        }

        public RuneBlock spawnedBlock;

        public new void OnBeginDrag(PointerEventData eventData)
        {
            if (ContainedScopedBlock is RuneListBlock)
            {
                RuneStock.Inst.DeductRune(runeType);
            }
            if (!IsMovable)
            {
                spawnedBlock = Instantiate(blockPrefab, transform.position, Quaternion.identity);
                BlockListRoot.inst.OnUse += spawnedBlock.SetUsed;
                BlockListRoot.inst.SetAllUnused += spawnedBlock.SetUnused;
                spawnedBlock.IsMovable = true;
                spawnedBlock.RuneType = RuneType;
                spawnedBlock.BaseOnPointerDown(eventData);
                spawnedBlock.BaseOnBeginDrag(eventData);
            }
            else
                BaseOnBeginDrag(eventData);
        }

        public new void OnDrag(PointerEventData eventData)
        {
            if (ContainedScopedBlock is RuneListBlock && !IsMovable && spawnedBlock != null)
            {
                spawnedBlock.BaseOnDrag(eventData);
            }
            else
            {
                BaseOnDrag(eventData);
            }
        }

        public new void OnEndDrag(PointerEventData eventData)
        {
            if (ContainedScopedBlock is RuneListBlock && !IsMovable)
            {
                spawnedBlock.BaseOnEndDrag(eventData);

                if (RuneStock.Inst.runeStockDictionary[runeType] == 1)
                {
                    IsMovable = true;
                }
            }
            else
                BaseOnEndDrag(eventData);
        }

        public void BaseOnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
        }

        public void BaseOnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
        }

        public void BaseOnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
        }

        public void BaseOnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
        }

        protected virtual void SetPrefabRuneType(RuneType value)
        {

        }

        public void SetUsed()
        {
            used = true;
            //Debug.Log(used);
        }

        public void SetUnused()
        {
            Debug.Log(gameObject.name +" set unused");
            used = false;
        }

        protected void OnDestroy()
        {
            Debug.Log(gameObject.name + " " + used);
            if (!used)
            {
                if(RuneStock.Inst != null)
                RuneStock.Inst.ReturnRune(runeType);
            }
            /*else if(used)
            {
                BlockListRoot.inst.ResetUsed();
            }*/
        }
    }
}
