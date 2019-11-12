using Formulator.Helpers;
using Formulator.Variations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formulator
{
    /// <summary>
    /// Парсер формулы
    /// </summary>
    public class ParseProvider
    {
        public ParseProvider(string text)
        {
            _text = text;
            _varriables = new Dictionary<string, Varriable>();
        }

        private readonly string _text;
        private readonly Dictionary<string, Varriable> _varriables;

        public Dictionary<string, Varriable> Varriables => _varriables;

        public ACalc ParseFormula()
        { return ParseFormula(_text); }

        private ACalc ParseFormula(string text)
        {
            int index = 0;

            ACalc result = ParseOperation(text, ref index);

            return result;
        }

        private Operations ParseOperation(string text, ref int index)
        {
            var operations = new List<CalcOperation>();
            CalcOperation.EOperation currentOperaion = CalcOperation.EOperation.Plus;

            for (int i = index; i < text.Length; i++)
            {
                var symbol = text[i];

                switch (symbol)
                {
                    case BlockVarriants.OpenScope:
                        i++;
                        var operation = ParseOperation(text, ref i);
                        operations.Add(new CalcOperation(currentOperaion, operation));
                        break;
                    case BlockVarriants.CloseScope:
                        index = i;
                        return new Operations(operations);
                    case BlockVarriants.Divide:
                        currentOperaion = CalcOperation.EOperation.Divide;
                        break;
                    case BlockVarriants.Multiply:
                        currentOperaion = CalcOperation.EOperation.Multiply;
                        break;
                    case BlockVarriants.Minus:
                        currentOperaion = CalcOperation.EOperation.Minus;
                        break;
                    case BlockVarriants.Plus:
                        currentOperaion = CalcOperation.EOperation.Plus;
                        break;
                    case ' ':
                    case '\t':
                        break;
                    default:
                        if (currentOperaion == CalcOperation.EOperation.None)
                        { throw new Exception("Parse error, can't find type of operation at " + i.ToString()); }

                        if (char.IsDigit(symbol))
                        {
                            var constant = ParseConstant(text, ref i);
                            operations.Add(new CalcOperation(currentOperaion, constant));
                        }
                        else if (char.IsLetter(symbol))
                        {
                            var varriable = ParseVarriable(text, ref i);
                            operations.Add(new CalcOperation(currentOperaion, varriable));
                        }
                        else
                        {
                            var newCalc = ParseOperation(text, ref i);
                            operations.Add(new CalcOperation(currentOperaion, newCalc));
                        }
                        currentOperaion = CalcOperation.EOperation.None;
                        break;
                }
            }

            if (!operations.Any())
            { throw new Exception($"Parse error, can't find any operands from {index} to {text.Length - 1}"); }

            index = text.Length;
            return new Operations(operations);
        }

        private Varriable ParseVarriable(string text, ref int index)
        {
            bool firstSymbol = true;

            for (int i = index; i < text.Length; i++)
            {
                var symbol = text[i];

                if (char.IsDigit(symbol))
                {
                    if (firstSymbol)
                    { firstSymbol = false; }
                    else
                    { throw new Exception("Parse error, first symbol or the operand can't be a digit. Error at " + i.ToString()); }
                }
                else if(symbol != '_' && !char.IsLetter(symbol))
                {
                    var varriable = ParseVarriable(text, index, i);

                    index = i - 1;

                    return varriable;
                }
            }
            {
                var varriable = ParseVarriable(text, index, text.Length);

                index = text.Length;

                return varriable;
            }
        }

        private Constant ParseConstant(string text, ref int index)
        {
            bool findedComma = false;
            bool sign = false;

            for (int i = index; i < text.Length; i++)
            {
                var symbol = text[i];

                if (symbol == ',' || symbol == '.')
                {
                    if (findedComma)
                    { throw new Exception("Parse error, double comma at " + i.ToString()); }

                    findedComma = true;
                    continue;
                }

                if (i == index)
                {
                    switch (symbol)
                    {
                        case BlockVarriants.Plus:
                        case BlockVarriants.Minus:
                            if (sign)
                            { throw new Exception("Parse error, double sign at" + i.ToString()); }
                            else
                            { sign = true; }
                            continue;
                    }
                }

                if (!char.IsDigit(symbol))
                {
                    var result = ParseConstant(text, index, i);
                    index = i - 1;
                    return result;
                }
            }
            {
                var result = ParseConstant(text, index, text.Length);
                index = text.Length;
                return result;
            }
        }

        private CalcOperation.EOperation MixOperaions(CalcOperation.EOperation newOperaion, CalcOperation.EOperation prevOperaion)
        {
            if (prevOperaion == CalcOperation.EOperation.None)
            { return newOperaion; }
            else
            {
                if (prevOperaion == newOperaion)
                {
                    switch (newOperaion)
                    {
                        case CalcOperation.EOperation.Divide:

                            break;
                        case CalcOperation.EOperation.Minus:
                            break;
                        case CalcOperation.EOperation.Multiply:
                            break;
                        case CalcOperation.EOperation.Plus:
                            break;
                    }
                }
            }

            return CalcOperation.EOperation.None;
        }

        private Constant ParseConstant(string text, int indexFrom, int indexTo)
        {
            var strConstant = text.Substring(indexFrom, indexTo - indexFrom);

            var dValue = Decimal.Parse(strConstant);

            return new Constant(dValue);
        }

        private Varriable ParseVarriable(string text, int indexFrom, int indexTo)
        {
            var varriableName = text.Substring(indexFrom, indexTo - indexFrom);
            Varriable varriable;

            if (_varriables.ContainsKey(varriableName))
            { varriable = _varriables[varriableName]; }
            else
            {
                varriable = new Varriable(varriableName);
                _varriables.Add(varriableName, varriable);
            }

            return varriable;
        }
    }
}