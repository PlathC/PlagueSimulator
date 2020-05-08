using System;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Agents
{
    public class AgentDetection : MonoBehaviour
    {
        private List<CitizenBody> m_citizenList = new List<CitizenBody>();
        public List<CitizenBody> CitizenList => m_citizenList;
        
        private void OnTriggerEnter(Collider other)
        {
            if(other.GetType() == typeof(BoxCollider) && other.gameObject.GetComponent<CitizenBody>() != null)
                m_citizenList.Add(other.gameObject.GetComponent<CitizenBody>());
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.GetType() == typeof(BoxCollider) && other.gameObject.GetComponent<CitizenBody>() != null)
                m_citizenList.Remove(other.gameObject.GetComponent<CitizenBody>());
        }
    }
}
