using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }


        public override void Run()
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

            DiscreteFourierTransform Dft1 = new DiscreteFourierTransform();
            Dft1.InputTimeDomainSignal = InputSignal1;
            Dft1.Run();
            DiscreteFourierTransform Dft2 = new DiscreteFourierTransform();
            Dft2.InputTimeDomainSignal = InputSignal2;
            Dft2.Run();

            OutputConvolvedSignal = new Signal(InputSignal1.Periodic, Dft1.OutputFreqDomainSignal.Frequencies, Dft1.OutputFreqDomainSignal.FrequenciesAmplitudes, Dft1.OutputFreqDomainSignal.FrequenciesPhaseShifts);
            OutputConvolvedSignal.FrequenciesAmplitudes = new List<float>();
            OutputConvolvedSignal.FrequenciesPhaseShifts = new List<float>();
            List<KeyValuePair<float, float>> complex = new List<KeyValuePair<float, float>>();

            for (int i = 0; i < InputSignal1.Samples.Count(); i++)
            {
                OutputConvolvedSignal.FrequenciesAmplitudes.Add(Dft1.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * Dft2.OutputFreqDomainSignal.FrequenciesAmplitudes[i]);
                OutputConvolvedSignal.FrequenciesPhaseShifts.Add(Dft1.OutputFreqDomainSignal.FrequenciesPhaseShifts[i] * Dft2.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]);
                float real = Dft1.complex[i].Key * Dft2.complex[i].Key - Dft1.complex[i].Value * Dft2.complex[i].Value;
                float imagenary = Dft1.complex[i].Key * Dft2.complex[i].Value + Dft1.complex[i].Value * Dft2.complex[i].Key;
                complex.Add(new KeyValuePair<float, float>(real, imagenary));
            }

            InverseDiscreteFourierTransform idft = new InverseDiscreteFourierTransform();
            idft.InputFreqDomainSignal = OutputConvolvedSignal;
            idft.complex = complex;
            idft.Run();
            OutputConvolvedSignal.Samples = idft.OutputTimeDomainSignal.Samples;
        }
    }
}