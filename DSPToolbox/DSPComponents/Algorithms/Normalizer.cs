using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Normalizer : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputMinRange { get; set; }
        public float InputMaxRange { get; set; }
        public Signal OutputNormalizedSignal { get; set; }

        public override void Run()
        {
            List<float> output = new List<float>();
            float maxi = InputSignal.Samples.Max();
            float mini = InputSignal.Samples.Min();
            if (InputMinRange == 0)
            {

                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    float x = (InputSignal.Samples[i] - mini) / (maxi - mini);
                    output.Add(x);
                }
            }
            else
            {
                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    float x = (2 * ((InputSignal.Samples[i] - mini) / (maxi - mini))) - 1;
                    output.Add(x);
                }
            }
            OutputNormalizedSignal = new Signal(output, false);
        }
    }
}