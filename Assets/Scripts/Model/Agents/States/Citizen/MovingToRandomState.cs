using UnityEngine;

namespace Model.Agents.States.Citizen
{
    public class MovingToRandomState : CitizenState
    {
        private bool m_computeDestination = true;
        private Vector3 m_destination = Vector3.zero;
           
        public MovingToRandomState(Agents.Citizen citizen) : base(citizen)
        {
            m_citizen.Body.CurrentPositionState = CitizenBody.PositionState.IsMoving;
            m_citizen.StartOrContinueTimer();
        }

        private void ComputeNewDirection()
        {
            float radiusX = 10f;
            float radiusZ = 10f;
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
        }

        public override IState Action()
        {
            var needToGoOutside =  m_citizen.Body.OutStress >= 0.1;

            //if (!needToGoOutside) 
            //    return new Idle(m_citizen);

            if(m_destination == Vector3.zero)
                ComputeNewDirection();
                
            m_citizen.Body.MoveTo(m_destination);
            
            if (Vector3.Distance(m_destination, m_citizen.transform.position) < m_citizen.PositionCloseThresh)
            {
                var needToSeePeople = m_citizen.Body.SocialStress > m_citizen.Body.SocialThresh;

                if(needToSeePeople)
                    return new MovingToPeopleState(m_citizen);
                if(!needToGoOutside)
                    return new Idle(m_citizen);
                m_destination = Vector3.zero;
            }
            
            return this;
        }
    }
}