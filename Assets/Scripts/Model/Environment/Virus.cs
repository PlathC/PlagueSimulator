using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Model.Environment
{
    public class Virus 
    {
        private System.Random m_systemRandom = new System.Random();
        private uint m_diseaseDuration;
        private float m_deathStatistic;

        public Virus(uint diseaseDuration, float deathStatistic)
        {
            m_diseaseDuration = diseaseDuration;
            m_deathStatistic = deathStatistic;
        }


        public bool GetVirusContagiosity(float distance)
        {
            return Random.Range(0f, 1f) < 1 / Math.Exp(distance);
        }


        //https://gist.github.com/tansey/1444070
        private static double SampleGaussian(System.Random random, double mean, double stddev)
        {
            // The method requires sampling from a uniform random of (0,1]
            // but Random.NextDouble() returns a sample of [0,1).
            double x1 = 1 - random.NextDouble();
            double x2 = 1 - random.NextDouble();

            double y1 = Math.Sqrt(-2.0 * Math.Log(x1)) * Math.Cos(2.0 * Math.PI * x2);
            return y1 * stddev + mean;
        }

        public float GetDiseaseDuration()
        {
            return (float)SampleGaussian(m_systemRandom,
                m_diseaseDuration,
                m_diseaseDuration / 10f); ;
        }

        public bool ImmunedOrDead()
        {
            return Random.Range(0f, 1f) > m_deathStatistic;
        }
    }
}
