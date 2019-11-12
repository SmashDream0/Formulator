using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formulator.Variations
{
    public class Constant : ACalc
    {
        public Constant(decimal value)
        { _value = value; }

        private readonly decimal _value;
        public override decimal Value => _value;
        public override void CleanFormula(bool appendScope, StringBuilder sb)
        { sb.Append(_value); }
        public override void CleanDigits(bool appendScope, StringBuilder sb)
        { sb.Append(_value); }

        public override bool CanCalculate => true;

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}