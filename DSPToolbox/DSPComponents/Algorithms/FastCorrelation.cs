using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            bool flag = false;
            if (InputSignal2 == null)
            {
                InputSignal2 = new Signal(new List<float>(), false);
                for (int i = 0; i < InputSignal1.Samples.Count(); i++)
                {
                    InputSignal2.Samples.Add(InputSignal1.Samples[i]);
                }
                flag = true;
            }

            if (InputSignal1.Samples.Count != InputSignal2.Samples.Count())
            {
                int size = InputSignal1.Samples.Count + InputSignal2.Samples.Count() - 1;
                int c1 = size - InputSignal1.Samples.Count();
                int c2 = size - InputSignal2.Samples.Count();

                for (int i = 0; i < c1; i++)
                {
                    InputSignal1.Samples.Add(0);
                }
                for (int i = 0; i < c2; i++)
                {
                    InputSignal2.Samples.Add(0);
                }
            }

            DiscreteFourierTransform Dft1 = new DiscreteFourierTransform();
            Dft1.InputTimeDomainSignal = InputSignal1;
            Dft1.Run();
            DiscreteFourierTransform Dft2 = new DiscreteFourierTransform();
            Dft2.InputTimeDomainSignal = InputSignal2;
            Dft2.Run();

            Signal Output = new Signal(InputSignal1.Periodic, Dft1.OutputFreqDomainSignal.Frequencies, Dft1.OutputFreqDomainSignal.FrequenciesAmplitudes, Dft1.OutputFreqDomainSignal.FrequenciesPhaseShifts);
            Output.FrequenciesAmplitudes = new List<float>();
            Output.FrequenciesPhaseShifts = new List<float>();
            List<KeyValuePair<float, float>> complex = new List<KeyValuePair<float, float>>();

            for (int i = 0; i < InputSignal1.Samples.Count(); i++)
            {
                if (flag == true)
                {
                    Output.FrequenciesAmplitudes.Add(Dft1.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * Dft2.OutputFreqDomainSignal.FrequenciesAmplitudes[i]);
                    Output.FrequenciesPhaseShifts.Add(Dft1.OutputFreqDomainSignal.FrequenciesPhaseShifts[i] * Dft2.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]);
                    float real = Dft1.complex[i].Key * Dft2.complex[i].Key - Dft1.complex[i].Value * -1 * Dft2.complex[i].Value;
                    float imagenary = Dft1.complex[i].Key * -1 * Dft2.complex[i].Value + Dft1.complex[i].Value * Dft2.complex[i].Key;
                    complex.Add(new KeyValuePair<float, float>(real, imagenary));
                }
                else
                {
                    Output.FrequenciesAmplitudes.Add(Dft1.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * Dft2.OutputFreqDomainSignal.FrequenciesAmplitudes[i]);
                    Output.FrequenciesPhaseShifts.Add(Dft1.OutputFreqDomainSignal.FrequenciesPhaseShifts[i] * Dft2.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]);
                    float real = Dft1.complex[i].Key * Dft2.complex[i].Key - Dft1.complex[i].Value * -1 * Dft2.complex[i].Value;
                    float imagenary = Dft1.complex[i].Key * Dft2.complex[i].Value + -1 * Dft1.complex[i].Value * Dft2.complex[i].Key;
                    complex.Add(new KeyValuePair<float, float>(real, imagenary));
                }
            }

            InverseDiscreteFourierTransform idft = new InverseDiscreteFourierTransform();
            idft.InputFreqDomainSignal = Output;
            idft.complex = complex;
            idft.Run();

            OutputNonNormalizedCorrelation = new List<float>();
            for (int i = 0; i < idft.OutputTimeDomainSignal.Samples.Count(); i++)
            {
                OutputNonNormalizedCorrelation.Add(idft.OutputTimeDomainSignal.Samples[i] / idft.OutputTimeDomainSignal.Samples.Count());
            }


            float sum_sq1 = 0, sum_sq2 = 0;
            for (int i = 0; i < InputSignal1.Samples.Count(); i++)
            {
                sum_sq1 += (float)Math.Pow(InputSignal1.Samples[i], 2);
                sum_sq2 += (float)Math.Pow(InputSignal2.Samples[i], 2);
            }

            float sum_squers = (float)Math.Sqrt(sum_sq1 * sum_sq2);
            float average = sum_squers / InputSignal1.Samples.Count();

            OutputNormalizedCorrelation = new List<float>();
            for (int i = 0; i < idft.OutputTimeDomainSignal.Samples.Count(); i++)
            {
                OutputNormalizedCorrelation.Add(OutputNonNormalizedCorrelation[i] / average);
            }
        }
    }
}