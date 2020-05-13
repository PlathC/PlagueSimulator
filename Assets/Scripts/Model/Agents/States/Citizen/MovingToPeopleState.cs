using System.Linq;
using UnityEngine;

namespace Model.Agents.States.Citizen
{
    public class MovingToPeopleState : CitizenState
    {
        private CitizenBody m_bodyToFollow;
        
        public MovingToPeopleState(Agents.Citizen citizen) : base(citizen)
        {
        }

        public void FindNewAgentToFollow()
        {
            var closestAgent = m_citizen.Body.GetClosestAgents();
            if (closestAgent != null && closestAgent.Any())
            {
                int index = Random.Range(0, closestAgent.Count);
                m_bodyToFollow = closestAgent[index];
            }
        }

        private void FollowCurrentBody()
        {
            m_citizen.Body.MoveTo(m_bodyToFollow.transform.position);
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
                FollowCurrentBody();

                return this;
            }
            return new Idle(m_citizen);
            
        }
    }
}