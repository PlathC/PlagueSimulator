using Model.Environment;
using UnityEngine;

namespace Model.Agents
{
    public class Citizen : Agent
    {
        private CitizenBody m_citizenBody;

        private Vector3 m_destination;
        private AgentEnvironment m_environment;


        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            m_citizenBody = gameObject.GetComponent<CitizenBody>();
            var env = GameObject.FindGameObjectWithTag("AgentEnvironment");
            if (!env) return;
                
            var agentEnvironment = env.GetComponent<AgentEnvironment>();
            if (agentEnvironment)
                m_environment = agentEnvironment;
        }

        // Update is called once per frame
        void Update()
        {
            if(m_citizenBody.SocialStress > m_citizenBody.SocialThresh)
            {
                if(m_citizenBody.PositionState != PositionStateEnum.IsMoving)
                {
                    float radiusX = 50f;
                    float radiusZ = 50f;
                    m_destination = m_citizenBody.HomePosition;

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

                    m_citizenBody.MoveTo(m_destination);
                }
                else
                {
                    m_citizenBody.MoveTo(m_destination);
                }
            
            }
            else
            {
                m_citizenBody.ReturnHome();
            }
            
        }
    }
}
