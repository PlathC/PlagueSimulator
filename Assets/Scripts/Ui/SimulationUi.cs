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
        private Text currentCitizensStatText;

        [SerializeField]
        private Text currentMayorStatText;

        private string m_bastCitizensStatText;
        private string m_bastMayorStatText;
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
            simulationDataText.text += "\n" +
                                       $"Density : {m_simulationData.populationDensity.ToString()} \n " +
                                       $"Infectivity : {m_simulationData.infectivity.ToString()} \n " +
                                       $"Launch Sick nb : {m_simulationData.launchSickNumber.ToString()} \n " +
                                       $"Launch Immuned nb :  {m_simulationData.launchImmunedNumber.ToString()} \n " +
                                       $"Disease duration :  {m_simulationData.diseaseDuration.ToString()} \n " +
                                       $"Death statistic :  {m_simulationData.deathStatistic.ToString()} \n " +
                                       $"Disease transmission distance :  {m_simulationData.diseaseTransmissionDistance.ToString()} \n ";

            m_bastCitizensStatText = currentCitizensStatText.text;
            m_bastMayorStatText = currentMayorStatText.text;

        }

        void Update()
        {
            currentCitizensStatText.text = $"{m_bastCitizensStatText} : " +
                                           $"Infected nb {m_environment.SickNumber.ToString()} | " + 
                                           $"Immuned nb {m_environment.ImmunedNumber.ToString()} | " + 
                                           $"Dead nb {m_environment.DeadNumber.ToString()} | ";

            currentMayorStatText.text = $"{m_bastMayorStatText}" +
                                        $"Maximum Time outside: {m_environment.MaximumTimeOutside.ToString()} | " +
                                        $"Social distancing rate: {m_environment.SocialDistancing.ToString()}";
        }
    }
}
