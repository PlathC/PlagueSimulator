using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenBody : MonoBehaviour
{
    private State m_state = State.Healthy;

    private float m_socialStress = .0f;
    private float m_socialStressThresh = 100f;

    private float m_outStress = .0f;
    private float m_outStressThresh = 100f;

    private Vector3 m_homePosition;


    // Start is called before the first frame update
    void Start()
    {
        m_homePosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 0, 0), 20f*Time.deltaTime);
    }

}

enum State
{
    Healthy,
    Infected,
    Immuned,
    Dead
}