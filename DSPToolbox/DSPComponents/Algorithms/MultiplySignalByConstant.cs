using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MultiplySignalByConstant : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputConstant { get; set; }
        public Signal OutputMultipliedSignal { get; set; }

        public override void Run()
        {
            float multplication = 1;
            List<float> samples = new List<float>();
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                multplication = InputConstant * InputSignal.Samples[i];
                samples.Add(multplication);
            }
            OutputMultipliedSignal = new Signal(samples, false);
        }
    }
}
