using Model.Environment;

namespace Model.Agents.States.Mayor
{
    public class SocialDistancing : MayorState
    {
        public SocialDistancing(AgentEnvironment environment, Agents.Mayor mayor) : base(environment, mayor)
        {
        }

        public override IState Action()
        {
            m_mayor.IncreaseSocialDistancing(.05f);
            return new Idle(m_environment, m_mayor);
        }
    }
}