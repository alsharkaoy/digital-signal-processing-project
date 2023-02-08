using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }
        public List<KeyValuePair<float, float>> complex { get; set; }
        public override void Run()
        {
            //Declare Our Lists are needed
            List<float> output_signal_frequenciesAmplitudes = new List<float>();
            List<float> output_signal_frequenciesPhaseShifts = new List<float>();
            List<float> output_signal_frequencies = new List<float>();
            complex = new List<KeyValuePair<float, float>>();
            OutputFreqDomainSignal = new Signal(InputTimeDomainSignal.Samples, false);

            for (int k = 0; k < InputTimeDomainSignal.Samples.Count(); k++)
            {
                float real_value = 0; //real value var 
                float imaginary_value = 0; //imaginary value var

                //loop on all sample to get the freq component (real and imaginary)
                for (int n = 0; n < InputTimeDomainSignal.Samples.Count(); n++)
                {
                    if (k == 0 || n == 0) // if statisfied then freq component is real part only 
                    {
                        real_value += InputTimeDomainSignal.Samples[n];
                    }
                    else
                    {
                        int N = InputTimeDomainSignal.Samples.Count();
                        double temp_power = ((2 * Math.PI * k * n) / N);
                        
                        real_value += (float)(InputTimeDomainSignal.Samples[n] * Math.Cos(temp_power));
                        imaginary_value += (float)(-1 * InputTimeDomainSignal.Samples[n] * Math.Sin(temp_power));
                    }
                }
                float omega = (float)Math.Round(((2 * Math.PI * InputSamplingFrequency) / InputTimeDomainSignal.Samples.Count()),1);

                //genrate complex list
                complex.Add(new KeyValuePair<float, float>(real_value, imaginary_value));
                //freq value
                output_signal_frequencies.Add(k * omega);
                //freq amplitude
                output_signal_frequenciesAmplitudes.Add((float)Math.Sqrt(Math.Pow(complex[k].Key, 2) + Math.Pow(complex[k].Value, 2)));
                //phaseshift for freq
                output_signal_frequenciesPhaseShifts.Add((float)Math.Atan2(complex[k].Value, complex[k].Key));
            }
            OutputFreqDomainSignal = new Signal(false, output_signal_frequencies, output_signal_frequenciesAmplitudes, output_signal_frequenciesPhaseShifts);
        }
    }
}