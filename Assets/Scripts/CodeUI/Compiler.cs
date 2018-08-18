﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeUI
{
    public class CompilerException : Exception
    {
        public CompilerException(string message) : base(message) { }
    }

    public class Compiler
    {
        public static Interpreter.Block Compile(StmtListBlock block)
        {
            return (Interpreter.Block) CompileStmt(block);
        }

        private static Interpreter.Stmt CompileStmt(StmtBlock stmtBlock)
        {
            if (stmtBlock is ScopedBlock)
            {
                ScopedBlock scopedBlock = (ScopedBlock) stmtBlock;
                if (scopedBlock is RepeatBlock)
                {
                    RepeatBlock repeatBlock = (RepeatBlock) scopedBlock;
                    return new Interpreter.Repeat
                    {
                        Body = new Interpreter.Block
                        {
                            Statements = repeatBlock.StatementBlocks
                                .Select(CompileStmt).ToList()
                        }
                    };
                }
                if (scopedBlock is StmtListBlock)
                {
                    StmtListBlock stmtListBlock = (StmtListBlock) scopedBlock;
                    return new Interpreter.Block
                    {
                        Statements = stmtListBlock.StatementBlocks
                            .Select(CompileStmt).ToList()
                    };
                }
                throw new CompilerException($"Scoped block {scopedBlock.GetType().Name} is not supported.");
            }
            if (stmtBlock is ConjureBlock)
            {
                ConjureBlock conjureBlock = (ConjureBlock) stmtBlock;
                if (conjureBlock.EntityToConjure == null)
                    throw new CompilerException($"Missing argument in statement Conjure.");
                return new Interpreter.Conjure
                {
                    Entity = conjureBlock.EntityToConjure.Value
                };
            }
            if (stmtBlock is ChangeBlock)
            {
                ChangeBlock changeBlock = (ChangeBlock) stmtBlock;
                if (changeBlock.ChangeType == null)
                    throw new CompilerException($"Missing argument in statement Change.");
                return new Interpreter.Change
                {
                    ChangeType = changeBlock.ChangeType.Value
                };
            }
            throw new CompilerException($"Statement block {stmtBlock.GetType().Name} is not supported.");
        }
    }
}