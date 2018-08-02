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
        public abstract IEnumerator Eval();
    }

    public class LangException : Exception
    {
        public LangException(string message) : base(message) { }
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

        public override IEnumerator Eval()
        {
            Expr.Eval();
            yield return new WaitForSeconds(Inst.Delay);
        }
    }

    public class Block : Stmt
    {
        public List<Stmt> Statements;

        public override IEnumerator Eval()
        {
            foreach (Stmt stmt in Statements)
            {
                yield return Inst.StartCoroutine(stmt.Eval());
            }
        }
    }

    public class If : Stmt
    {
        public Expr Cond;
        public Stmt Then;
        public Stmt Else;

        public override IEnumerator Eval()
        {
            object condResult = Cond.Eval();
            if (condResult is bool)
            {
                bool condValue = (bool) condResult;
                if (condValue)
                {
                    yield return Inst.StartCoroutine(Then.Eval());
                }
                else if (Else != null)
                {
                    yield return Inst.StartCoroutine(Else.Eval());
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

        public override IEnumerator Eval()
        {
            while (true)
            {
                yield return Inst.StartCoroutine(Body.Eval());
            }
        }
    }

    // TODO
    public class Enchant : Stmt
    {
        public Stmt Body;

        public override IEnumerator Eval()
        {
            yield return Inst.StartCoroutine(Body.Eval());
        }
    }

    // Same as "conjure"
    // TODO
    public class Create : Stmt
    {
        public Stmt Body;

        public override IEnumerator Eval()
        {
            // TODO
            yield return Inst.StartCoroutine(Body.Eval());
        }
    }

    public static Interpreter Inst;

    public float Delay = 1.0f;

    public void Execute(Block program)
    {
        Inst.StartCoroutine(program.Eval());
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
                                }
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
