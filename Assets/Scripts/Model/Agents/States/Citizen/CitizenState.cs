namespace Model.Agents.States.Citizen
{
    public abstract class CitizenState : IState
    {
        protected Agents.Citizen m_citizen;
        
        protected CitizenState(Agents.Citizen citizen)
        {
            m_citizen = citizen;
        }
    
        public abstract IState Action();
    }
}