using System.Linq;
using Model.Environment;
using UnityEngine;

namespace Model.Agents
{
    public class Citizen : Agent
    {
        private CitizenBody m_citizenBody;
        private Vector3 m_destination;
        private AgentEnvironment m_environment;
        private Vector3 m_homePosition;
        private float m_positionCloseThresh = 0.5f;
        
        protected override void Start()
        {
            base.Start();
            
            m_homePosition = gameObject.transform.position;
            m_citizenBody = gameObject.GetComponent<CitizenBody>();
            m_destination = transform.position;
                
            var env = GameObject.FindGameObjectWithTag("AgentEnvironment");
            if (!env) return;
                
            var agentEnvironment = env.GetComponent<AgentEnvironment>();
            if (agentEnvironment)
                m_environment = agentEnvironment;
        }

        void Update()
        {
            var canMove = (m_citizenBody.PositionState != CitizenBody.PositionStateEnum.ReturningHome
                           || m_citizenBody.PositionState != CitizenBody.PositionStateEnum.AtHome);
            
            if(m_citizenBody.SocialStress >= m_citizenBody.SocialThresh && canMove)
            {
                if(m_citizenBody.PositionState != CitizenBody.PositionStateEnum.IsMoving)
                {
                    var closestAgent = m_citizenBody.GetClosestAgents();
                    if (closestAgent != null && closestAgent.Any())
                    {
                        int index = Random.Range(0, closestAgent.Count);
                        m_destination = closestAgent[index].transform.position;
                        m_citizenBody.MoveTo(m_destination);
                    }
                }
                else
                {
                    m_citizenBody.MoveTo(m_destination);
                }
                
                if (Vector3.Distance(m_citizenBody.transform.position, m_destination) < m_positionCloseThresh)
                    m_citizenBody.PositionState = CitizenBody.PositionStateEnum.NotMoving;
                else
                    m_citizenBody.PositionState = CitizenBody.PositionStateEnum.IsMoving;
            
            }
            else if(m_citizenBody.PositionState != CitizenBody.PositionStateEnum.AtHome)
            {
                m_citizenBody.PositionState = CitizenBody.PositionStateEnum.ReturningHome;
                m_citizenBody.MoveTo(m_homePosition);
            }
        }
    }
}
