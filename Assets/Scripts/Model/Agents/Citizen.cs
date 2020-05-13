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
            var needToSeePeople = m_citizenBody.SocialStress > m_citizenBody.SocialThresh;
            var needToGoOutside =  m_citizenBody.OutStress > m_citizenBody.OutStressThresh;
            if((needToSeePeople || needToGoOutside) && canMove)
            {

                if (m_citizenBody.PositionState != CitizenBody.PositionStateEnum.IsMoving)
                {
                    if (needToSeePeople)
                    {
                        var closestAgent = m_citizenBody.GetClosestAgents();
                        if (closestAgent != null && closestAgent.Any())
                        {
                            int index = Random.Range(0, closestAgent.Count);
                            m_destination = closestAgent[index].transform.position;
                        }
                    }
                    else if(needToGoOutside)
                    {
                        float radiusX = 50f;
                        float radiusZ = 50f;
                        m_destination = m_homePosition;
                        float widthSteps = (float) m_environment.Coordinates.width / 2;
                        float heightSteps = (float) m_environment.Coordinates.height / 2;
                        if (m_destination.x - radiusX < -widthSteps)
                        {
                            radiusX -= -widthSteps - (m_destination.x - radiusX);
                        }
                    
                        if (m_destination.z - radiusZ < -heightSteps)
                        {
                            radiusZ -= -heightSteps - (m_destination.z - radiusZ);
                        }
                        if (m_destination.x + radiusX > widthSteps)
                        {
                            radiusX -= m_destination.x + radiusX - widthSteps;
                        }
                    
                        if (m_destination.z + radiusZ > heightSteps)
                        {
                            radiusZ -= m_destination.z + radiusZ - heightSteps;
                        }
                    
                        m_destination.x += Random.Range(-radiusX, radiusX);
                        m_destination.z += Random.Range(-radiusZ, radiusZ);
                    }
                }
                
                if (Vector3.Distance(m_citizenBody.transform.position, m_destination) < m_positionCloseThresh)
                    m_citizenBody.PositionState = CitizenBody.PositionStateEnum.NotMoving;
                else
                    m_citizenBody.PositionState = CitizenBody.PositionStateEnum.IsMoving;
                
                m_citizenBody.MoveTo(m_destination);
            }
            else if(m_citizenBody.PositionState != CitizenBody.PositionStateEnum.AtHome)
            {
                m_citizenBody.PositionState = CitizenBody.PositionStateEnum.ReturningHome;
                m_citizenBody.MoveTo(m_homePosition);
            }
        }
    }
}
