using UnityEngine;
using System.Collections;

public class Citizen : Agent
{
    private CitizenBody m_citizenBody;

    private Vector3 destination;


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
            if(m_citizenBody.PositionState != PositionState.IsMoving)
            {
                float radius = 50f;
                destination = m_citizenBody.HomePosition;
                destination.x += Random.Range(-radius, radius);
                destination.z += Random.Range(-radius, radius);
                m_citizenBody.MoveTo(destination);
            }
            else
            {
                m_citizenBody.MoveTo(destination);
            }
            
        }
        else
        {
            m_citizenBody.ReturnHome();
        }
            
    }
}
