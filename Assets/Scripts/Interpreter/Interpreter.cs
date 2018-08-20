using CodeUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Interpreter : MonoBehaviour
{
    public abstract class Expr
    {
        public abstract object Eval();
    }

    public abstract class Stmt
    {
        public abstract Task<StmtResult> Eval();
    }

    public enum StmtResult
    {
        None, Break
    }

    public class LangException : Exception
    {
        public LangException(string message) : base(message) { }
    }

    public class Entity : Expr
    {
        public EntityType Type;

        public override object Eval() => Type;
    }

    public class ChangeObj : Expr
    {
        public ChangeType ChangeType;

        public override object Eval() => ChangeType;
    }

    public class BoolExpr : Expr
    {
        public bool Value;

        public override object Eval() => Value;
    }

    public class Expression : Stmt
    {
        public Expr Expr;

        public override async Task<StmtResult> Eval()
        {
            Expr.Eval();
            await new WaitForSeconds(Inst.Delay);
            return StmtResult.None;
        }
    }

    public class Block : Stmt
    {
        public List<Stmt> Statements;

        public override async Task<StmtResult> Eval()
        {
            foreach (Stmt stmt in Statements)
            {
                StmtResult result = await stmt.Eval();
                if (result == StmtResult.Break)
                    return StmtResult.Break;
            }
            return StmtResult.None;
        }
    }

    public class If : Stmt
    {
        public Expr Cond;
        public Stmt Then;
        public Stmt Else;

        public override async Task<StmtResult> Eval()
        {
            object condResult = Cond.Eval();
            if (condResult is bool)
            {
                bool condValue = (bool) condResult;
                if (condValue)
                {
                    return await Then.Eval();
                }
                if (Else != null)
                {
                    return await Else.Eval();
                }
                return StmtResult.None;
            }
            else
            {
                throw new LangException("condition in if is not boolean");
            }
        }
    }

    public class Repeat : Stmt
    {
        public Stmt Body;

        public override async Task<StmtResult> Eval()
        {
            while (true)
            {
                StmtResult result = await Body.Eval();
                if (result == StmtResult.Break)
                {
                    return StmtResult.None;
                }
            }
        }
    }

    public class Break : Stmt
    {
        public override async Task<StmtResult> Eval()
        {
            return StmtResult.Break;
        }
    }

    public class Conjure : Stmt
    {
        public EntityType Entity;

        public override async Task<StmtResult> Eval()
        {
            Inst.CommandMgr.Conjure(Entity);
            await new WaitForSeconds(Inst.Delay);
            return StmtResult.None;
        }
    }

    public class Change : Stmt
    {
        public ChangeType ChangeType;

        public override async Task<StmtResult> Eval()
        {
            Inst.CommandMgr.Change(ChangeType);
            await new WaitForSeconds(Inst.Delay);
            return StmtResult.None;
        }
    }

    public static Interpreter Inst;

    public float Delay = 1.0f;

    public ICommandManager CommandMgr { get; private set; }

    public async void Execute(Block program)
    {
        await program.Eval();
    }

    void Awake()
    {
        Inst = this;
    }

    void Start()
    {
        if (CommandManager.Inst != null)
            CommandMgr = CommandManager.Inst;
        else
            CommandMgr = DummyCommandManager.Inst;
    }
}
