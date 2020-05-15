using UnityEngine;

namespace Model.Agents.States.Citizen
{
    public class GoToHomeState : CitizenState 
    {
        public GoToHomeState(Agents.Citizen citizen) : base(citizen)
        {
            citizen.Body.CurrentPositionState = CitizenBody.PositionState.ReturningHome;
        }

        public override IState Action()
        {
            if (Vector3.Distance(m_citizen.HomePosition, m_citizen.transform.position) <= 1f)
            {
                m_citizen.Body.CurrentPositionState = CitizenBody.PositionState.AtHome;
                return new Idle(m_citizen);
            }
            
            m_citizen.Body.MoveTo(m_citizen.HomePosition);
            
            return this;
        }
    }
}