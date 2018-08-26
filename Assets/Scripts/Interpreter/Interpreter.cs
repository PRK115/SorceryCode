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
        public virtual async Task<StmtResult> Eval(EvalContext context)
        {
            if (Inst.CancelAllPrograms)
            {
                throw new CancelException();
            }
            return StmtResult.None;
        }
    }

    public enum StmtResult
    {
        None, Break
    }

    public class LangException : Exception
    {
        public LangException(string message) : base(message) { }
    }

    public class CancelException : LangException
    {
        public CancelException() : base("Cancelling running code.") { }
    }

    public class NullTargetException : LangException
    {
        public NullTargetException(string command) : base($"Target not specified for command {command}") { }
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
            await base.Eval(context);
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
            await base.Eval(context);
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
            await base.Eval(context);
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
            await base.Eval(context);
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
            await base.Eval(context);
            return StmtResult.Break;
        }
    }

    public class Conjure : Stmt
    {
        public EntityType Entity;

        public override async Task<StmtResult> Eval(EvalContext context)
        {
            await base.Eval(context);
            Inst.CommandMgr.Conjure(context, Entity);
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
            await base.Eval(context);
            if (context.Target == null)
            {
                throw new NullTargetException("Change");
            }
            if (Adjective.IsLeft)
            {
                Inst.CommandMgr.Change(context, Adjective.Left);
            }
            else
            {
                Inst.CommandMgr.Change(context, Adjective.Right);
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
            await base.Eval(context);
            if (context.Target == null)
            {
                throw new NullTargetException("Move");
            }
            Inst.CommandMgr.Move(context, Dir, Distance);
            await new WaitForSeconds(Inst.Delay);
            Inst.Nounce++;
            return StmtResult.None;
        }
    }

    public static Interpreter Inst;

    public float Delay = 1.1f;

    public ICommandManager CommandMgr { get; private set; }

    public int Nounce { get; set; } = 0;

    public int ExecutingPrograms { get; set; } = 0;

    public bool CancelAllPrograms { get; private set; } = false;

    public async void Execute(EvalContext context, Block program)
    {
        ExecutingPrograms++;
        try
        {
            await program.Eval(context);
        }
        catch (CancelException ex)
        {
            Debug.Log(ex);
        }
        catch (NullTargetException ex)
        {
            Debug.Log(ex);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
        finally
        {
            ExecutingPrograms--;
        }
    }

    public async void CancelAll()
    {
        CancelAllPrograms = true;
        await new WaitUntil(() => ExecutingPrograms == 0);
        CancelAllPrograms = false;
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