using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Formulator.Variations
{
    /// <summary>
    /// Операция
    /// </summary>
    public class Operations : ACalc
    {
        public Operations(IEnumerable<CalcOperation> calc)
        {
            _calcs = calc;

            var orderOperationsProvider = new OrderOperaionProvider(calc);

            _orderedCalcs = orderOperationsProvider.Order();
        }

        public override decimal Value
        {
            get
            {
                decimal result = 0;

                foreach (var operationCalc in _orderedCalcs)
                {
                    switch (operationCalc.Operation)
                    {
                        case CalcOperation.EOperation.Divide:
                            result /= operationCalc.Calc.Value;
                            break;
                        case CalcOperation.EOperation.Minus:
                            result = result - operationCalc.Calc.Value;
                            break;
                        case CalcOperation.EOperation.Multiply:
                            result *= operationCalc.Calc.Value;
                            break;
                        case CalcOperation.EOperation.Plus:
                            result += operationCalc.Calc.Value;
                            break;
                        default:
                            throw new Exception($"Unknown operation: {operationCalc.Operation}");
                    }
                }

                return result;
            }
        }

        private IEnumerable<CalcOperation> _calcs;

        private IEnumerable<CalcOperation> _orderedCalcs;

        public override bool CanCalculate
        {
            get
            {
                foreach (var calc in _calcs)
                {
                    if (!calc.Calc.CanCalculate)
                    { return false; }
                }

                return true;
            }
        }

        public override void CleanFormula(bool appendScope, StringBuilder sb)
        {
            if (appendScope)
            { sb.Append('('); }

            bool first = true;
            foreach (var operation in _calcs)
            {
                if (operation.Operation == CalcOperation.EOperation.Minus || !first)
                { sb.Append(CalcOperation.GetStringOperation(operation.Operation)); }

                operation.Calc.CleanFormula(true, sb);

                first = false;
            }

            if (appendScope)
            { sb.Append(')'); }
        }

        public override void CleanDigits(bool appendScope, StringBuilder sb)
        {
            if (appendScope)
            { sb.Append('('); }

            bool first = true;
            foreach (var operation in _calcs)
            {
                if (operation.Operation == CalcOperation.EOperation.Minus || !first)
                { sb.Append(CalcOperation.GetStringOperation(operation.Operation)); }

                operation.Calc.CleanDigits(true, sb);

                first = false;
            }

            if (appendScope)
            { sb.Append(')'); }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append('(');

            bool first = true;
            foreach (var operation in _calcs)
            {
                if (operation.Operation == CalcOperation.EOperation.Minus || !first)
                { sb.Append(CalcOperation.GetStringOperation(operation.Operation)); }

                sb.Append(operation.Calc.ToString());

                first = false;
            }

            sb.Append(')');

            return sb.ToString();
        }
    }
}