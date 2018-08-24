using CodeUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Util;

public class Interpreter : MonoBehaviour
{
    public abstract class Expr
    {
        public abstract object Eval();
    }

    public abstract class Stmt
    {
        public virtual async Task<StmtResult> Eval()
        {
            if (Inst.CancelRequest)
            {
                Inst.IsRunning = false;
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
            await base.Eval();
            Expr.Eval();
            await new WaitForSeconds(Inst.Delay);
            Inst.Nounce++;
            return StmtResult.None;
        }
    }

    public class Block : Stmt
    {
        public List<Stmt> Statements;

        public override async Task<StmtResult> Eval()
        {
            await base.Eval();
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
            await base.Eval();
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
            await base.Eval();
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
            await base.Eval();
            return StmtResult.Break;
        }
    }

    public class Conjure : Stmt
    {
        public EntityType Entity;

        public override async Task<StmtResult> Eval()
        {
            await base.Eval();
            Inst.CommandMgr.Conjure(Entity);
            await new WaitForSeconds(Inst.Delay);
            Inst.Nounce++;
            return StmtResult.None;
        }
    }

    public class Change : Stmt
    {
        public Either<ChangeType, EntityType> Adjective;

        public override async Task<StmtResult> Eval()
        {
            await base.Eval();
            if (Adjective.IsLeft)
            {
                Inst.CommandMgr.Change(Adjective.Left);
            }
            else
            {
                Inst.CommandMgr.Change(Adjective.Right);
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

        public override async Task<StmtResult> Eval()
        {
            await base.Eval();
            Inst.CommandMgr.Move(Dir, Distance);
            await new WaitForSeconds(Inst.Delay);
            Inst.Nounce++;
            return StmtResult.None;
        }
    }

    public static Interpreter Inst;

    public float Delay = 1.1f;

    public ICommandManager CommandMgr { get; private set; }

    public int Nounce = 0;

    public bool IsRunning = false;
    public bool CancelRequest = false;

    public async void Execute(Block program, Action onStart)
    {
        if (IsRunning)
        {
            CancelRequest = true;
        }
        await new WaitUntil(() => !IsRunning);
        CancelRequest = false;

        onStart();
        IsRunning = true;
        try
        {
            await program.Eval();
        }
        catch (CancelException ex)
        {
            Debug.Log(ex);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
        finally
        {
            IsRunning = false;
        }
    }

    public async void Stop(Action onStop)
    {
        if (IsRunning)
        {
            CancelRequest = true;
        }
        await new WaitUntil(() => !IsRunning);
        onStop();
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