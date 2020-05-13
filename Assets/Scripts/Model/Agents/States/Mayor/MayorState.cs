using Model.Environment;

namespace Model.Agents.States.Mayor
{
    public abstract class MayorState : IState
    {
        protected AgentEnvironment m_environment;
        protected Agents.Mayor m_mayor;

        protected MayorState(AgentEnvironment environment, Agents.Mayor mayor)
        {
            m_environment = environment;
            m_mayor = mayor;
        }

        public abstract IState Action();
    }
}