using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Model.Agents;
using Model.Agents.States.Mayor;
using Model.Data;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Model.Environment
{
    public class AgentEnvironment : MonoBehaviour
    {
        private System.Random m_systemRandom = new System.Random();
        
        private readonly List<CitizenBody> m_citizenList = new List<CitizenBody>();
        
        private int m_sickNumber = 0;
        public int SickNumber => m_sickNumber;

        private int m_immunedNb = 0;
        public int ImmunedNumber => m_immunedNb;

        private int m_deadNb = 0;
        public int DeadNumber => m_deadNb;

        private List<int> m_growthRate = new List<int>(){0};
        private int m_lastSickNumber = 0;

        public int LastGrowthRate => m_growthRate.Last();

        private float m_maximumTimeOutside = 30f;

        public float MaximumTimeOutside
        {
            get => m_maximumTimeOutside; 
            set => m_maximumTimeOutside = value > 30f ? 30f : m_maximumTimeOutside;
        }
        private float m_socialDistancing = 1f;
        public float SocialDistancing
        {
            get => m_socialDistancing;
            set => m_socialDistancing = value < 1f ? 1f : m_socialDistancing;
        }

        public List<CitizenBody> CitizenList => m_citizenList;

        public struct MapCoordinates
        {
            public Vector3 startPoint;
            public uint height;
            public uint width;

            public MapCoordinates(Vector3 startPoint, uint width, uint height)
            {
                this.startPoint = startPoint;
                this.width = width;
                this.height = height;
            }
        }

        private MapCoordinates m_coordinates;
        public MapCoordinates Coordinates { get => m_coordinates; set => m_coordinates = value; }
        
        private SimulationData simulationData = null;

        private List<StorageData> m_save = new List<StorageData>();
        
        void Start()
        {
            simulationData = ScriptableObject.FindObjectOfType<SimulationData>();
            InvokeRepeating(nameof(UpdateGrowthRate), 2f, 2f);
        }

        private void UpdateGrowthRate()
        {
            m_growthRate.Add(m_sickNumber - m_lastSickNumber);
            m_lastSickNumber = m_sickNumber;
        }
        
        void OnApplicationQuit()
        {
            Debug.Log("Application ending after " + Time.time + " seconds");
            string folder = "./SimulationExport";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            
            string csvSick = "Time,PositionState,SickingState,x,y,z,CauseOfDeath\n";
            csvSick += String.Join("\n", m_save.Select(x => x.ToString()).ToArray());

            Debug.Log("Saving data to " + folder);
            string destination = folder + "/data.csv";
            var file = File.Create(destination);
            var sw = new StreamWriter(file);
            sw.Write(csvSick);
            file.Close();
        }

        public void NotifyAgentModification(StorageData old)
        {
            m_save.Add(old);
            
            if (old.sicknessState == CitizenBody.SicknessState.Infected)
                m_sickNumber++;
            else if (old.sicknessState == CitizenBody.SicknessState.Immuned)
                m_immunedNb++;
            else if (old.sicknessState == CitizenBody.SicknessState.Dead)
                m_deadNb++;
            
            if (old.oldSicknessState == CitizenBody.SicknessState.Infected)
                m_sickNumber--;
            else if (old.oldSicknessState == CitizenBody.SicknessState.Immuned)
                m_immunedNb--;
        }

        public bool GetVirusContagiosity(float distance)
        {
            return Random.Range(0f, 1f) < 1/Math.Exp(distance);
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
            return (float) SampleGaussian(m_systemRandom, simulationData.diseaseDuration, simulationData.diseaseDuration/10f);;
        }
        
        public bool ImmunedOrDead()
        {
            return Random.Range(0f, 1f) > simulationData.deathStatistic;
        }
        
        public void UpdateAgentList()
        {
            m_citizenList.Clear();

            var agents = GameObject.FindGameObjectsWithTag("Player");
            uint immunedNb = (uint) Math.Floor(simulationData.populationDensity * simulationData.launchImmunedNumber);
            uint sickNb = (uint) Math.Floor(simulationData.populationDensity * simulationData.launchSickNumber);

            foreach(var agent in agents)
            {
                var body = agent.GetComponent<CitizenBody>();
                m_citizenList.Add(body);
                   
                if (immunedNb > 0)
                {
                    body.CurrentSickness = CitizenBody.SicknessState.Immuned;
                    immunedNb--;
                }
                else if(sickNb > 0)
                {
                    body.CurrentSickness = CitizenBody.SicknessState.Infected;
                    sickNb--;
                }
                else
                {
                    body.CurrentSickness = CitizenBody.SicknessState.Healthy;
                }
            }
        }

        public bool IsOnMapRelative(Vector3 v)
        {
            float widthSteps = (float)m_coordinates.width / 2;
            float heightSteps = (float)m_coordinates.height / 2;
                
            return v.x > -widthSteps 
                   && v.z > -heightSteps
                   && v.x < widthSteps 
                   && v.z < heightSteps;
        }
    }
}
