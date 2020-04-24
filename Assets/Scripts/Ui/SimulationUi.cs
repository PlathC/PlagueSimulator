using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SimulationUi : MonoBehaviour
{
    [SerializeField]
    private SimulationData m_simulationData;

    [SerializeField]
    private Text m_simulationDataText;

    // Use this for initialization
    void Start()
    {
        m_simulationData = ScriptableObject.FindObjectOfType<SimulationData>();
        m_simulationDataText.text += $"{m_simulationData.ToString()} : " +
            $"Density {m_simulationData.populationDensity} | " +
            $"Infectivity {m_simulationData.infectivity} | " +
            $"Stress Level {m_simulationData.stressLevel}";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
