﻿using System.Linq;
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
            //TODO return always null sometimes and create an infinite loop MovingToPeople -> MovingToRandom -> Idle -> MovingToPeople
            
            var closestAgents = m_citizen.Body.GetClosestAgents();
            if (closestAgents != null && closestAgents.Any())
            {
                CitizenBody closestAgent = null;
                uint count = 0;
                bool exit = false;
                do
                {
                    int index = Random.Range(0, closestAgents.Count);
                    closestAgent = closestAgents[index];
                    count++;
                    
                    if (!m_bodyToFollow)
                        exit = true;
                    else
                    {
                        if (closestAgent != m_bodyToFollow || closestAgents.Count == 1)
                            exit = true;
                        else if (count >= 10)
                            exit = true;
                    }
                    
                } while (!exit);
                
                if(count < 10)
                    m_bodyToFollow = closestAgent;
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

                if (m_bodyToFollow)
                {
                    if(Vector3.Distance(m_bodyToFollow.transform.position, m_citizen.transform.position) < 1f)
                        return new Idle(m_citizen);
                    
                    FollowCurrentBody();
                }

                return this;
            }

            return new Idle(m_citizen);
        }
    }
}