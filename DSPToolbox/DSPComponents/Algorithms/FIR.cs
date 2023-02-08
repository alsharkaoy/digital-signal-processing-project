using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.IO;
namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; }  // Sampling freq
        public float? InputCutOffFrequency { get; set; } //HighLow Passband edge freq
        public float? InputF1 { get; set; } //BandStop 
        public float? InputF2 { get; set; } //BandStop 
        public float InputStopBandAttenuation { get; set; }
        public float InputTransitionBand { get; set; }  //Transition width
        public Signal OutputHn { get; set; }
        public Signal OutputYn { get; set; }

        /// <summary>
        ///  StopBand -> WindowType
        ///  WindowType -> N(coefficients no.)
        ///  N -> WindowValueRule
        ///  GetFilterType H(d)
        ///  H(n)->W(n)*H(d)
        ///  Y(n)->conv(H(n) & TimeDomain)
        /// </summary>
        public string getWindowType()
        {
            if (0 <= InputStopBandAttenuation && InputStopBandAttenuation <= 21)
                return "Rectangular";
            if (21 < InputStopBandAttenuation && InputStopBandAttenuation <= 44)
                return "Hanning";
            if (44 < InputStopBandAttenuation && InputStopBandAttenuation <= 53)
                return "Hamming";
            if (53 < InputStopBandAttenuation && InputStopBandAttenuation <= 74)
                return "Blackman";
            return "";
        }
        public double getFilterLength(string window_name)
        {
            double N = 0;
            if (window_name == "Rectangular")
                N = 0.9 * InputFS / InputTransitionBand;    // Normalize = N/(transition/sampling)
            if (window_name == "Hanning")
                N = 3.1 * InputFS / InputTransitionBand;
            if (window_name == "Hamming")
                N = 3.3 * InputFS / InputTransitionBand;
            if (window_name == "Blackman")
                N = 5.5 * InputFS / InputTransitionBand;
            N = Math.Round(N);  // Ceilling N roundup to ODD
            if (N % 2 == 0)
                N++;
            return N;
        }
        public double getWindowFunction(int n, string window_name, int N) // Given Window Rules
        {
            double w_value = 0;
            if (window_name == "Rectangular")
                w_value = 1;
            if (window_name == "Hanning")
                w_value = 0.5 + 0.5 * Math.Cos(2 * Math.PI * n / N);
            if (window_name == "Hamming")
                w_value = 0.54 + 0.46 * Math.Cos(2 * Math.PI * n / N);
            if (window_name == "Blackman")
                w_value = 0.42 + 0.5 * Math.Cos((2 * Math.PI * n) / (N - 1)) + 0.08 * Math.Cos((4 * Math.PI * n) / (N - 1));
            return w_value;
        }
        public double getHdLow_low_filters(int n, int sign)
        {
            //Smearing effect f` = f + delta(f/2)
            // Normalize = f` / sampling f
            double fc = (double)(InputCutOffFrequency + InputTransitionBand / 2) / InputFS;
            double Hd_value = 0;
            if (n == 0)
            {
                if (sign == 1) // Low Pass filter
                    Hd_value = 2 * fc;
                if (sign == -1) // High Pass filter
                    Hd_value = 1 - 2 * fc;
            }
            else  // sign * 2fc * sin(n*w)/n*w
                Hd_value = sign * (2 * fc * Math.Sin(n * 2 * Math.PI * fc) / (n * 2 * Math.PI * fc));
            return Hd_value;
        }

        public double getHdLow_High_filters(int n, int sign)
        {
            //Smearing effect f` = f + delta(f/2)
            // Normalize = f` / sampling f
            double fc = (double)(InputCutOffFrequency - InputTransitionBand / 2) / InputFS;
            double Hd_value = 0;
            if (n == 0)
            {
                if (sign == 1) // Low Pass filter
                    Hd_value = 2 * fc;
                if (sign == -1) // High Pass filter
                    Hd_value = 1 - 2 * fc;
            }
            else  // sign * 2fc * sin(n*w)/n*w
                Hd_value = sign * (2 * fc * Math.Sin(n * 2 * Math.PI * fc) / (n * 2 * Math.PI * fc));
            return Hd_value;
        }
        public double getHdBand_filters(int n, int sign)
        {
            double Hd_value = 0, fc1, fc2;
            fc1 = (double)(InputF1 - InputTransitionBand / 2) / InputFS;
            fc2 = (double)(InputF2 + InputTransitionBand / 2) / InputFS;
            if (n == 0)
            {
                if (sign == 1)  // Band Pass
                    Hd_value = 2 * (fc2 - fc1);
                if (sign == -1) // Band Stop
                    Hd_value = 1 - 2 * (fc2 - fc1);
            }
            else
                Hd_value = (sign * 2 * fc2 * Math.Sin(n * 2 * Math.PI * fc2) / (n * 2 * Math.PI * fc2)) - (sign * 2 * fc1 * Math.Sin(n * 2 * Math.PI * fc1) / (n * 2 * Math.PI * fc1));
            return Hd_value;
        }
        public double band_stop(int n, int sign)
        {
            double Hd_value = 0, fc1, fc2;
            fc1 = (double)(InputF1 + InputTransitionBand / 2) / InputFS;
            fc2 = (double)(InputF2 - InputTransitionBand / 2) / InputFS;
            if (n == 0)
            {
                if (sign == 1)  // Band Pass
                    Hd_value = 2 * (fc2 - fc1);
                if (sign == -1) // Band Stop
                    Hd_value = 1 - 2 * (fc2 - fc1);
            }
            else
                Hd_value = (sign * 2 * fc2 * Math.Sin(n * 2 * Math.PI * fc2) / (n * 2 * Math.PI * fc2)) - (sign * 2 * fc1 * Math.Sin(n * 2 * Math.PI * fc1) / (n * 2 * Math.PI * fc1));
            return Hd_value;
        }

        public List<double> get_full_Hn(int half_N, List<double> hD, List<double> W) //Symmetric and Odd
        {
            List<double> Hn_half = new List<double>();
            for (int i = 0; i <= half_N; i++)
                Hn_half.Add(hD[i] * W[i]);
            List<double> Hn = new List<double>(Hn_half);
            Hn.RemoveAt(0);
            Hn.Reverse();
            Hn.AddRange(Hn_half);
            return Hn;
        }
        public void apply_filter(int half_N, List<double> hD, List<double> W) // Add signal to output h(n),y(n)
        {
            List<float> samples = new List<float>();
            OutputHn = new Signal(samples, false);
            List<double> Hn = get_full_Hn(half_N, hD, W);
            int indexCount = -half_N;
            for (int i = 0; i < Hn.Count; i++)
            {
                OutputHn.Samples.Add((float)Hn[i]);
                OutputHn.SamplesIndices.Add(indexCount); // -ve -> +ve Symmetrical
                indexCount++;
            }
            DirectConvolution dc = new DirectConvolution();
            dc.InputSignal1 = InputTimeDomainSignal;
            dc.InputSignal2 = OutputHn;
            dc.Run();
            OutputYn = dc.OutputConvolvedSignal;
        }
        public static void writeSignalFile(Signal Output, int N)
        {
            //Text file is saved inside DSPComponentsUnitTes\bin\Debug folder if test is run from there

            string fileName = "FIR_Coefficients.txt";
            var signal_type = 0;
            FileStream file = new FileStream(fileName, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(file);
            sw.WriteLine(signal_type.ToString());
            sw.WriteLine(Convert.ToInt32(Output.Periodic).ToString());
            sw.WriteLine(N.ToString());
            for (int i = 0; i < N; i++)
                sw.WriteLine(Output.SamplesIndices[i].ToString() + " " + Output.Samples[i].ToString());
            sw.Close();
            file.Close();
        }
        public override void Run()
        {
            string window_fn = getWindowType();
            int filter_length = (int)getFilterLength(window_fn);
            int half_len = (filter_length - 1) / 2;
            List<double> hD = new List<double>();
            List<double> W = new List<double>();
            for (int i = 0; i <= half_len; i++)
            {
                if (InputFilterType == FILTER_TYPES.LOW)
                    hD.Add(getHdLow_low_filters(i, 1));
                if (InputFilterType == FILTER_TYPES.HIGH)
                    hD.Add(getHdLow_High_filters(i, -1));
                if (InputFilterType == FILTER_TYPES.BAND_PASS)
                    hD.Add(getHdBand_filters(i, 1));
                if (InputFilterType == FILTER_TYPES.BAND_STOP)
                    hD.Add(band_stop(i, -1));
                W.Add(getWindowFunction(i, window_fn, filter_length));
            }
            apply_filter(half_len, hD, W);
            writeSignalFile(OutputHn, filter_length);
        }
    }
}