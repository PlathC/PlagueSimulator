using Model.Environment;

namespace Model.Agents.States.Mayor
{
    public class SocialDistancing : MayorState
    {
        private float m_growthRate;
        public SocialDistancing(AgentEnvironment environment, Agents.Mayor mayor, float growthRate) : base(environment, mayor)
        {
            m_growthRate = growthRate;
        }

        public override IState Action()
        {
            if(m_growthRate > 0)
                m_mayor.IncreaseSocialDistancing(.05f);
            else
                m_mayor.IncreaseSocialDistancing(-.05f);
            
            return new Idle(m_environment, m_mayor);
        }
    }
}