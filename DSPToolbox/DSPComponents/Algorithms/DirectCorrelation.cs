using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            bool nuu = false;
            List<decimal> out_not_norm = new List<decimal>();
            // out_not_norm = InputSignal1.Samples;
            List<decimal> l1 = new List<decimal>();
            List<decimal> l2 = new List<decimal>();

            float num = 0;
            bool p = InputSignal1.Periodic;

            if (InputSignal2 == null)
            {
                nuu = true;
                //auto correlation;
                //Input signal 1= input signal 2
                InputSignal2 = new Signal(new List<float>(), InputSignal1.Periodic);
                for (int i = 0; i < InputSignal1.Samples.Count; ++i)
                {
                    InputSignal2.Samples.Add((float)Math.Round((decimal)InputSignal1.Samples[i], 4));

                }
                for (int i = 0; i < InputSignal1.Samples.Count; ++i)
                {
                    l1.Add((decimal)InputSignal1.Samples[i]);
                    l2.Add((decimal)InputSignal1.Samples[i]);


                }


            }
            else
            {
                //cross correlation
                for (int i = 0; i < InputSignal1.Samples.Count; ++i)
                {
                    l1.Add((decimal)InputSignal1.Samples[i]);


                }
                for (int i = 0; i < InputSignal2.Samples.Count; ++i)
                {
                    l2.Add((decimal)InputSignal2.Samples[i]);


                }
            }
            int j = 0;
            if (l1.Count != l2.Count)
            {
                if (p == false)
                //non periodic
                {
                    int diff = Math.Max(InputSignal1.Samples.Count, InputSignal2.Samples.Count) -
                               Math.Min(InputSignal1.Samples.Count, InputSignal2.Samples.Count);
                    for (int i = 0; i < diff; ++i)
                    {
                        if (l1.Count < l2.Count)
                            //complete samples by zeros
                            l1.Add(0);
                        else if (l1.Count > l2.Count)
                        {
                            l2.Add(0);
                        }

                    }
                }
                else
                {
                    //if periodic
                    //N1+N2-1
                    int s = (InputSignal1.Samples.Count + InputSignal2.Samples.Count) - 1;
                    for (int i = 0; i < s; ++i)
                    {
                        if (l1.Count < s)
                            l1.Add(0);
                        if (l2.Count < s)
                        {
                            l2.Add(0);
                        }

                    }

                }

            }

            int size = l1.Count;
            for (int i = 0; i < size; ++i)
                out_not_norm.Add(0);
            while (j < size)
            {
                num = 0;
                for (int i = 0; i < size; ++i)
                //1/N summation x1(n)*x2👎
                {
                    out_not_norm[j] += ((l1[i] * l2[i]));

                }

                out_not_norm[j] /= size;
                if (p == true)
                {
                    decimal tmp = l2[0];
                    for (int i = 0; i < l2.Count - 1; ++i)
                    {
                        //shifted left by 1
                        l2[i] = l2[i + 1];

                    }
                    //periodic so l2[last index]=l2[0]
                    l2[l2.Count - 1] = tmp;
                }
                else
                {

                    for (int i = 0; i < l2.Count - 1; ++i)
                    {
                        l2[i] = l2[i + 1];

                    }
                    //non periodic
                    l2[l2.Count - 1] = 0;
                }

                ++j;
            }

            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();

            for (int i = 0; i < out_not_norm.Count; ++i)
            {
                OutputNonNormalizedCorrelation.Add((float)out_not_norm[i]);

            }

            if (nuu == true)
            //auto correlation
            {
                for (int i = 0; i < InputSignal1.Samples.Count; ++i)
                {
                    l1[i] = ((decimal)InputSignal1.Samples[i]);
                    l2[i] = ((decimal)InputSignal1.Samples[i]);


                }
            }
            else
            {
                for (int i = 0; i < InputSignal1.Samples.Count; ++i)
                {
                    l1[i] = ((decimal)InputSignal1.Samples[i]);


                }
                for (int i = 0; i < InputSignal2.Samples.Count; ++i)
                {
                    l2[i] = ((decimal)InputSignal2.Samples[i]);


                }
            }
            //normalization=summation x(n)y(n)/square root (summation x^2(n)*summation y^2(n))
            decimal sum1 = 0;
            decimal sum2 = 0;
            for (int i = 0; i < l1.Count; ++i)
            {
                sum1 += (l1[i] * l1[i]);
            }
            for (int i = 0; i < l2.Count; ++i)
            {
                sum2 += (l2[i] * l2[i]);
            }

            decimal norm = (decimal)Math.Sqrt((double)((sum2 * sum1)));
            norm /= size;
            if (norm != 0)
                for (int i = 0; i < out_not_norm.Count; ++i)
                {
                    OutputNormalizedCorrelation.Add((float)(out_not_norm[i] / norm));
                }

            // throw new NotImplementedException();

        }
    }
}