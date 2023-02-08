using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;


namespace DSPAlgorithms.Algorithms
{
    public class AccumulationSum : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();

            List<float> outputSamples = new List<float>();

            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                float sum = 0;
                for (int j = 0; j <= i; j++)
                {
                    sum += InputSignal.Samples[j];
                }
                outputSamples.Add(sum);
            }
            OutputSignal = new Signal(outputSamples, false);
        }
    }
}
