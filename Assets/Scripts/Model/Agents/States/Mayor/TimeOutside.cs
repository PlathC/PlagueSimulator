using Model.Environment;

namespace Model.Agents.States.Mayor
{
    public class TimeOutside : MayorState
    {
        public TimeOutside(AgentEnvironment environment) : base(environment)
        {
        }
        
        public override IState Action()
        {
            throw new System.NotImplementedException();
        }
    }
}