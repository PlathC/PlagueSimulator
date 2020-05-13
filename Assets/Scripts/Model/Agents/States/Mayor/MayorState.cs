using Model.Environment;

namespace Model.Agents.States.Mayor
{
    public abstract class MayorState : IState
    {
        protected AgentEnvironment m_environment;

        protected MayorState(AgentEnvironment environment)
        {
            m_environment = environment;
        }

        public abstract void action();
        public abstract IState next();
    }
}