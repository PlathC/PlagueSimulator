using UnityEngine;
using System.Collections;

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
    }

    // Update is called once per frame
    void Update()
    {
        if(m_citizenBody.SocialStress > m_citizenBody.SocialThresh)
        {
            if(m_citizenBody.PositionState != PositionStateEnum.IsMoving)
            {
                float radius = 50f;
                m_destination = m_citizenBody.HomePosition;
                m_destination.x += Random.Range(-radius, radius);
                m_destination.z += Random.Range(-radius, radius);
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
