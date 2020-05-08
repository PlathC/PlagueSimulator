using Model.Data;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ui
{
    public class SimulationUi : MonoBehaviour
    {
        [SerializeField]
        private SimulationData simulationData;

        [SerializeField]
        private Text simulationDataText;

        // Use this for initialization
        void Start()
        {
            simulationData = ScriptableObject.FindObjectOfType<SimulationData>();
            simulationDataText.text += $"{simulationData.ToString()} : " +
                                         $"Density {simulationData.populationDensity} | " +
                                         $"Infectivity {simulationData.infectivity} | " +
                                         $"Stress Level {simulationData.stressLevel}";
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
