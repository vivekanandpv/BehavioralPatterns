using System;
using System.Collections.Generic;

namespace Interpreter {
    internal class Program {
        static void Main(string[] args) {
            //  Setting up of context
            var context = new Context();

            //  One terminal symbol
            var input = new LiteralExpression();

            //  Building an expression
            var expression = new OrExpression {
                LValue = new EqualsExpression {
                    LValue = input,
                    RValue = new LiteralExpression { Value = "I" }
                },
                RValue = new EqualsExpression {
                    LValue = input,
                    RValue = new LiteralExpression { Value = "i" }
                }
            };

            //  Setting the value of the terminal symbol
            input.Value = "i";

            //  Letting the grammar rules interpret
            expression.Interpret(context);
            Console.WriteLine($"i == I or i == i --> {context.Result.Pop()}");

            input.Value = "x";
            expression.Interpret(context);
            Console.WriteLine($"x == I or x == i --> {context.Result.Pop()}");
        }
    }

    class Context {
        public Stack<string> Result = new Stack<string>();
    }

    //  Section 2
    //  Start Symbol
    interface Expression {
        void Interpret(Context context);
    }

    //  Section 3
    //  Definition of the grammar
    abstract class OperatorExpression : Expression {
        public Expression LValue { private get; set; }
        public Expression RValue { private get; set; }

        public void Interpret(Context context) {
            LValue.Interpret(context);
            string leftValue = context.Result.Pop();

            RValue.Interpret(context);
            string rightValue = context.Result.Pop();

            _Interpret(context, leftValue, rightValue);
        }

        protected abstract void _Interpret(Context context, string leftValue, string rightValue);
    }

    class EqualsExpression : OperatorExpression {
        protected override void _Interpret(Context context, string leftValue, string rightValue) {
            context.Result.Push(leftValue == rightValue ? "true" : "false");
        }
    }

    class OrExpression : OperatorExpression {
        protected override void _Interpret(Context context, string leftValue, string rightValue) {
            context.Result.Push(leftValue == "true" || rightValue == "true" ? "true" : "false");
        }
    }

    //  Section 1
    //  Terminal Symbol
    class LiteralExpression : Expression {
        public string Value { private get; set; }

        public void Interpret(Context context) {
            context.Result.Push(Value);
        }
    }
}
