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
            if (other.GetType() != typeof(BoxCollider)) return;
            
            var otherBody = other.gameObject.GetComponentInParent<CitizenBody>();
            if (!otherBody) return;
                
            m_citizenList.Add(otherBody);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetType() != typeof(BoxCollider)) return;
            
            var otherBody = other.gameObject.GetComponentInParent<CitizenBody>();
            if (!otherBody) return;
                
            m_citizenList.Remove(otherBody);
        }
    }
}
