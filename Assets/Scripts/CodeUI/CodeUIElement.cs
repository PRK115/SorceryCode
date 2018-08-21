using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeUI
{
    public class CodeUIElement : MonoBehaviour
    {
        public static CodeUIElement Instance;

        public Block DraggedBlock;

        public StmtListBlock Program;

        [SerializeField] private GraphicRaycaster raycaster;
        [SerializeField] private EventSystem eventSystem;
        private PointerEventData pointerEventData;

        public List<Block> HoveredBlocks { get; private set; }

        void Awake()
        {
            Instance = this;
        }

        void Update()
        {
            if (DraggedBlock != null)
            {
                pointerEventData = new PointerEventData(eventSystem);
                pointerEventData.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                raycaster.Raycast(pointerEventData, results);
                HoveredBlocks = results
                    .Select(res => res.gameObject.GetComponent<Block>())
                    .Where(b => b != null)
                    .ToList();
            }
        }

        public void RunProgram()
        {
            var code = Compiler.Compile(Program);
            Interpreter.Inst.Execute(code);
        }
    }
}
