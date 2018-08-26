using CodeUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Util;

public class EvalContext
{
    public Entity Target;
    public Vector3 Location;
}

public class Interpreter : MonoBehaviour
{
    public abstract class Expr
    {
        public abstract object Eval();
    }

    public abstract class Stmt
    {
        public abstract Task<StmtResult> Eval(EvalContext context);
    }

    public enum StmtResult
    {
        None, Break
    }

    public class LangException : Exception
    {
        public LangException(string message) : base(message) { }
    }

    public class BoolExpr : Expr
    {
        public bool Value;

        public override object Eval() => Value;
    }

    public class Expression : Stmt
    {
        public Expr Expr;

        public override async Task<StmtResult> Eval(EvalContext context)
        {
            Expr.Eval();
            await new WaitForSeconds(Inst.Delay);
            Inst.Nounce++;
            return StmtResult.None;
        }
    }

    public class Block : Stmt
    {
        public List<Stmt> Statements;

        public override async Task<StmtResult> Eval(EvalContext context)
        {
            foreach (Stmt stmt in Statements)
            {
                StmtResult result = await stmt.Eval(context);
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

        public override async Task<StmtResult> Eval(EvalContext context)
        {
            object condResult = Cond.Eval();
            if (condResult is bool)
            {
                bool condValue = (bool) condResult;
                if (condValue)
                {
                    return await Then.Eval(context);
                }
                if (Else != null)
                {
                    return await Else.Eval(context);
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

        public override async Task<StmtResult> Eval(EvalContext context)
        {
            while (true)
            {
                StmtResult result = await Body.Eval(context);
                if (result == StmtResult.Break)
                {
                    return StmtResult.None;
                }
            }
        }
    }

    public class Break : Stmt
    {
        public override async Task<StmtResult> Eval(EvalContext context)
        {
            return StmtResult.Break;
        }
    }

    public class Conjure : Stmt
    {
        public EntityType Entity;

        public override async Task<StmtResult> Eval(EvalContext context)
        {
            Inst.CommandMgr.Conjure(context.Location, Entity);
            await new WaitForSeconds(Inst.Delay);
            Inst.Nounce++;
            return StmtResult.None;
        }
    }

    public class Change : Stmt
    {
        public Either<ChangeType, EntityType> Adjective;

        public override async Task<StmtResult> Eval(EvalContext context)
        {
            if (Adjective.IsLeft)
            {
                Inst.CommandMgr.Change(context.Target, Adjective.Left);
            }
            else
            {
                Inst.CommandMgr.Change(context.Target, Adjective.Right);
            }
            await new WaitForSeconds(Inst.Delay);
            Inst.Nounce++;
            return StmtResult.None;
        }
    }

    public class Move : Stmt
    {
        public MoveDirection Dir;
        public int Distance;

        public override async Task<StmtResult> Eval(EvalContext context)
        {
            Inst.CommandMgr.Move(context.Target, Dir, Distance);
            await new WaitForSeconds(Inst.Delay);
            Inst.Nounce++;
            return StmtResult.None;
        }
    }

    public static Interpreter Inst;

    public float Delay = 1.1f;

    public ICommandManager CommandMgr { get; private set; }

    public int Nounce = 0;

    public async void Execute(EvalContext context, Block program)
    {
        await program.Eval(context);
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