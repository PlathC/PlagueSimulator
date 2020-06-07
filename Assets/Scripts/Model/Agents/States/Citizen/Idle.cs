namespace Model.Agents.States.Citizen
{
    public class Idle : CitizenState
    {
        public Idle(Agents.Citizen citizen) : base(citizen)
        {
            citizen.Body.CurrentPositionState = CitizenBody.PositionState.NotMoving;
        }
        
        public override IState Action()
        {
            var needToSeePeople = m_citizen.Body.SocialStress > m_citizen.Body.SocialThresh;
            var needToGoOutside =  m_citizen.Body.OutStress > m_citizen.Body.OutStressThresh;
            
            if(m_citizen.TimeOutside > m_citizen.AssociatedEnvironment.MaximumTimeOutside && 
                m_citizen.Body.CurrentPositionState != CitizenBody.PositionState.AtHome)
                return new GoToHomeState(m_citizen);
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