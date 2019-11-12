using System;
using System.Collections.Generic;
using System.Text;

namespace Formulator.Variations
{
    /// <summary>
    /// Переменная
    /// </summary>
    public class Varriable : ACalc
    {
        public Varriable(string name)
        { this.Name = name; }

        private ACalc _innerCalc;

        /// <summary>
        /// Значение переменной
        /// </summary>
        public override decimal Value
        {
            get
            {
                if (CanCalculate)
                { return _innerCalc.Value; }
                else
                { throw new Exception($"Not set value for varriable name {Name}"); }
            }
        }

        /// <summary>
        /// Имя переменной
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Занести значение переменной
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(decimal value)
        { _innerCalc = new Constant(value); }

        public void SetValue(Formula calc)
        { _innerCalc = calc._calc; }

        public override bool CanCalculate => _innerCalc != null;

        public override void CleanFormula(bool appendScope, StringBuilder sb)
        {
            if (!CanCalculate || _innerCalc is Constant)
            { sb.Append(Name); }
            else
            { _innerCalc.CleanFormula(appendScope, sb); }
        }
        public override void CleanDigits(bool appendScope, StringBuilder sb)
        { _innerCalc.CleanDigits(appendScope, sb); }

        public override string ToString()
        {
            if (CanCalculate)
            {
                return $"{Name}[{_innerCalc.ToString()}]";
            }
            else
            { return $"{Name}[]"; }
        }
    }
}
