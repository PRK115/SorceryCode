using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CodeUI
{
    public class CodeUIElement : MonoBehaviour
    {
        public float LineWidth;
        public float LineHeight;
        public float IndentWidth;

        public static CodeUIElement Instance;

        private List<Block> codeBlocks = new List<Block>();

        void Awake()
        {
            Instance = this;
        }
    }
}
