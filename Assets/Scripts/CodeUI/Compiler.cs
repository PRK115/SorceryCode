using System;
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
        public static Interpreter.Block Compile(ScopedBlock block)
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
                    RepeatBlock repeatBlock = (RepeatBlock) stmtBlock;
                    return new Interpreter.Repeat
                    {
                        Body = new Interpreter.Block
                        {
                            Statements = repeatBlock.Blocks
                                .Select(CompileStmt).ToList()
                        }
                    };
                }
                return new Interpreter.Block
                {
                    Statements = scopedBlock.Blocks
                        .Select(CompileStmt).ToList()
                };
            }
            if (stmtBlock is ConjureBlock)
            {
                ConjureBlock conjureBlock = (ConjureBlock) stmtBlock;
                return new Interpreter.Conjure
                {
                    Entity = conjureBlock.EntityToConjure
                };
            }
            if (stmtBlock is ChangeBlock)
            {
                ChangeBlock changeBlock = (ChangeBlock) stmtBlock;
                return new Interpreter.Change
                {
                    ChangeObj = new Interpreter.ChangeObj { ChangeType = changeBlock.changeType }
                };
            }
            throw new CompilerException($"Statement block {stmtBlock} is not supported.");
        }
    }
}