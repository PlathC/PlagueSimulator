namespace Model.Agents.States.Citizen
{
    public class GoToHomeState : CitizenState 
    {
        public GoToHomeState(Agents.Citizen citizen) : base(citizen)
        {}

        public override IState Action()
        {
            throw new System.NotImplementedException();
        }
    }
}