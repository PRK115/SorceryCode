using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Interpreter
{
    abstract class Value
    {

    }

    class Null : Value
    {
    }

    class Bool : Value
    {
        public bool Contents { get; set; }
    }

    class Object : Value
    {
        public string Name { get; set; }
    }

    abstract class Node
    {
        public abstract Value Execute();
    }

    class ValueNode : Node
    {
        public Value Value;

        public override Value Execute()
        {
            return Value;
        }
    }

    class FnNode : Node
    {
        public string Name { get; set; }
        public Node Argument { get; set; }

        public Func<Value, Value> Fun { get; set; }

        public override Value Execute()
        {
            Value value = Argument.Execute();
            return Fun(value);
        }
    }

    class StmtNode : Node
    {
        public Node Current;
        public Node Next;

        public override Value Execute()
        {
            Current.Execute();
            Interpreter.Wait();
            return Next.Execute();
        }
    }

    class RepeatNode : Node
    {
        public Node Expr;

        public override Value Execute()
        {
            while (true)
            {
                Expr.Execute();
                Interpreter.Wait();
            }
        }
    }

    class IfNode : Node
    {
        public Node Cond;
        public Node Expr;

        public override Value Execute()
        {
            Value value = Cond.Execute();
            if (value is Bool)
            {
                Bool boolean = (Bool) value;
                if (boolean.Contents)
                {
                    Value result = Expr.Execute();
                    return result;
                }
                else
                {
                    return new Null();
                }
            }
            else
            {
                throw new Exception("Condition is not Bool");
            }
        }
    }

    class Interpreter
    {
        public static int Gas = 100;

        public Node RootNode;

        public static void Wait()
        {
            Debug.Log("Step increased");
            Gas--;
            if (Gas == 0)
            {
                throw new Exception("Not enough gas!");
            }
        }

        public void Execute()
        {
            RootNode.Execute();
        }
    }

    class Program
    {
        static Value GameMoveFunction(Value input)
        {
            if (input is Object)
            {
                Object obj = (Object) input;
                Debug.Log($"{obj.Name} is moved!");
                return new Null();
            }
            else
            {
                throw new Exception("Can't move something that is not an object");
            }
        }

        static void Main(string[] args)
        {
            Interpreter interpreter = new Interpreter();
            interpreter.RootNode = new IfNode()
            {
                Cond = new ValueNode()
                {
                    Value = new Bool() {Contents = true}
                },
                Expr = new FnNode()
                {
                    Name = "move",
                    Argument = new ValueNode()
                    {
                        Value = new Object() {Name = "Mouse"}
                    },
                    Fun = GameMoveFunction
                }
            };
            interpreter.Execute();
        }
    }
}
