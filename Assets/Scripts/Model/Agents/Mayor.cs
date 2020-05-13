using System;
using System.Linq;
using Model.Agents.States;
using Model.Agents.States.Mayor;
using Model.Environment;
using UnityEngine;

namespace Model.Agents
{
    public class Mayor : Agent
    {
        [SerializeField]
        private AgentEnvironment environment;

        private IState m_currentState;

        protected override void Start()
        {
            base.Start();
            m_currentState = new Idle(environment);
        }

        private void Update()
        {
            m_currentState.action();
            m_currentState = m_currentState.next();
        }
    }
}