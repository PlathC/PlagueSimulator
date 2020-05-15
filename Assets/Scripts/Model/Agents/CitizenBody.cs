using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Model.Data;
using Model.Environment;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Model.Agents
{
    public class CitizenBody : AgentBody
    {
     
        #region SerializeField
        [SerializeField] private GameObject agentDetectionPrefab; 
        private GameObject m_agentDetection;
        private AgentDetection m_detection;

        [SerializeField]
        private float speed = 5f;
        #endregion
        
        #region Stress

        private float m_outStressGrowthRate;
        private float m_outStressThresh;
        private float m_outStress = .0f;

        public float OutStress => m_outStress;
        public float OutStressThresh => m_outStressThresh;

        
        private float m_socialGrowthRate;
        private float m_socialStressThresh;
        private float m_socialStress = .0f;
        
        public float SocialStress => m_socialStress;
        public float SocialThresh => m_socialStressThresh;
        
        #endregion

        #region BodyStates
    
        public enum SicknessState
        {
            Healthy,
            Infected,
            Immuned,
            Dead
        }

        public enum PositionState
        {
            IsMoving,
            ReturningHome,
            AtHome,
            NotMoving
        }
        
        private PositionState m_currentPositionState = PositionState.AtHome;
        public PositionState CurrentPositionState
        {
            get => m_currentPositionState;
            set => m_currentPositionState = value;
        }
        
        private SicknessState m_currentSickness;
        public SicknessState CurrentSickness
        {
            get => m_currentSickness;
            private set
            {
                m_currentSickness = value;

                Color color;
                switch (m_currentSickness)
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
                
                m_environment.NotifyAgentModification(
                    new StorageData(m_currentPositionState, m_currentSickness, transform.position)
                    );
            }
        }
        #endregion
        
        
        private AgentEnvironment m_environment;
        private static readonly int SicknessShader = Shader.PropertyToID("_Color");
        private NavMeshAgent m_navmesh;

        private void Start()
        {
            var env = GameObject.FindGameObjectWithTag("AgentEnvironment");
            if (!env)
                return;

            var agentEnvironment = env.GetComponent<AgentEnvironment>();
            if (agentEnvironment)
                m_environment = agentEnvironment;
            
            CurrentSickness = Random.Range(0, 10) > 8 ? SicknessState.Infected : SicknessState.Healthy;
            
            m_socialGrowthRate = Random.Range(.05f, .3f);
            m_socialStressThresh = Random.Range(10f, 100f);
            
            m_outStressGrowthRate = Random.Range(.05f, .3f);
            m_outStressThresh = Random.Range(10f, 15f);
            
            m_agentDetection = Instantiate(agentDetectionPrefab, transform);
            m_agentDetection.transform.localPosition = new Vector3(0, 0, 0);
            m_detection = m_agentDetection.GetComponent<AgentDetection>();
            m_navmesh = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (CurrentPositionState == PositionState.IsMoving)
            {
                m_outStress -= m_outStressGrowthRate;
                m_outStress = (m_outStress < 0) ? 0 : m_outStress;
            }
            else
            {
                m_outStress += m_outStressGrowthRate;
            }
            
            m_socialStress += m_socialGrowthRate;
        }
        
        public void MoveTo(Vector3 position)
        {
            // TODO: More complex movements such as velocity based  
            if (!m_navmesh)
                return;
            //m_navmesh.destination = position;
            transform.position = Vector3.MoveTowards(transform.position, position, 0.07f);
        }

        public List<CitizenBody> GetClosestAgents()
        {
            if (!m_detection) return null;
            
            var closestAgent = 
                m_detection.CitizenList.Select(agent => new Tuple<float, CitizenBody>(Vector3.Distance(transform.position, agent.transform.position), agent)).ToList();
            closestAgent.Sort((agent1, agent2) => agent2.Item1.CompareTo(agent1.Item1));
            closestAgent.RemoveAll(agent1 => agent1.Item2.m_currentPositionState != PositionState.IsMoving);
            
            return closestAgent.GetRange(0, closestAgent.Count > 10 ? 10 : closestAgent.Count).Select(agent => agent.Item2).ToList();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            if (CurrentPositionState == PositionState.AtHome) return;
            m_socialStress = 0f;

            if (CurrentSickness != SicknessState.Healthy) return;
            
            var otherBody = other.gameObject.GetComponent<CitizenBody>();
            if (!otherBody) return;
            
            if (otherBody.CurrentPositionState != PositionState.AtHome &&
                otherBody.m_currentSickness == SicknessState.Infected)
            {
                if (m_environment.GetVirusContagiosity())
                {
                    CurrentSickness = SicknessState.Infected;
                }
            }

        }
    }
}