using Model.Environment;

namespace Model.Agents.States.Mayor
{
    public class TimeOutside : MayorState
    {
        public TimeOutside(AgentEnvironment environment) : base(environment)
        {
        }
        
        public override IState action()
        {
            throw new System.NotImplementedException();
        }
    }
}