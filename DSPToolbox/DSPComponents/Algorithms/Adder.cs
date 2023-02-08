using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Adder : Algorithm
    {
        public List<Signal> InputSignals { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            float sumofSamples = 0;
            List<float> samples = new List<float>();
            for (int i = 0; i < InputSignals.Count / 2; i += 2)
            {
                for (int j = 0; j < InputSignals[i].Samples.Count; j++)
                {
                    sumofSamples = InputSignals[i].Samples[j] + InputSignals[i + 1].Samples[j];
                    samples.Add(sumofSamples);
                }
            }
            OutputSignal = new Signal(samples, false);
        }
    }
}