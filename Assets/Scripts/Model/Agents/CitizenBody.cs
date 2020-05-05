using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenBody : AgentBody
{
    private State m_state = State.Healthy;

    public State State
    {
        get { return m_state; }
        set
        {
            m_state = value;

            Color color = new Color();
            switch (m_state)
            {
                case State.Healthy:
                    color = Color.green;
                    break;
                case State.Infected:
                    color = Color.red;
                    break;
                case State.Immuned:
                    color = Color.gray;
                    break;
                case State.Dead:
                    color = Color.black;
                    break;
            }
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", color);
        }
    }

    [SerializeField]
    private float m_speed = 5f;

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

    void MoveTo(Vector3 position)
    {
        transform.position = Vector3.MoveTowards(transform.position, position, m_speed * Time.deltaTime);
    }

    void ReturnHome()
    {
        transform.position = Vector3.MoveTowards(transform.position, m_homePosition, m_speed * Time.deltaTime);
    }
}

public enum State
{
    Healthy,
    Infected,
    Immuned,
    Dead
}