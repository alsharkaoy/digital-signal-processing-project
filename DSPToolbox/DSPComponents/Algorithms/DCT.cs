using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DCT : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            List<float> lst = new List<float>();
            float root = 0;
            float result;
            float harmonic;
            for (int k = 0; k < InputSignal.Samples.Count(); k++)
            {
                result = 0;
                root = (float)Math.Sqrt(2.0 / InputSignal.Samples.Count());

                for (int n = 0; n < InputSignal.Samples.Count(); n++)
                {
                    float sum = InputSignal.Samples[n] * (float)Math.Cos((Math.PI / (4 * InputSignal.Samples.Count())) * (2 * n - 1) * (2 * k - 1));
                    result += sum;
                }

                harmonic = root * result;
                lst.Add(harmonic);
            }
            OutputSignal = new Signal(lst, false);
        }
    }
}