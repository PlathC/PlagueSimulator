using Model.Data;
using Model.Environment;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ui
{
    public class SimulationUi : MonoBehaviour
    {
        [SerializeField]
        private Text simulationDataText;
        
        [SerializeField]
        private Text currentStatText;

        private string m_bastStatText;
        private SimulationData m_simulationData;

        private AgentEnvironment m_environment;
        
        void Start()
        {
            var env = GameObject.FindGameObjectWithTag("AgentEnvironment");
            if (!env)
                return;

            var agentEnvironment = env.GetComponent<AgentEnvironment>();
            if (agentEnvironment)
                m_environment = agentEnvironment;

            m_simulationData = ScriptableObject.FindObjectOfType<SimulationData>();
            simulationDataText.text += $"{m_simulationData.ToString()} : " +
                                         $"Density {m_simulationData.populationDensity.ToString()} | " +
                                         $"Infectivity {m_simulationData.infectivity.ToString()} | " +
                                         $"Stress Level {m_simulationData.stressLevel.ToString()}";

            m_bastStatText = currentStatText.text;

        }

        void Update()
        {
            currentStatText.text = $"{m_bastStatText} : " +
                                    $"Infected nb {m_environment.SickNumber.ToString()} | " + 
                                    $"Immuned nb {m_environment.ImmunedNumber.ToString()} | " + 
                                    $"Dead nb {m_environment.DeadNumber.ToString()} | ";
        }
    }
}
