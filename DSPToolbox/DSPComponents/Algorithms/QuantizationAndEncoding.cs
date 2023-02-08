using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }
        public List<float> MidPoint { get; set; }
        public List<float> EndPoints { get; set; }

        public List<float> quantizedSignal { get; set; }


        public override void Run()
        {
            MidPoint = new List<float>();
            EndPoints = new List<float>();
            OutputSamplesError = new List<float>();
            OutputIntervalIndices = new List<int>();
            OutputEncodedSignal = new List<string>();
            quantizedSignal = new List<float>();

            float rangeOfLevel;
            float maximum = 0.0f;
            float minimum = 10000.0f;

            maximum = InputSignal.Samples.Max();
            minimum = InputSignal.Samples.Min();


            if (InputLevel == 0)
            {
                InputLevel = (int)Math.Pow(2, InputNumBits);
            }
            if (InputNumBits == 0)
            {
                InputNumBits = (int)Math.Log(InputLevel, 2);
            }


            rangeOfLevel = (maximum - minimum) / InputLevel;
            float x = minimum;
            EndPoints.Add(x);
            while (x <= maximum)
            {
                EndPoints.Add(x + rangeOfLevel);
                x = x + rangeOfLevel;
            }


            float mid_value;
            for (int k = 0; k < InputLevel; k++)
            {
                mid_value = (EndPoints[k] + EndPoints[k + 1]) / 2;
                MidPoint.Add(mid_value);
            }

            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                for (int j = 0; j < EndPoints.Count; j++)
                {
                    if (InputSignal.Samples[i] >= EndPoints[j] && InputSignal.Samples[i] <= EndPoints[j + 1] + 0.0001)
                    {
                        quantizedSignal.Add(MidPoint[j]);
                        OutputEncodedSignal.Add(Convert.ToString(j, 2).PadLeft(InputNumBits, '0'));

                        OutputIntervalIndices.Add(j + 1);
                        break;

                    }
                }

            }
            OutputQuantizedSignal = new Signal(quantizedSignal, false);

            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                float Quan_error = quantizedSignal[i] - InputSignal.Samples[i];
                OutputSamplesError.Add(Quan_error);
            }


        }
    }
}