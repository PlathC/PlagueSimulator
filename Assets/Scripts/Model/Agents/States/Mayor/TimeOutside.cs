using Model.Environment;

namespace Model.Agents.States.Mayor
{
    public class TimeOutside : MayorState
    {
        public TimeOutside(AgentEnvironment environment, Agents.Mayor mayor) : base(environment, mayor)
        {
        }
        
        public override IState Action()
        {
            m_mayor.DecreaseTimeOutside(.05f);
            return new Idle(m_environment, m_mayor);
        }
    }
}