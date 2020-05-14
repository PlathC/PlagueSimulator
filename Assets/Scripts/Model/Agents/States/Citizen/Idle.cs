namespace Model.Agents.States.Citizen
{
    public class Idle : CitizenState
    {
        public Idle(Agents.Citizen citizen) : base(citizen)
        {
        }
        
        public override IState Action()
        {
            var needToSeePeople = m_citizen.Body.SocialStress > m_citizen.Body.SocialThresh;
            var needToGoOutside =  m_citizen.Body.OutStress > m_citizen.Body.OutStressThresh;

            if(needToSeePeople)
                return new MovingToPeopleState(m_citizen);
            if(needToGoOutside)
                return new MovingToRandomState(m_citizen);
            
            if(m_citizen.Body.CurrentPositionState != CitizenBody.PositionState.AtHome)
                return new GoToHomeState(m_citizen);
            return this;
        }
    }
}