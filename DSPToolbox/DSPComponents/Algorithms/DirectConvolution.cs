using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            List<float> out_sample = new List<float>();
            List<int> Sample_index = new List<int>();

            float sum = 0;

            for (int i = 0; i < InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1; i++)
            {
                for (int j = 0; j < InputSignal2.Samples.Count; j++)
                {
                    if (i - j >= 0 && i - j < InputSignal1.Samples.Count)
                    {
                        sum += InputSignal1.Samples[i - j] * InputSignal2.Samples[j];

                    }
                }
                if (i + 1 == InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1 && sum == 0)
                {
                    break;
                }
                out_sample.Add(sum);

                sum = 0;
                if (i == 0)
                {
                    int index = InputSignal1.SamplesIndices[i] + InputSignal2.SamplesIndices[i];
                    Sample_index.Add(index);
                }
                else
                {
                    int index = Sample_index.Last<int>(); // equals -3 then ++

                    Sample_index.Add((index + 1));
                }
            }
            OutputConvolvedSignal = new Signal(out_sample, Sample_index, false);
        }
    }
}