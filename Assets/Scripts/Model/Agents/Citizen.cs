using System;
using System.Linq;
using Model.Agents.States;
using Model.Agents.States.Citizen;
using Model.Environment;
using UnityEngine;

namespace Model.Agents
{
    public class Citizen : Agent
    {
        private CitizenBody m_citizenBody;
        public CitizenBody Body => m_citizenBody;
        
        private AgentEnvironment m_environment;
        public AgentEnvironment AssociatedEnvironment => m_environment;
        
        private Vector3 m_homePosition;
        public Vector3 HomePosition => m_homePosition;

        public float TimeOutside { get; set; }
        private bool isTimerLaunched = false;

        public float PositionCloseThresh { get; } = 1f;

        private IState m_currentState;

        protected override void Start()
        {
            base.Start();
            
            m_homePosition = gameObject.transform.position;
            m_citizenBody = gameObject.GetComponent<CitizenBody>();
                
            var env = GameObject.FindGameObjectWithTag("AgentEnvironment");
            if (!env) return;
                
            var agentEnvironment = env.GetComponent<AgentEnvironment>();
            if (agentEnvironment)
                m_environment = agentEnvironment;
            
            m_currentState = new Idle(this);

            TimeOutside = 0f;
        }

        public void StartOrContinueTimer()
        {
            if (isTimerLaunched) return;
            
            InvokeRepeating(nameof(IncreaseTimer), 0f, 0.5f);
            isTimerLaunched = true;
        }

        private void IncreaseTimer()
        {
            TimeOutside += 0.5f;
        }

        public void ResetAndStopTimer()
        {
            CancelInvoke();
            TimeOutside = 0f;
            isTimerLaunched = false;
        }

        void Update()
        {
            m_currentState = m_currentState.Action();
        }
    }
}
