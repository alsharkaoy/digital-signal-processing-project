using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }
        public List<KeyValuePair<float, float>> complex { get; set; }
        public override void Run()
        {
            //Declare Our Lists are needed
            List<float> output_samples = new List<float>();
            float real_value, imaginary_value;

            if (complex == null)
            {
                complex = new List<KeyValuePair<float, float>>(InputFreqDomainSignal.Frequencies.Count);
                //loop on all frequencies to get their real and imaginary values.
                for (int k = 0; k < InputFreqDomainSignal.Frequencies.Count(); k++)
                {
                    //real value = freq amplitude * cos(phaseshift). 
                    real_value = (float)(InputFreqDomainSignal.FrequenciesAmplitudes[k] * Math.Cos(InputFreqDomainSignal.FrequenciesPhaseShifts[k]));
                    //imaginary value = freq amplitude * sin(phaseshift). 
                    imaginary_value = (float)(InputFreqDomainSignal.FrequenciesAmplitudes[k] * Math.Sin(InputFreqDomainSignal.FrequenciesPhaseShifts[k]));
                    //insert real and imaginary values in list of keyvalue complex(key=real,value=imaginary).
                    complex.Add(new KeyValuePair<float, float>(real_value, imaginary_value));
                }
            }

            for (int s = 0; s < InputFreqDomainSignal.Frequencies.Count(); s++)
            {
                float real_sum = 0;
                float imaginary_sum = 0;
                //loop on all freq components to get value of the sample 
                for (int k = 0; k < InputFreqDomainSignal.Frequencies.Count(); k++)
                {
                    float temp_power = (float)((2 * Math.PI * k * s) / InputFreqDomainSignal.Frequencies.Count());
                    //sum real part 
                    real_sum += (float)(complex[k].Key * Math.Cos(temp_power));
                    //sum imaginary part
                    imaginary_sum += (float)(complex[k].Value * Math.Sin(temp_power));
                }
                //sample value
                output_samples.Add((float)((real_sum - imaginary_sum) / InputFreqDomainSignal.Frequencies.Count()));
            }
            OutputTimeDomainSignal = new Signal(output_samples, false);
        }
    }
}