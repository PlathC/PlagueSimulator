using Model.Environment;
using UnityEngine;

namespace Model.Agents.States.Mayor
{
    public class TimeOutside : MayorState
    {
        private float m_growthRate;
        public TimeOutside(AgentEnvironment environment, Agents.Mayor mayor, float growthRate) : base(environment, mayor)
        {
            m_growthRate = growthRate;
        }
        
        public override IState Action()
        {
            if(m_growthRate > 0)
                m_mayor.DecreaseTimeOutside(.05f);
            else
                m_mayor.DecreaseTimeOutside(-0.05f);
            return new Idle(m_environment, m_mayor);
        }
    }
}