using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Model.Agents;
using Model.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Model.Environment
{
    public class AgentEnvironment : MonoBehaviour
    {
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

        public float MaximumTimeOutside { get; set; } = 500f;
        public float SocialDistancing { get; set; } = 10f;

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

        // Update is called once per frame
        void Update()
        {
        }
        
        void OnApplicationQuit()
        {
            Debug.Log("Application ending after " + Time.time + " seconds");
            
            var sickList = m_save.Where(item => item.sicknessState == CitizenBody.SicknessState.Infected).ToList();
            var immunedList = m_save.Where(item => item.sicknessState == CitizenBody.SicknessState.Immuned).ToList();
            var deadList = m_save.Where(item => item.sicknessState == CitizenBody.SicknessState.Dead).ToList();
            
            string csvSick = String.Join(",", sickList.Select(x => x.ToString() + "\n").ToArray());
            string csvImmuned = String.Join(",", immunedList.Select(x => x.ToString()  + "\n").ToArray());
            string csvDead = String.Join(",", deadList.Select(x => x.ToString() + "\n").ToArray());

            Debug.Log("Saving data to " + Application.persistentDataPath);
            string destination = Application.persistentDataPath + "/sickData.csv";
            var file = File.Create(destination);
            var sw = new StreamWriter(file);
            sw.Write(csvSick);
            file.Close();
            
            destination = Application.persistentDataPath + "/immunedData.csv";
            file = File.Create(destination);
            sw = new StreamWriter(file);
            sw.Write(csvImmuned);
            file.Close();
            
            destination = Application.persistentDataPath + "/deadData.csv";
            file = File.Create(destination);
            sw = new StreamWriter(file);
            sw.Write(csvDead);
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
        }

        public bool GetVirusContagiosity()
        {
            return Random.Range(0f, 1f) < simulationData.infectivity;
        }

        public float GetDiseaseDuration()
        {
            return 20f;
        }
        
        public bool ImmunedOrDead()
        {
            return Random.Range(0f, 1f) > 0.5f;
        }
        
        public void UpdateAgentList()
        {
            m_citizenList.Clear();

            var agents = GameObject.FindGameObjectsWithTag("Player");

            foreach(var agent in agents)
            {
                m_citizenList.Add(agent.GetComponent<CitizenBody>());
                if (agent.GetComponent<CitizenBody>().CurrentSickness == CitizenBody.SicknessState.Infected)
                    m_sickNumber++;
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
