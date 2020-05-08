using System;
using System.Collections;
using System.Collections.Generic;
using Model.Data;
using Model.Environment;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Model.Agents
{
    public class CitizenBody : AgentBody
    {
        private SicknessState m_state;
        private PositionStateEnum m_positionStateEnum = PositionStateEnum.AtHome;

        public SicknessState State
        {
            get => m_state;
            set
            {
                m_state = value;

                Color color = new Color();
                switch (m_state)
                {
                    case SicknessState.Healthy:
                        color = Color.green;
                        break;
                    case SicknessState.Infected:
                        color = Color.red;
                        break;
                    case SicknessState.Immuned:
                        color = Color.gray;
                        break;
                    case SicknessState.Dead:
                        color = Color.black;
                        break;
                }
                var renderer = gameObject.GetComponent<Renderer>();
                if(renderer)
                    renderer.material.SetColor("_Color", color);

                var env = GameObject.FindGameObjectWithTag("AgentEnvironment");
                if(env)
                {
                    var agentEnvironment = env.GetComponent<AgentEnvironment>();
                    if (agentEnvironment)
                        agentEnvironment.NotifyAgentModification(new StorageData(m_state, transform.position));
                }
            }
        }

        public PositionStateEnum PositionState => m_positionStateEnum;

        [SerializeField]
        private float m_speed = 5f;

        private float m_socialStress = .0f;
        private float m_socialGrowthRate;
        private float m_socialStressThresh;

        public float SocialStress => m_socialStress;

        public float SocialThresh => m_socialStressThresh;

        public Vector3 HomePosition => m_homePosition;

        private float m_outStress = .0f;
        private float m_outStressThresh = 100f;

        private float m_positionCloseThresh = 0.5f;

        private Vector3 m_homePosition;

        // Start is called before the first frame update
        void Start()
        {
            m_homePosition = gameObject.transform.position;
            if (Random.Range(0, 10) > 8)
                State = SicknessState.Infected;
            else
                State = SicknessState.Healthy;

            m_socialGrowthRate = Random.Range(.05f, .3f);
            m_socialStressThresh = Random.Range(10f, 100f);
        }

        void Update()
        {
            if(PositionState != PositionStateEnum.IsMoving)
            {
                m_socialStress += m_socialGrowthRate;
                m_outStress += 0.1f;
            }
        }

        public void MoveTo(Vector3 position)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, m_speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, position) < m_positionCloseThresh)
                m_positionStateEnum = PositionStateEnum.NotMoving;
            else
                m_positionStateEnum = PositionStateEnum.IsMoving;
        }

        public void ReturnHome()
        {
            transform.position = Vector3.MoveTowards(transform.position, m_homePosition, m_speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, m_homePosition) < m_positionCloseThresh)
                m_positionStateEnum = PositionStateEnum.AtHome;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (PositionState != PositionStateEnum.AtHome)
            {
                if (State == SicknessState.Healthy)
                {
                    if (other.gameObject.GetComponent<CitizenBody>())
                    {
                        var otherBody = other.gameObject.GetComponent<CitizenBody>();

                        if (otherBody.PositionState != PositionStateEnum.AtHome &&
                            otherBody.m_state == SicknessState.Infected)
                        {
                            var envObject = GameObject.FindGameObjectWithTag("AgentEnvironment");
                            var env = envObject.GetComponent<AgentEnvironment>();

                            if (env.GetVirusContagiousity())
                            {
                                State = SicknessState.Infected;
                            }
                        }

                        m_socialStress = 0f;
                    }
                }
            }
        }
    }
    
    public enum SicknessState
    {
        Healthy,
        Infected,
        Immuned,
        Dead
    }

    public enum PositionStateEnum
    {
        IsMoving,
        AtHome,
        NotMoving
    }
}