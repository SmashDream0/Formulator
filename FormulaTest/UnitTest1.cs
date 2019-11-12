using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Formulator;
using System.Collections.Generic;
using Formulator.Variations;

namespace FormulaTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TextSingleVarriable()
        {
            var textFormula = "asd";

            var formula = new Formula();

            formula.FormulaText = textFormula;

            var varriables = formula.Varraibles.ToArray();
            varriables[0].SetValue(1);

            if (formula.Summ != 1)
            { throw new Exception("Error result: 1"); }
        }
        [TestMethod]
        public void TextMultyVarriable()
        {
            var textFormula = "asd+14+s";

            var formula = new Formula();

            formula.FormulaText = textFormula;

            var varriables = formula.Varraibles.ToArray();
            varriables[0].SetValue(1);
            varriables[1].SetValue(2);

            if (formula.Summ != 17)
            { throw new Exception("Error result: 17"); }
        }
        [TestMethod]
        public void TextScobeMultiVarriable()
        {
            {
                var textFormula = "asd+14+s-(s-5/4)";

                var formula = new Formula();

                formula.FormulaText = textFormula;

                var varriables = formula.Varraibles.ToArray();
                varriables[0].SetValue(1);
                varriables[1].SetValue(2);

                CheckCalc(formula, textFormula, 16.25m);
            }

            {
                var textFormula = "asd*(12/(s-1))-(s-5/4)";

                var formula = new Formula();

                formula.FormulaText = textFormula;

                var varriables = formula.Varraibles.ToArray();
                varriables[0].SetValue(2);
                varriables[1].SetValue(3);

                CheckCalc(formula, textFormula, 10.25m);
            }

            {
                var textFormula = "(FK-DK)/DK*Tr*Q";

                var formula = new Formula();

                formula.FormulaText = textFormula;

                var varriables = formula.Varraibles.ToArray();
                varriables[0].SetValue(831);
                varriables[1].SetValue(240);
                varriables[2].SetValue(18.2m);
                varriables[3].SetValue(778);
                
                CheckCalc(formula, textFormula, 34868.01500m);
            }

            {
                var textFormula = "(FK-DK)*Q/1000*TP*Kf";

                var formula = new Formula();

                formula.FormulaText = textFormula;

                var varriables = formula.Varraibles.ToArray();
                varriables[0].SetValue(2);
                varriables[1].SetValue(3);
                varriables[2].SetValue(4);
                varriables[3].SetValue(5);
                varriables[4].SetValue(6);

                CheckCalc(formula, textFormula, -0.12m);
            }
        }
        [TestMethod]
        public void TextToStringTest()
        {
            var textFormula = "asd*(12/(s-1))-(s-5/4)";

            var formula = new Formula();

            formula.FormulaText = textFormula;

            if (formula.CleanFormula != textFormula)
            { throw new Exception($"Formula don't match: {formula.CleanFormula} and {textFormula}"); }
        }

        private static void CheckCalc(Formula formula, string textFormula, decimal checkValue)
        {
            if (formula.CleanFormula != textFormula.Replace(" ", String.Empty))
            { throw new Exception($"Formula don't match: {formula.CleanFormula} and {textFormula}"); }

            var temp = formula.CleanDigits;

            if (formula.Summ != checkValue)
            { throw new Exception($"Error result: {checkValue}"); }
        }

        [TestMethod]
        public void OrderTest1()
        {
            //+2-2*2/3+4+4*2/3
            var operations = new List<CalcOperation>();
            operations.Add(new CalcOperation(CalcOperation.EOperation.Plus, new Constant(2)));
            operations.Add(new CalcOperation(CalcOperation.EOperation.Minus, new Constant(2)));
            operations.Add(new CalcOperation(CalcOperation.EOperation.Multiply, new Constant(2)));
            operations.Add(new CalcOperation(CalcOperation.EOperation.Divide, new Constant(3)));
            operations.Add(new CalcOperation(CalcOperation.EOperation.Plus, new Constant(4)));
            operations.Add(new CalcOperation(CalcOperation.EOperation.Plus, new Constant(4)));
            operations.Add(new CalcOperation(CalcOperation.EOperation.Multiply, new Constant(2)));
            operations.Add(new CalcOperation(CalcOperation.EOperation.Divide, new Constant(3)));

            var orderProvider = new OrderOperaionProvider(operations);

            var resultOperations = orderProvider.Order();

            string result = String.Join(String.Empty, resultOperations);
            string compare = "-2*2/3+4*2/3+2+4";

            if (!String.Equals(result, compare))
            { throw new Exception($"{result} != {compare}"); }
        }

        [TestMethod]
        public void OrderTest2()
        {
            //+2-2*2/3+4+4
            var operations = new List<CalcOperation>();
            operations.Add(new CalcOperation(CalcOperation.EOperation.Plus, new Constant(2)));
            operations.Add(new CalcOperation(CalcOperation.EOperation.Minus, new Constant(2)));
            operations.Add(new CalcOperation(CalcOperation.EOperation.Multiply, new Constant(2)));
            operations.Add(new CalcOperation(CalcOperation.EOperation.Divide, new Constant(3)));
            operations.Add(new CalcOperation(CalcOperation.EOperation.Plus, new Constant(4)));
            operations.Add(new CalcOperation(CalcOperation.EOperation.Plus, new Constant(4)));

            var orderProvider = new OrderOperaionProvider(operations);

            var resultOperations = orderProvider.Order();

            string result = String.Join(String.Empty, resultOperations);
            string compare = "-2*2/3+2+4+4";

            if (!String.Equals(result, compare))
            { throw new Exception($"{result} != {compare}"); }
        }

        [TestMethod]
        public void InnerCalcTest()
        {
            var textFormula = "(FK-DK)*Q/1000*TP*Kf";

            var formula = new Formula();

            formula.FormulaText = textFormula;
            
            var innerFormula = new Formula();

            innerFormula.FormulaText = "1+2+3";

            var varriables = formula.Varraibles.ToArray();
            varriables[0].SetValue(2);
            varriables[1].SetValue(3);
            varriables[2].SetValue(4);
            varriables[3].SetValue(5);
            varriables[4].SetValue(6);

            formula.TrySetValue("Kf", innerFormula);

            if (formula.Summ != -0.12m)
            { throw new Exception($"Error result: {-0.12m}"); }
        }
    }
}
