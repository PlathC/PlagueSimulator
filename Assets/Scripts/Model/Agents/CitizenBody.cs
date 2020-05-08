using System;
using System.Collections;
using System.Collections.Generic;
using Model.Data;
using Model.Environment;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
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
            private set
            {
                m_state = value;

                var color = new Color();
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
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                var goRenderer = gameObject.GetComponent<Renderer>();
                if(goRenderer)
                    goRenderer.material.SetColor("_Color", color);
                
                m_environment.NotifyAgentModification(new StorageData(m_state, transform.position));
            }
        }

        public PositionStateEnum PositionState => m_positionStateEnum;

        [SerializeField]
        private float speed = 5f;

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
        private static readonly int ColorProperty = Shader.PropertyToID("_Color");

        private AgentEnvironment m_environment;
        
        // Start is called before the first frame update
        private void Start()
        {
            m_homePosition = gameObject.transform.position;
                        
            var env = GameObject.FindGameObjectWithTag("AgentEnvironment");
            if (!env)
                return;

            var agentEnvironment = env.GetComponent<AgentEnvironment>();
            if (agentEnvironment)
                m_environment = agentEnvironment;
            
            State = Random.Range(0, 10) > 8 ? SicknessState.Infected : SicknessState.Healthy;
            
            m_socialGrowthRate = Random.Range(.05f, .3f);
            m_socialStressThresh = Random.Range(10f, 100f);
        }

        private void Update()
        {
            if (PositionState == PositionStateEnum.IsMoving) return;
            m_socialStress += m_socialGrowthRate;
            m_outStress += 0.1f;
        }

        public void MoveTo(Vector3 position)
        {
            Vector3 positionToCheck = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
            
            if (!m_environment.IsOnMapRelative(positionToCheck))
                return;
            
            transform.position = positionToCheck;
            
            if (Vector3.Distance(transform.position, position) < m_positionCloseThresh)
                m_positionStateEnum = PositionStateEnum.NotMoving;
            else
                m_positionStateEnum = PositionStateEnum.IsMoving;
        }

        public void ReturnHome()
        {
            var positionToCheck = Vector3.MoveTowards(transform.position, m_homePosition, speed * Time.deltaTime);

            if (!m_environment.IsOnMapRelative(positionToCheck))
                return;
            
            transform.position = positionToCheck;
            
            if (Vector3.Distance(transform.position, m_homePosition) < m_positionCloseThresh)
                m_positionStateEnum = PositionStateEnum.AtHome;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (PositionState == PositionStateEnum.AtHome) return;
            if (State != SicknessState.Healthy) return;
            if (!other.gameObject.GetComponent<CitizenBody>()) return;
            
            var otherBody = other.gameObject.GetComponent<CitizenBody>();

            if (otherBody.PositionState != PositionStateEnum.AtHome &&
                otherBody.m_state == SicknessState.Infected)
            {
                if (m_environment.GetVirusContagiousity())
                {
                    State = SicknessState.Infected;
                }
            }

            m_socialStress = 0f;
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