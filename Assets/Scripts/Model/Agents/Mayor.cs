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
        protected override void Start()
        {
            base.Start();

            m_currentState = new Idle(m_environment, this);
        }

        private void Update()
        {
            if(m_currentState != null)
                m_currentState = m_currentState.Action();
        }

        public void DecreaseTimeOutside(float delta)
        {
            m_environment.MaximumTimeOutside -= delta;
        }

        public void IncreaseSocialDistancing(float delta)
        {
            m_environment.SocialDistancing += delta;
        }
    }
}