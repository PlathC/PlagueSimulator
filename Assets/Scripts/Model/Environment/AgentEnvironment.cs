using System.Collections.Generic;
using System.Linq;
using Model.Agents;
using Model.Data;
using UnityEngine;

namespace Model.Environment
{
    public class AgentEnvironment : MonoBehaviour
    {
        private readonly List<CitizenBody> m_citizenList = new List<CitizenBody>();
        
        private int m_sickNumber = 0;
        public int SickNumber
        {
            get => m_sickNumber;
            set => m_sickNumber = value;
        }
        
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

        public void NotifyAgentModification(StorageData old)
        {
            m_save.Add(old);
            if (old.sicknessState == CitizenBody.SicknessState.Infected)
                m_sickNumber++;
        }

        public bool GetVirusContagiosity()
        {
            return Random.Range(0f,1f) > simulationData.infectivity;
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
