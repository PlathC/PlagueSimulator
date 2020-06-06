using Model.Agents.States;
using Model.Environment;
using System;
using UnityEngine;

namespace Model.Agents
{
    public abstract class Agent : MonoBehaviour
    {
        protected AgentEnvironment m_environment;
        public AgentEnvironment AssociatedEnvironment => m_environment;

        private float m_startTime;
        public float StartTime { get => m_startTime; }
    
        private Guid m_uniqueIdentifier;
        public Guid GetUniqueIdentifier { get => m_uniqueIdentifier; }

        protected IState m_currentState;

        protected virtual void Start()
        {
            m_uniqueIdentifier = Guid.NewGuid();
            m_startTime = Time.time;

            var env = GameObject.FindGameObjectWithTag("AgentEnvironment");
            if (!env) return;

            var agentEnvironment = env.GetComponent<AgentEnvironment>();
            if (agentEnvironment)
                m_environment = agentEnvironment;
        }
    }
}
