using System.Collections.Generic;
using Model.Agents;
using Model.Data;
using UnityEngine;

namespace Model.Environment
{
    public class AgentEnvironment : MonoBehaviour
    {
        private List<CitizenBody> m_citizenList = new List<CitizenBody>();

        private int m_sickNumber = 0;
    
        public List<CitizenBody> CitizenList => m_citizenList;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void NotifyAgentModification(StorageData old)
        {
            if (old.State == SicknessState.Infected)
                m_sickNumber++;

            Debug.Log(m_sickNumber);
        }

        public bool GetVirusContagiousity()
        {
            return true;
        }

        public void UpdateAgentList()
        {
            m_citizenList.Clear();

            var agents = GameObject.FindGameObjectsWithTag("Player");

            foreach(var agent in agents)
            {
                m_citizenList.Add(agent.GetComponent<CitizenBody>());
                if (agent.GetComponent<CitizenBody>().State == SicknessState.Infected)
                    m_sickNumber++;
            }
        }
    }
}
