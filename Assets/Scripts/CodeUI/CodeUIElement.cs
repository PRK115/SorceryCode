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

        public ScopedBlock Program;

        void Awake()
        {
            Instance = this;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RunProgram();
            }
        }

        public void RunProgram()
        {
            var code = Compiler.Compile(Program);
            Interpreter.Inst.Execute(code);
        }
    }
}
