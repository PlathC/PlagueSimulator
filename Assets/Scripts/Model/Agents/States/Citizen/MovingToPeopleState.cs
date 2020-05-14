using System.Linq;
using UnityEngine;

namespace Model.Agents.States.Citizen
{
    public class MovingToPeopleState : CitizenState
    {
        private CitizenBody m_bodyToFollow;
        private uint m_countWithoutSocial = 0;
        
        public MovingToPeopleState(Agents.Citizen citizen) : base(citizen)
        {
        }

        private void FindNewAgentToFollow()
        {
            var closestAgents = m_citizen.Body.GetClosestAgents();
            if (closestAgents != null && closestAgents.Any())
            {
                CitizenBody closestAgent = null;
                do
                {
                    int index = Random.Range(0, closestAgents.Count);
                    closestAgent = closestAgents[index];
                } while (!m_bodyToFollow || closestAgent != m_bodyToFollow);
               
                m_bodyToFollow = closestAgent;
            }
            else
            {
                m_countWithoutSocial++;
            }
        }

        private void FollowCurrentBody()
        {
            m_citizen.Body.MoveTo(m_bodyToFollow.transform.position);
            m_citizen.Body.CurrentPositionState = CitizenBody.PositionState.IsMoving;
        }
        
        public override IState Action()
        {
            var needToSeePeople = m_citizen.Body.SocialStress > m_citizen.Body.SocialThresh;
            if (needToSeePeople)
            {
                if (!m_bodyToFollow)
                {
                    FindNewAgentToFollow();
                }
                
                if(m_bodyToFollow)
                    FollowCurrentBody();

                if (m_countWithoutSocial > 5)
                    return new MovingToRandomState(m_citizen);
                
                return this;
            }
            return new Idle(m_citizen);
            
        }
    }
}