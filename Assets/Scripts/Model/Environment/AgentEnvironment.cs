using System.Collections.Generic;
using Model.Agents;
using Model.Data;
using UnityEngine;

namespace Model.Environment
{
    public class AgentEnvironment : MonoBehaviour
    {
        private readonly List<CitizenBody> m_citizenList = new List<CitizenBody>();
        
        private int m_sickNumber = 0;
    
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
        
        void Start()
        {
            simulationData = ScriptableObject.FindObjectOfType<SimulationData>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void NotifyAgentModification(StorageData old)
        {
            if (old.State == CitizenBody.SicknessState.Infected)
                m_sickNumber++;
        }

        public bool GetVirusContagiousity()
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
