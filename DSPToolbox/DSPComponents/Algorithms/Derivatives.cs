using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Derivatives : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal FirstDerivative { get; set; }
        public Signal SecondDerivative { get; set; }

        public override void Run()
        {

            //sharping

            //FirstDerivative
            List<float> first_derivative = new List<float>();

            for (int i = 1; i < InputSignal.Samples.Count; i++)
            {
                first_derivative.Add(InputSignal.Samples[i] - InputSignal.Samples[i - 1]);
            }

            FirstDerivative = new Signal(first_derivative, false);

            //SecondDerivative
            List<float> second_derivative = new List<float>();

            //calculate first sample
            second_derivative.Add((-2 * InputSignal.Samples[0]) + InputSignal.Samples[1]);

            for (int i = 1; i < (InputSignal.Samples.Count - 1); i++)
            {
                second_derivative.Add((-2 * InputSignal.Samples[i]) + InputSignal.Samples[i + 1] + InputSignal.Samples[i - 1]);
            }

            SecondDerivative = new Signal(second_derivative, false);


        }
    }
}
