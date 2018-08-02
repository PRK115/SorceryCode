using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
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

    public class LangCoroutine
    {
        public Coroutine Coroutine { get; private set; }
        public object Result;
        private IEnumerator target;

        public LangCoroutine(MonoBehaviour owner, IEnumerator target)
        {
            this.target = target;
            this.Coroutine = owner.StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            while (target.MoveNext())
            {
                Result = target.Current;
                yield return Result;
            }
        }
    }

    public class Object : Expr
    {
        public object Obj;

        public override object Eval() => Obj;
    }

    public class Call : Expr
    {
        public Func<object, object> Fun;
        public Expr Argument;

        public override object Eval() => Fun(Argument.Eval());
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

    // TODO
    public class Enchant : Stmt
    {
        public Stmt Body;

        public override IEnumerator Eval(Continuation cont)
        {
            yield return Inst.StartCoroutine(Body.Eval(cont));
        }
    }

    // Same as "conjure"
    // TODO
    public class Create : Stmt
    {
        public Stmt Body;

        public override IEnumerator Eval(Continuation cont)
        {
            // TODO
            yield return Inst.StartCoroutine(Body.Eval(cont));
        }
    }

    public static Interpreter Inst;

    public float Delay = 1.0f;

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
                        Cond = new Object() { Obj = true },
                        Then = new Block()
                        {
                            Statements = new List<Stmt>()
                            {
                                new Expression()
                                {
                                    Expr = new Call()
                                    {
                                        Argument = new Object() { Obj = 1 },
                                        Fun = Test1
                                    }
                                },
                                new Expression()
                                {
                                    Expr = new Call()
                                    {
                                        Argument = new Object() { Obj = 2 },
                                        Fun = Test2
                                    }
                                },
                                new Break()
                            }
                        },
                        Else = new Expression()
                        {
                            Expr = new Call()
                            {
                                Argument = new Object() { Obj = 1 },
                                Fun = Test3
                            }
                        }
                    }
                },
            }
        };

        Execute(Program);
    }
}
