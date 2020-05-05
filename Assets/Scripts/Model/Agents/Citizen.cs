using UnityEngine;
using System.Collections;

public class Citizen : Agent
{
    private CitizenBody m_citizenBody;

    // Use this for initialization
    void Start()
    {
        m_citizenBody = gameObject.GetComponent<CitizenBody>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
