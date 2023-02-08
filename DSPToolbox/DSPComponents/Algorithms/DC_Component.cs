using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DC_Component : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            float Sum = 0.0f;

            //calculate Sum
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                Sum += InputSignal.Samples[i];
            }

            //calculate Mean
            float Mean = (Sum / InputSignal.Samples.Count);

            List<float> result = new List<float>();

            //calculate the formula
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                result.Add(InputSignal.Samples[i] - Mean);
            }

            OutputSignal = new Signal(result, false);
        }
    }
}
