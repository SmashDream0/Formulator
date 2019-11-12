using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulator
{
    public class Formula : ICloneable
    {
        public Formula()
        { }

        private string _formulaText;

        internal Variations.ACalc _calc = null;

        /// <summary>
        /// Текст формулы
        /// </summary>
        public string FormulaText
        {
            get { return _formulaText; }
            set
            {
                if (_formulaText != value)
                {
                    _formulaText = value;

                    ParseFormula(_formulaText);
                }
            }
        }

        public IEnumerable<Variations.Varriable> Varraibles => varriableDictionary.Values;

        private Dictionary<string, Variations.Varriable> varriableDictionary;

        public Variations.Varriable GetVarriable(string name)
        {
            Variations.Varriable varriable;

            varriableDictionary.TryGetValue(name, out varriable);

            return varriable;
        }

        public bool TrySetValue(string name, Formula value)
        {
            var varriable = GetVarriable(name);

            if (varriable != null)
            {
                varriable.SetValue(value);
                return true;
            }
            else
            { return false; }
        }


        public bool TrySetValue(string name, decimal value)
        {
            var varriable = GetVarriable(name);

            if (varriable != null)
            {
                varriable.SetValue(value);
                return true;
            }
            else
            { return false; }
        }

        private void ParseFormula(string text)
        {
            var parseProvider = new ParseProvider(text);

            this._calc = parseProvider.ParseFormula();

            this.varriableDictionary = parseProvider.Varriables;
        }

        public decimal Summ
        {
            get
            {
                if (_calc == null)
                { return 0; }
                else
                { return _calc.Value; }
            }
        }

        public bool CanCalculate => _calc.CanCalculate;

        public string CleanFormula
        {
            get
            {
                var sb = new StringBuilder();

                _calc.CleanFormula(false, sb);

                return sb.ToString();
            }
        }
        public string CleanDigits
        {
            get
            {
                var sb = new StringBuilder();

                _calc.CleanDigits(false, sb);

                return sb.ToString();
            }
        }

        /// <summary>
        /// Не работает сейчас
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            var cloneFormula = new Formula();

            //cloneFormula._calc = this._calc.Clone() as Variations.ACalc;
            

            return cloneFormula;
        }

        public override string ToString()
        {
            return _calc.ToString();
        }
    }
}
