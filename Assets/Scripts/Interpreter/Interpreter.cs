using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using CodeUI;
using UnityEngine;

public class Interpreter : MonoBehaviour
{
    public abstract class Expr
    {
        public abstract object Eval();
    }

    public abstract class Stmt
    {
        public delegate void Continuation(StmtResult result);

        public abstract IEnumerator Eval(Continuation cont);
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

        public override IEnumerator Eval(Continuation cont)
        {
            Expr.Eval();
            yield return new WaitForSeconds(Inst.Delay);
        }
    }

    public class Block : Stmt
    {
        public List<Stmt> Statements;

        public override IEnumerator Eval(Continuation cont)
        {
            foreach (Stmt stmt in Statements)
            {
                yield return Inst.StartCoroutine(stmt.Eval(cont));
            }
        }
    }

    public class If : Stmt
    {
        public Expr Cond;
        public Stmt Then;
        public Stmt Else;

        public override IEnumerator Eval(Continuation cont)
        {
            object condResult = Cond.Eval();
            if (condResult is bool)
            {
                bool condValue = (bool) condResult;
                if (condValue)
                {
                    yield return Inst.StartCoroutine(Then.Eval(cont));
                }
                else if (Else != null)
                {
                    yield return Inst.StartCoroutine(Else.Eval(cont));
                }
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

        public override IEnumerator Eval(Continuation cont)
        {
            while (true)
            {
                StmtResult result = StmtResult.None;
                yield return Body.Eval(newResult => result = newResult);
                if (result == StmtResult.Break)
                {
                    break;
                }
            }
        }
    }

    public class Break : Stmt
    {
        public override IEnumerator Eval(Continuation cont)
        {
            cont(StmtResult.Break);
            yield return StmtResult.Break;
        }
    }

    public class Conjure : Stmt
    {
        public EntityType Entity;

        public override IEnumerator Eval(Continuation cont)
        {
            Inst.CommandMgr.Conjure(Entity);
            yield return new WaitForSeconds(Inst.Delay);
        }
    }

    public class Change : Stmt
    {
        public ChangeType ChangeType;

        public override IEnumerator Eval(Continuation cont)
        {
            Inst.CommandMgr.Change(ChangeType);
            yield return new WaitForSeconds(Inst.Delay);
        }
    }

    public static Interpreter Inst;

    public float Delay = 1.0f;

    public ICommandManager CommandMgr { get; private set; }

    public void Execute(Block program)
    {
        Inst.StartCoroutine(program.Eval(_ => { }));
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

        Func<object, object> Test1 = arg =>
        {
            Debug.Log("Test1 Called!");
            return null;
        };

        Func<object, object> Test2 = arg =>
        {
            Debug.Log("Test2 Called!");
            return null;
        };

        Func<object, object> Test3 = arg =>
        {
            Debug.Log("Test3 Called!");
            return null;
        };

        Block Program = new Block
        {
            Statements = new List<Stmt>()
            {
                new Repeat
                {
                    Body = new If
                    {
                        Cond = new BoolExpr() { Value = true },
                        Then = new Block()
                        {
                            Statements = new List<Stmt>()
                            {
                                new Conjure()
                                {
                                    Entity = EntityType.Lion
                                },
                                new Conjure()
                                {
                                    Entity = EntityType.Mouse
                                },
                                new Break()
                            }
                        },
                        Else = new Conjure()
                        {
                            Entity = EntityType.Lion
                        },
                    }
                },
            }
        };

        Execute(Program);
    }
}
