using Formulator.Variations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formulator
{
    public class CalcOperation
    {
        public CalcOperation(EOperation operation, ACalc calc)
        {
            Operation = operation;
            Calc = calc;
        }

        public EOperation Operation
        { get; private set; }

        public ACalc Calc
        { get; private set; }

        public static string GetStringOperation(EOperation operation)
        {
            switch (operation)
            {
                case EOperation.Divide:
                    return "/";
                case EOperation.Minus:
                    return "-";
                case EOperation.Multiply:
                    return "*";
                case EOperation.Plus:
                    return "+";
                default:
                    throw new Exception("");
            }
        }

        public override string ToString()
        {
            return $"{GetStringOperation(Operation)}{Calc.ToString()}";
        }

        public enum EOperation
        {
            None = 0,
            Divide = 1,
            Multiply = 2,
            Plus = 3,
            Minus = 4,
        };
    }
}