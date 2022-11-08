using System;
using NationalInstruments.DAQmx;
using System.Diagnostics;

namespace NationalInstruments.Examples
{
    public enum WaveformType {
        SineWave     = 0
    }

	public class FunctionGenerator
	{
        public FunctionGenerator(
            Timing timingSubobject,
            string desiredFrequency,
            string duration,
            string samplesPerCycle,
            string amplitude)
        {
            WaveformType t = new WaveformType();
            t = WaveformType.SineWave;

            Init(
                timingSubobject,
                Double.Parse(desiredFrequency),
                Double.Parse(duration),
                int.Parse(samplesPerCycle),
                t,
                Double.Parse(amplitude));
        }

        public FunctionGenerator(
            Timing timingSubobject,
            double desiredFrequency,
            double duration,
            int samplesPerCycle,
            WaveformType type,
            double amplitude)
        {
            Init(
                timingSubobject,
                desiredFrequency,
                duration,
                samplesPerCycle,
                type,
                amplitude);
        }

        private void Init(
		    Timing timingSubobject,
            double desiredFrequency,
            double duration,
            int samplesPerCycle,
            WaveformType type,
            double amplitude)
		{
            if(desiredFrequency <= 0)
                throw new ArgumentOutOfRangeException("desiredFrequency",desiredFrequency,"This parameter must be a positive number");
            if(samplesPerCycle <= 0)
                throw new ArgumentOutOfRangeException("samplesPerCycle",samplesPerCycle,"This parameter must be a positive number");

            // First configure the Task timing parameters
            if(timingSubobject.SampleTimingType == SampleTimingType.OnDemand)
                timingSubobject.SampleTimingType = SampleTimingType.SampleClock;

            // clock rate is sample rate per cycle * frequency (which is number of cycles)
            _sampleClockRate = (desiredFrequency * samplesPerCycle);

            // Determine the actual sample clock rate
            timingSubobject.SampleClockRate = _sampleClockRate;
            _resultingSampleClockRate = timingSubobject.SampleClockRate;

            switch(type)
            {
                case WaveformType.SineWave:
                    _data = GenerateSineWave(desiredFrequency,amplitude, _resultingSampleClockRate, samplesPerCycle, duration);
                    break;
                default:
                    // Invalid type value
                    Debug.Assert(false);
                    break;
            }
        }

        public double[] Data
        {
            get
            {
                return _data;
            }
        }

        public double ResultingSampleClockRate
        {
            get
            {
                return _resultingSampleClockRate;
            }
        }

        public static double[] GenerateSineWave(
            double frequency, 
            double amplitude,
            double sampleClockRate,
            int samplesPerCycle,
            double duration)
        {
            int totalSamples = Convert.ToInt32(frequency * samplesPerCycle * duration);
            double[] rVal = new double[totalSamples];

            for(int i=0; i<totalSamples; i++)
                rVal[i] = amplitude * Math.Sin( (2.0 * Math.PI) * frequency * i / sampleClockRate );

            return rVal;
        }

        private double[] _data;
        private double _resultingSampleClockRate;
        private double _sampleClockRate;
    }
}
