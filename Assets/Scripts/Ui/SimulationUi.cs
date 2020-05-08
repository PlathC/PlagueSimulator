using Model.Data;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ui
{
    public class SimulationUi : MonoBehaviour
    {
        [SerializeField]
        private Text simulationDataText;

        private SimulationData m_simulationData;

        void Start()
        {
            m_simulationData = ScriptableObject.FindObjectOfType<SimulationData>();
            simulationDataText.text += $"{m_simulationData.ToString()} : " +
                                         $"Density {m_simulationData.populationDensity.ToString()} | " +
                                         $"Infectivity {m_simulationData.infectivity.ToString()} | " +
                                         $"Stress Level {m_simulationData.stressLevel.ToString()}";
        }

        void Update()
        {

        }
    }
}
