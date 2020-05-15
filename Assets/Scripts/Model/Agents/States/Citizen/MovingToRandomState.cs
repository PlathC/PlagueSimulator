using UnityEngine;

namespace Model.Agents.States.Citizen
{
    public class MovingToRandomState : CitizenState
    {
        private bool m_computeDestination = true;
        private Vector3 m_destination = Vector3.zero;
           
        public MovingToRandomState(Agents.Citizen citizen) : base(citizen)
        {
        }

        private void ComputeNewDirection()
        {
            float radiusX = 50f;
            float radiusZ = 50f;
            m_destination = m_citizen.HomePosition;
            float widthSteps = (float) m_citizen.AssociatedEnvironment.Coordinates.width / 2;
            float heightSteps = (float) m_citizen.AssociatedEnvironment.Coordinates.height / 2;
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
            m_citizen.Body.CurrentPositionState = CitizenBody.PositionState.IsMoving;
        }

        public override IState Action()
        {
            var needToGoOutside =  m_citizen.Body.OutStress > m_citizen.Body.OutStressThresh;

            if (!needToGoOutside) 
                return new Idle(m_citizen);
            
            if(m_destination == Vector3.zero)
                ComputeNewDirection();
                
            m_citizen.Body.MoveTo(m_destination);
            
            if(Vector3.Distance(m_destination, m_citizen.Body.transform.position) < m_citizen.PositionCloseThresh)
                return new Idle(m_citizen);
            
            return this;
        }
    }
}