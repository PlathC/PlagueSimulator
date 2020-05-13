using Model.Environment;

namespace Model.Agents.States.Mayor
{
    public class Idle : MayorState
    {
        public Idle(AgentEnvironment environment) : base(environment)
        {
        }

        public override void action()
        {
        }

        public override IState next()
        {
            //TODO 
            throw new System.NotImplementedException();
        }
    }
}