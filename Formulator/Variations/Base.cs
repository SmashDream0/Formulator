using System;
using System.Collections.Generic;
using System.Text;

namespace Formulator.Variations
{
    public abstract class ACalc
    {
        /// <summary>
        /// Рассчетное значение
        /// </summary>
        public abstract decimal Value { get; }

        /// <summary>
        /// Могу ли рассчитывать
        /// </summary>
        public abstract bool CanCalculate { get; }

        /// <summary>
        /// Получить чистую формулу
        /// </summary>
        public abstract void CleanFormula(bool appendScope, StringBuilder sb);

        /// <summary>
        /// Получить чистые числа, если возможно
        /// </summary>
        public abstract void CleanDigits(bool appendScope, StringBuilder sb);
    }
}
