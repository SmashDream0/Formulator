using Formulator.Variations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formulator
{
    public class OrderOperaionProvider
    {
        public OrderOperaionProvider(IEnumerable<CalcOperation> calcs)
        { _calcs = calcs; }

        private IEnumerable<CalcOperation> _calcs;

        public IEnumerable<CalcOperation> Order()
        {
            var calcSequences = new Dictionary<EGroupType, List<CalcOperation>>();

            CalcOperation prevCalc = null;
            int sequencePlusOrMinusCount = 0;

            foreach (var calc in _calcs)
            {
                if (calc.Operation == CalcOperation.EOperation.Multiply || calc.Operation == CalcOperation.EOperation.Divide)
                {
                    if (prevCalc != null)
                    { AddToSequence(calcSequences, EGroupType.MD, prevCalc); }

                    AddToSequence(calcSequences, EGroupType.MD, calc);

                    prevCalc = null;

                    sequencePlusOrMinusCount = 0;

                    continue;
                }
                else
                {
                    if (sequencePlusOrMinusCount > 0)
                    { AddToSequence(calcSequences, EGroupType.PM, prevCalc); }

                    sequencePlusOrMinusCount++;
                }

                prevCalc = calc;
            }

            if (prevCalc != null)
            { AddToSequence(calcSequences, EGroupType.PM, prevCalc); }

            var result = calcSequences.OrderBy(x => x.Key).SelectMany(x => x.Value).ToArray();

            return result;
        }

        private void AddToSequence(Dictionary<EGroupType, List<CalcOperation>> calcSequences, EGroupType groupType, CalcOperation calc)
        {
            if (!calcSequences.ContainsKey(groupType))
            { calcSequences.Add(groupType, new List<CalcOperation>()); }
            
            calcSequences[groupType].Add(calc);
        }

        private enum EGroupType : byte { MD = 0, PM = 1 }
    }
}