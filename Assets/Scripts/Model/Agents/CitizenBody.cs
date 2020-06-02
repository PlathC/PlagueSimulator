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

        [SerializeField] private GameObject agentProximityPrefab;

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

        public enum CauseOfDeath
        {
            Solitude,
            Disease,
            None
        }

        public enum PositionState
        {
            IsMoving,
            ReturningHome,
            AtHome,
            NotMoving
        }

        private CauseOfDeath m_causeOfDeath = CauseOfDeath.None;
        
        private PositionState m_currentPositionState = PositionState.AtHome;
        public PositionState CurrentPositionState
        {
            get => m_currentPositionState;
            set => m_currentPositionState = value;
        }

        private float m_timeAtInfection = -1;
        private SicknessState m_currentSickness;
        public SicknessState CurrentSickness
        {
            get => m_currentSickness;
            set
            {
                SicknessState oldSickness = m_currentSickness;
                m_currentSickness = value;

                Color color;
                switch (m_currentSickness)
                {
                    case SicknessState.Healthy:
                        color = Color.green;
                        break;
                    case SicknessState.Infected:
                        color = Color.red;
                        m_timeAtInfection = Time.time;
                        break;
                    case SicknessState.Immuned:
                        color = Color.blue;
                        break;
                    case SicknessState.Dead:
                        color = Color.black;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                var goRenderer = gameObject.GetComponent<Renderer>();
                if(goRenderer)
                    goRenderer.material.SetColor(SicknessShader, color);
                
                m_environment.NotifyAgentModification(
                    new StorageData(Time.time, m_currentPositionState, oldSickness, m_currentSickness, transform.position, m_causeOfDeath)
                    );
                if(m_currentSickness == SicknessState.Dead)
                    Destroy(gameObject);
            }
        }
        #endregion
        
        private AgentEnvironment m_environment;
        private static readonly int SicknessShader = Shader.PropertyToID("_Color");
        private float m_speed;
        private List<CitizenBody> m_closestAgentsForSickness = new List<CitizenBody>();

        private void Awake()
        {
            var env = GameObject.FindGameObjectWithTag("AgentEnvironment");
            if (!env)
                return;

            var agentEnvironment = env.GetComponent<AgentEnvironment>();
            if (agentEnvironment)
                m_environment = agentEnvironment;
        }

        private void Start()
        {
            m_socialGrowthRate = Random.Range(.001f, .01f);
            m_socialStressThresh = Random.Range(1f, 10f);
            
            m_outStressGrowthRate = Random.Range(.001f, .01f);
            m_outStressThresh = Random.Range(1f, 15f);
            
            m_agentDetection = Instantiate(agentDetectionPrefab, transform);
            m_detection = m_agentDetection.GetComponent<AgentDetection>();

            Instantiate(agentProximityPrefab, transform);

            m_speed = Random.Range(.8f, 2f);
        }

        private void Update()
        {
            if (CurrentPositionState == PositionState.IsMoving)
            {
                m_outStress -= m_outStress/ 2;
                m_outStress = (m_outStress < 0) ? 0 : m_outStress;
            }
            else
            {
                m_outStress += m_outStressGrowthRate;
            }
            
            m_socialStress += m_socialGrowthRate;

            if (m_currentSickness == SicknessState.Infected && m_environment.GetDiseaseDuration() < (Time.time - m_timeAtInfection))
            {
                if (m_environment.ImmunedOrDead())
                    CurrentSickness = SicknessState.Immuned;
                else
                {
                    m_causeOfDeath = CauseOfDeath.Disease;
                    CurrentSickness = SicknessState.Dead;
                }
            }

            //It died of solitude
            if (m_socialStress > m_socialStressThresh * 2)
            {
                m_causeOfDeath = CauseOfDeath.Solitude;
                CurrentSickness = SicknessState.Dead;
            }
                
        }
        
        public void MoveTo(Vector3 position)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, 0.05f*m_speed);
        }

        public List<CitizenBody> GetClosestAgents()
        {
            if (!m_detection) return null;
            m_detection.CitizenList.RemoveAll(agent1 => !agent1);

            var closestAgent = 
                m_detection.CitizenList.Select(selector: agent => new Tuple<float, CitizenBody>(Vector3.Distance(transform.position, agent.transform.position), agent)).ToList();
            closestAgent.Sort((agent1, agent2) => agent2.Item1.CompareTo(agent1.Item1));
            closestAgent.RemoveAll(agent1 => agent1.Item2.m_currentPositionState != PositionState.IsMoving);
            
            return closestAgent.GetRange(0, closestAgent.Count > 10 ? 10 : closestAgent.Count).Select(agent => agent.Item2).ToList();
        }

        private bool m_isBecomeInfectedInvoked = false;

        public void NotifyAgentProximityEnter(CitizenBody other)
        {
            if (!other.CompareTag("Player")) return;
            if (CurrentPositionState == PositionState.AtHome) return;

            m_closestAgentsForSickness.Add(other);
            if (!m_isBecomeInfectedInvoked)
            {
                InvokeRepeating(nameof(IsBecomeInfected), 0f, 1f);
                m_isBecomeInfectedInvoked = true;
            }
        }
        
        public void NotifyAgentProximityExit(CitizenBody other)
        {
            if (!other.CompareTag("Player")) return;
            if (CurrentPositionState == PositionState.AtHome) return;

            m_closestAgentsForSickness.Remove(other);
        }

        private void IsBecomeInfected()
        {
            if (!m_closestAgentsForSickness.Any())
            {
                CancelInvoke(nameof(IsBecomeInfected));
                m_isBecomeInfectedInvoked = false;
                return;
            }

            CitizenBody closestAgent = m_closestAgentsForSickness[0];
            m_closestAgentsForSickness.RemoveAll(agent1 => !agent1);

            try
            {
                foreach (var agent in m_closestAgentsForSickness)
                {
                    if (Vector3.Distance(agent.transform.position, transform.position) <
                        Vector3.Distance(closestAgent.transform.position, transform.position))
                        closestAgent = agent;
                }
            
                float distance = Vector3.Distance(closestAgent.transform.position, transform.position);
                
                if (closestAgent.CurrentPositionState != PositionState.AtHome)
                {
                    m_socialStress -= m_socialStress * 1f/(float)Math.Exp(distance);
                    if (m_socialStress < 1)
                        m_socialStress = 0;
                }
                
                if (CurrentSickness != SicknessState.Healthy) return;
            
                if (closestAgent.CurrentPositionState != PositionState.AtHome &&
                    closestAgent.m_currentSickness == SicknessState.Infected)// && distance)
                {
                    if (m_environment.GetVirusContagiosity(distance))
                    {
                        CurrentSickness = SicknessState.Infected;
                    }
                }
            }
            catch (MissingReferenceException e)
            {
                // ignored
            }
        }
    }
}